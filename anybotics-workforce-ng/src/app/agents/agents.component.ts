import { Component, OnInit } from '@angular/core';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { NgFor } from '@angular/common';
import { AgentCardComponent } from "../agent-card/agent-card.component";

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [NgFor, AgentCardComponent],
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
}
