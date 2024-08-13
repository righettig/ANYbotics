import { Component, Input } from '@angular/core';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AgentBatteryLevelComponent } from '../agent-battery-level/agent-battery-level.component';
import { AgentStatusComponent } from '../agent-status/agent-status.component';

@Component({
  selector: 'app-agent-card',
  standalone: true,
  imports: [
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    AgentBatteryLevelComponent,
    AgentStatusComponent,
  ],
  templateUrl: './agent-card.component.html',
  styleUrls: ['./agent-card.component.scss'],
})
export class AgentCardComponent {
  @Input() agent!: AgentDto;

  constructor(private agentService: AgentService) {}

  async onRecharge(): Promise<void> {
    try {
      await this.agentService.rechargeAgent(this.agent.id);
      console.log(`Agent ${this.agent.name} recharged successfully.`);
    } catch (error) {
      console.error(`Failed to recharge agent ${this.agent.name}:`, error);
    }
  }

  async onShutdown(): Promise<void> {
    try {
      await this.agentService.shutdownAgent(this.agent.id);
      console.log(`Agent ${this.agent.name} shut down successfully.`);
    } catch (error) {
      console.error(`Failed to shut down agent ${this.agent.name}:`, error);
    }
  }

  async onWakeup(): Promise<void> {
    try {
      await this.agentService.wakeupAgent(this.agent.id);
      console.log(`Agent ${this.agent.name} woke up successfully.`);
    } catch (error) {
      console.error(`Failed to wake up agent ${this.agent.name}:`, error);
    }
  }

  copyToClipboard(id: string) {
    navigator.clipboard
      .writeText(id)
      .then(() => {
        console.log(`ID ${id} copied to clipboard`);
      })
      .catch((err) => {
        console.error('Failed to copy: ', err);
      });
  }
}
