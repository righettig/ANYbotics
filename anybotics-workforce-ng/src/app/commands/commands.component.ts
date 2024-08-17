import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AgentService } from '../services/agent.service';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-commands',
  standalone: true,
  imports: [MatButtonModule, RouterModule, MatListModule],
  templateUrl: './commands.component.html',
  styleUrl: './commands.component.scss'
})
export class CommandsComponent implements OnInit {
  public agentId!: string | null;

  constructor(
    private route: ActivatedRoute,
    private agentService: AgentService
  ) {}

  ngOnInit(): void {
    this.agentId = this.route.snapshot.paramMap.get('id');
  }

  shutdown() {
    this.agentService.shutdownAgent(this.agentId!);
  }

  recharge() {
    this.agentService.rechargeAgent(this.agentId!);
  }
  
  wakeup() {
    this.agentService.wakeupAgent(this.agentId!);
  }
  
  setManualMode() {
    this.agentService.setManualMode(this.agentId!, true); // TODO: hardcoded
  }
  
  thermalInspection() {
    this.agentService.thermalInspection(this.agentId!);
  }
  
  combustibleInspection() {
    this.agentService.combustibleInspection(this.agentId!);
  }
  
  gasInspection() {
    this.agentService.gasInspection(this.agentId!);
  }
  
  acousticMeasure() {
    this.agentService.acousticMeasure(this.agentId!);
  }

  moveLeft() {
    this.agentService.moveLeft(this.agentId!);
  }

  moveRight() {
    this.agentService.moveRight(this.agentId!);
  }

  moveForward() {
    this.agentService.moveForward(this.agentId!);
  }

  moveBackward() {
    this.agentService.moveBackward(this.agentId!);
  }
}
