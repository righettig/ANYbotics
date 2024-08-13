import { Component, OnInit } from '@angular/core';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { AgentCardComponent } from '../agent-card/agent-card.component';
import { SearchComponent } from '../search/search.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [
    AgentCardComponent,
    SearchComponent,
    MatFormFieldModule,
    MatSelectModule,
  ],
  templateUrl: './agents.component.html',
  styleUrls: ['./agents.component.scss'],
})
export class AgentsComponent implements OnInit {
  agents: AgentDto[] = [];
  filteredAgents: AgentDto[] = [];
  currentSearchTerm: string = '';
  selectedSortOption: string = 'name-asc'; // Default sort option

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

  onSortChange(sortOption: string): void {
    this.selectedSortOption = sortOption;
    this.applyFilter();
  }

  private applyFilter(): void {
    if (!this.currentSearchTerm) {
      this.filteredAgents = this.agents;
    } else {
      this.filteredAgents = this.agents.filter(
        (agent) =>
          agent.id.includes(this.currentSearchTerm) ||
          agent.name
            .toLowerCase()
            .includes(this.currentSearchTerm.toLowerCase())
      );
    }
    this.sortAgents();
  }

  private sortAgents(): void {
    switch (this.selectedSortOption) {
      case 'name-asc':
        this.filteredAgents.sort((a, b) => a.name.localeCompare(b.name));
        break;
      case 'name-desc':
        this.filteredAgents.sort((a, b) => b.name.localeCompare(a.name));
        break;
      case 'battery-asc':
        this.filteredAgents.sort((a, b) => a.batteryLevel - b.batteryLevel);
        break;
      case 'battery-desc':
        this.filteredAgents.sort((a, b) => b.batteryLevel - a.batteryLevel);
        break;
    }
  }
}
