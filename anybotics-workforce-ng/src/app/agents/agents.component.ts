import { Component, OnDestroy, OnInit } from '@angular/core';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { AgentCardComponent } from '../agent-card/agent-card.component';
import { SearchComponent } from '../search/search.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { SortingComponent } from '../sorting/sorting.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [
    AgentCardComponent,
    SearchComponent,
    SortingComponent,
    MatFormFieldModule,
    MatSelectModule,
  ],
  templateUrl: './agents.component.html',
  styleUrls: ['./agents.component.scss'],
})
export class AgentsComponent implements OnInit, OnDestroy {
  agents: AgentDto[] = [];
  filteredAgents: AgentDto[] = [];
  currentSearchTerm: string = '';
  sortOption: string = 'nameAsc';

  private subscription!: Subscription;

  constructor(private agentService: AgentService) {}

  ngOnInit(): void {
    // Start streaming real-time updates for the agent
    this.agentService.startAgentsStreaming();

    this.subscription = this.agentService.agents$.subscribe((agents) => {
      this.agents = agents;
      this.applyFilterAndSort();
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    // Stop the connection when the component is destroyed
    this.agentService.stopConnection();
  }

  onSearch(searchTerm: string): void {
    this.currentSearchTerm = searchTerm;
    this.applyFilterAndSort();
  }

  clearSearch(): void {
    this.currentSearchTerm = '';
    this.filteredAgents = this.agents;
  }

  onSortChange(sortOption: string): void {
    this.sortOption = sortOption;
    this.applyFilterAndSort();
  }

  private applyFilterAndSort(): void {
    let filtered = this.agents;

    if (this.currentSearchTerm) {
      filtered = filtered.filter(
        (agent) =>
          agent.id.includes(this.currentSearchTerm) ||
          agent.name
            .toLowerCase()
            .includes(this.currentSearchTerm.toLowerCase())
      );
    }

    this.filteredAgents = this.sortAgents(filtered, this.sortOption);
  }

  private sortAgents(agents: AgentDto[], sortOption: string): AgentDto[] {
    return agents.sort((a, b) => {
      switch (sortOption) {
        case 'nameAsc':
          return a.name.localeCompare(b.name);
        case 'nameDesc':
          return b.name.localeCompare(a.name);
        case 'batteryAsc':
          return a.batteryLevel - b.batteryLevel;
        case 'batteryDesc':
          return b.batteryLevel - a.batteryLevel;
        default:
          return 0;
      }
    });
  }
}
