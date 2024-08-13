import { Component, OnInit } from '@angular/core';
import { AgentDto, AgentService } from '../services/agent.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [NgFor, MatCardModule, MatIconModule],
  templateUrl: './agents.component.html',
  styleUrl: './agents.component.scss',
})
export class AgentsComponent implements OnInit {
  agents: AgentDto[] = [];

  constructor(private agentService: AgentService) {}

  ngOnInit(): void {
    this.agentService.agents$.subscribe((agents) => {
      this.agents = agents;
    });
  }

  rechargeAgent(id: string) {
    console.log(`Recharge agent with ID: ${id}`);
  }

  shutdownAgent(id: string) {
    console.log(`Shutdown agent with ID: ${id}`);
  }

  wakeupAgent(id: string) {
    console.log(`Wakeup agent with ID: ${id}`);
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
