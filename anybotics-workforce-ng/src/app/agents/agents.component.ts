import { Component, OnInit } from '@angular/core';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { AgentCardComponent } from '../agent-card/agent-card.component';
import { SearchComponent } from '../search/search.component';

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [AgentCardComponent, SearchComponent],
  templateUrl: './agents.component.html',
  styleUrls: ['./agents.component.scss'],
})
export class AgentsComponent implements OnInit {
  agents: AgentDto[] = [];
  filteredAgents: AgentDto[] = [];
  currentSearchTerm: string = '';

  constructor(private agentService: AgentService) {}

  ngOnInit(): void {
    this.agentService.agents$.subscribe((agents) => {
      this.agents = agents;
      this.applyFilter();
    });
  }

  onSearch(searchTerm: string): void {
    this.currentSearchTerm = searchTerm;
    this.applyFilter();
  }

  clearSearch(): void {
    this.currentSearchTerm = '';
    this.filteredAgents = this.agents;
  }

  private applyFilter(): void {
    if (!this.currentSearchTerm) {
      this.filteredAgents = this.agents;
    } else {
      this.filteredAgents = this.agents.filter(
        (agent) =>
          agent.id.includes(this.currentSearchTerm) ||
          agent.name.toLowerCase().includes(this.currentSearchTerm.toLowerCase())
      );
    }
  }
}
