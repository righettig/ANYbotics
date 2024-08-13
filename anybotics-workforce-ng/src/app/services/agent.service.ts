import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import * as signalR from '@microsoft/signalr';

export interface AgentDto {
  id: string;
  name: string;
  batteryLevel: number;
  status: string;
}

@Injectable({
  providedIn: 'root',
})
export class AgentService {
  private hubConnection: signalR.HubConnection;
  private agentsSubject = new BehaviorSubject<AgentDto[]>([]);

  agents$ = this.agentsSubject.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7272/agentsHub', { withCredentials: false })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.startConnection();

    this.hubConnection.on('ReceiveAgentsData', (agents: AgentDto[]) => {
      this.agentsSubject.next(agents);
    });

    this.hubConnection.onreconnected(() => {
      this.startStreaming();
    });
  }

  startConnection() {
    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected');
        this.startStreaming();
      })
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  private startStreaming() {
    this.hubConnection
      .invoke('StreamAgentsData')
      .catch((err) => console.error('Error while starting the stream', err));
  }
}
