import { Injectable } from '@angular/core';
import { HttpService } from './http.service';

export interface Mission {
  id?: string;
  name: string;
  commands: string[];
}

@Injectable({
  providedIn: 'root',
})
export class MissionsService {
  private baseUrl = 'https://localhost:7272';

  private commandsApiUrl = `${this.baseUrl}/commands`;
  private missionsApiUrl = `${this.baseUrl}/missions`;

  constructor(private http: HttpService) {}

  async getCommands(): Promise<string[]> {
    const response = await this.http.fetch(this.commandsApiUrl, {
      method: 'GET',
    });
    return response.json();
  }

  async getMissions(): Promise<Mission[]> {
    const response = await this.http.fetch(this.missionsApiUrl, {
      method: 'GET',
    });
    return response.json();
  }

  async createMission(mission: Mission): Promise<Mission> {
    const response = await this.http.fetch(this.missionsApiUrl + '/create', {
      method: 'POST',
      body: JSON.stringify(mission),
    });
    return response.json();
  }

  async deleteMission(missionid: string): Promise<void> {
    await this.http.fetch(this.missionsApiUrl + `?missionId=${missionid}`, {
      method: 'DELETE',
    });
  }

  async executeMission(agentId: string, missionId: string): Promise<void> {
    await this.http.fetch(this.missionsApiUrl + '/execute', {
      method: 'POST',
      body: JSON.stringify({ agentId, missionId }),
    });
  }
}
