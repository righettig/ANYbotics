import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { AgentBatteryLevelComponent } from '../agent-battery-level/agent-battery-level.component';
import { AgentStatusComponent } from '../agent-status/agent-status.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-agent-details',
  standalone: true,
  imports: [RouterModule, AgentBatteryLevelComponent, AgentStatusComponent],
  templateUrl: './agent-details.component.html',
  styleUrls: ['./agent-details.component.scss'],
})
export class AgentDetailsComponent implements OnInit, OnDestroy {
  agent?: AgentDto;

  private subscription!: Subscription;

  constructor(
    private route: ActivatedRoute,
    private agentService: AgentService
  ) {}

  ngOnInit(): void {
    const agentId = this.route.snapshot.paramMap.get('id');

    // Start streaming real-time updates for the agent
    this.agentService.startAgentStreaming(agentId!);

    // Subscribe to real-time updates
    this.subscription = this.agentService.agent$.subscribe((agent) => {
      if (agent && agent.id === agentId) {
        this.agent = agent;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    // Stop the connection when the component is destroyed
    this.agentService.stopConnection();
  }
}
