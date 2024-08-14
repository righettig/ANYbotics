import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AgentDto } from '../models/agent-dto.model';
import { AgentDetailsDto } from '../models/agent-details-dto.model';

import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class AgentService {
  private hubConnection: signalR.HubConnection;

  private agentsSubject = new BehaviorSubject<AgentDto[]>([]);
  private agentSubject = new BehaviorSubject<AgentDetailsDto | null>(null);

  private baseUrl = 'https://localhost:7272';
  private baseApiUrl = `${this.baseUrl}/Anymal`;

  agents$ = this.agentsSubject.asObservable();
  agent$ = this.agentSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/agentsHub`, { withCredentials: false })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection.on('ReceiveAgentsData', (agents: AgentDto[]) => {
      this.agentsSubject.next(agents);
    });

    this.hubConnection.on('ReceiveAgentData', (agent: AgentDetailsDto) => {
      this.agentSubject.next(agent);
    });
  }

  startAgentsStreaming() {
    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected');

        this.hubConnection
          .invoke('StreamAgentsData')
          .catch((err) =>
            console.error('Error while starting the stream', err)
          );
      })
      .catch((err) => console.log('Error while starting connection: ' + err));

    this.hubConnection.onreconnected(() => {
      this.hubConnection
        .invoke('StreamAgentsData')
        .catch((err) => console.error('Error while starting the stream', err));
    });
  }
  
  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .then(() => console.log('SignalR Disconnected'))
        .catch((err) => console.log('Error while stopping connection: ' + err));
    }
  }

  startAgentStreaming(id: string) {
    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected');

        this.hubConnection
          .invoke('StreamAgentData', id)
          .catch((err) =>
            console.error('Error while starting the agent stream', err)
          );
      })
      .catch((err) => console.log('Error while starting connection: ' + err));

    this.hubConnection.onreconnected(() => {
      this.hubConnection
        .invoke('StreamAgentData', id)
        .catch((err) =>
          console.error('Error while starting the agent stream', err)
        );
    });
  }

  async rechargeAgent(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/recharge`;
    await this.performAction(url, id);
  }

  async shutdownAgent(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/shutdown`;
    await this.performAction(url, id);
  }

  async wakeupAgent(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/wakeup`;
    await this.performAction(url, id);
  }

  private async performAction(url: string, id: string): Promise<void> {
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(id),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
    } catch (error) {
      console.error('Error performing action:', error);
      throw error;
    }
  }
}
