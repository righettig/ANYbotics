import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-agent-status',
  standalone: true,
  imports: [],
  templateUrl: './agent-status.component.html',
  styleUrl: './agent-status.component.scss',
})
export class AgentStatusComponent {
  @Input() status!: string;
}
