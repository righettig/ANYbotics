import { Component, OnDestroy, OnInit } from '@angular/core';
import { AgentService } from '../services/agent.service';
import { AgentDto } from '../models/agent-dto.model';
import { AgentCardComponent } from '../agent-card/agent-card.component';
import { combineLatest, Subscription } from 'rxjs';
import { SearchService } from '../services/search.service';
import { SortingService, SortOption } from '../services/sorting.service';

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [AgentCardComponent],
  templateUrl: './agents.component.html',
  styleUrls: ['./agents.component.scss'],
})
export class AgentsComponent implements OnInit, OnDestroy {
  agents: AgentDto[] = [];
  filteredAgents: AgentDto[] = [];

  private subscription!: Subscription;

  constructor(
    private agentService: AgentService,
    private searchService: SearchService,
    private sortingService: SortingService
  ) {}

  ngOnInit(): void {
    // Start streaming real-time updates for the agent
    this.agentService.startAgentsStreaming();

    this.subscription = combineLatest([
      this.agentService.agents$,
      this.searchService.searchTerm$,
      this.sortingService.sortOption$,
    ]).subscribe(([agents, searchTerm, sortOption]) => {
      this.agents = agents;
      this.applyFilterAndSort(searchTerm, sortOption);
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    // Stop the connection when the component is destroyed
    this.agentService.stopConnection();
  }

  private applyFilterAndSort(searchTerm: string, sortOption: SortOption): void {
    let filtered = this.agents;

    if (searchTerm) {
      filtered = filtered.filter(
        (agent) =>
          agent.id.includes(searchTerm) ||
          agent.name.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    this.filteredAgents = this.sortAgents(filtered, sortOption);
  }

  private sortAgents(agents: AgentDto[], sortOption: SortOption): AgentDto[] {
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
