import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AgentService } from '../services/agent.service';
import { AgentDetailsDto } from '../models/agent-details-dto.model';
import { AgentBatteryLevelComponent } from '../agent-battery-level/agent-battery-level.component';
import { AgentStatusComponent } from '../agent-status/agent-status.component';
import { Subscription } from 'rxjs';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { agentDetailsStub } from '../test-stubs/agent-details-stub';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-agent-details',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    DatePipe,
    MatCardModule,
    MatTabsModule,
    MatButtonModule,
    MatTableModule,
    RouterModule,
    CommonModule,
    AgentBatteryLevelComponent,
    AgentStatusComponent,
  ],
  templateUrl: './agent-details.component.html',
  styleUrls: ['./agent-details.component.scss'],
})
export class AgentDetailsComponent implements OnInit, OnDestroy {
  agent?: AgentDetailsDto;
  hardwareItems: { name: string; status: string }[] = [];
  statusHistoryDataSource!: MatTableDataSource<{ timestamp: Date; status: string }>;
  
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
        //this.agent = agent;
        this.agent = agentDetailsStub;
        this.hardwareItems = this.createHardwareItems(this.agent.hardware);
        this.statusHistoryDataSource = new MatTableDataSource(this.agent.statusHistory);
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

  createHardwareItems(hardware: any): { name: string; status: string }[] {
    return [
      { name: 'Temperature Sensor', status: hardware.temperatureSensor },
      { name: 'Pressure Sensor', status: hardware.pressureSensor },
      { name: 'Leg 1 Status', status: hardware.leg1Status },
      { name: 'Leg 2 Status', status: hardware.leg2Status },
      { name: 'Leg 3 Status', status: hardware.leg3Status },
      { name: 'Leg 4 Status', status: hardware.leg4Status },
      { name: 'GPS', status: hardware.gps },
      { name: 'Engine', status: hardware.engine },
      { name: 'Battery', status: hardware.battery },
      { name: 'Lidar Scanner', status: hardware.lidarScanner },
      { name: 'Wi-Fi', status: hardware.wifi },
      { name: '4G LTE', status: hardware.lte },
      { name: 'CPU 1', status: hardware.cpu1 },
      { name: 'CPU 2', status: hardware.cpu2 },
      { name: 'Depth Cameras', status: hardware.depthCameras.join(', ') },
      { name: 'Optical Cameras', status: hardware.opticalCameras.join(', ') },
      { name: 'Thermal Camera', status: hardware.thermalCamera },
      { name: 'Pan-Tilt Unit', status: hardware.panTiltUnit },
      { name: 'Spotlight', status: hardware.spotlight },
      { name: 'Ultrasonic Microphone', status: hardware.ultrasonicMicrophone },
    ];
  }
}
