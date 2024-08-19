import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AgentDto } from '../models/agent-dto.model';
import { AgentDetailsDto } from '../models/agent-details-dto.model';
import { HttpService } from '../http.service';

import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class AgentService {
  private hubConnection: signalR.HubConnection;

  private agentsSubject = new BehaviorSubject<AgentDto[]>([]);
  private agentSubject = new BehaviorSubject<AgentDetailsDto | null>(null);
  private anomalyDetectedSubject = new BehaviorSubject<any>(null);
  private hardwareFailureSubject = new BehaviorSubject<any>(null);
  
  private baseUrl = 'https://localhost:7272';
  private baseApiUrl = `${this.baseUrl}/Anymal`;

  agents$ = this.agentsSubject.asObservable();
  agent$ = this.agentSubject.asObservable();
  anomalyDetected$ = this.anomalyDetectedSubject.asObservable();
  hardwareFailure$ = this.hardwareFailureSubject.asObservable();

  constructor(private http: HttpService) {
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

    this.hubConnection.on('AnomalyDetected', (data: any) => {
      this.anomalyDetectedSubject.next(data);
    });

    this.hubConnection.on('HardwareFailure', (data: any) => {
      this.hardwareFailureSubject.next(data);
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
    const url = `${this.baseApiUrl}/rechargeBattery`;
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

  async thermalInspection(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/thermalInspection`;
    await this.performAction(url, id);
  }

  async combustibleInspection(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/combustibleInspection`;
    await this.performAction(url, id);
  }

  async gasInspection(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/gasInspection`;
    await this.performAction(url, id);
  }

  async acousticMeasure(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/acousticMeasure`;
    await this.performAction(url, id);
  }

  async setManualMode(id: string, manualMode: boolean): Promise<void> {
    const url = `${this.baseApiUrl}/setManualMode`;

    try {
      const response = await this.http.fetch(url, {
        method: 'POST',
        body: JSON.stringify({
          id,
          manualMode,
        }),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
    } catch (error) {
      console.error('Error performing action:', error);
      throw error;
    }
  }

  async moveLeft(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/moveLeft`;
    await this.performAction(url, id);
  }

  async moveRight(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/moveRight`;
    await this.performAction(url, id);
  }

  async moveForward(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/moveForward`;
    await this.performAction(url, id);
  }

  async moveBackward(id: string): Promise<void> {
    const url = `${this.baseApiUrl}/moveBackward`;
    await this.performAction(url, id);
  }

  private async performAction(url: string, id: string): Promise<void> {
    try {
      const response = await this.http.fetch(url, {
        method: 'POST',
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
