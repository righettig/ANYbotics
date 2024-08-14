import { AgentDetailsDto } from '../models/agent-details-dto.model';
import { Status } from '../models/status.enum';

export const agentDetailsStub: AgentDetailsDto = {
  id: '6782cf85-09d3-4aef-8ca2-bc2daea1befa',
  name: 'AgentAlpha',
  batteryLevel: 75,
  status: Status.Active,
  general: {
    currentCommand: 'Move Forward',
    model: 'ANYmal X',
    firmwareVersion: '1.2.3',
    firmwareLastUpdated: new Date('2024-07-15T12:00:00Z'),
    manualModeOn: false,
    location: { x: 123.45, y: 67.89, z: 10.11 },
    trekkerVersion: '2.3.4',
    trekkerLastUpdated: new Date('2024-07-14T15:00:00Z'),
  },
  hardware: {
    temperatureSensor: 'Running',
    pressureSensor: 'Anomaly_detected',
    leg1Status: 'Running',
    leg2Status: 'Running',
    leg3Status: 'Failed',
    leg4Status: 'Running',
    gps: 'Running',
    engine: 'Running',
    battery: 'Anomaly_detected',
    lidarScanner: 'Running',
    wifi: 'Failed',
    lte: 'Running',
    cpu1: 'Running',
    cpu2: 'Running',
    depthCameras: [
      'Running',
      'Running',
      'Failed',
      'Running',
      'Running',
      'Running',
    ],
    opticalCameras: ['Running', 'Anomaly_detected'],
    thermalCamera: 'Running',
    panTiltUnit: 'Failed',
    spotlight: 'Running',
    ultrasonicMicrophone: 'Running',
  },
  recentImages: [
    'https://placehold.co/400?text=Room\n1',
    'https://placehold.co/400?text=Room\n2',
    'https://placehold.co/400?text=Room\n3',
    'https://placehold.co/400?text=Room\n4',
    'https://placehold.co/400?text=Room\n5',
  ],
  commandHistory: [
    'Initialized system',
    'Started navigation',
    'Executed pathfinding',
    'Battery check completed',
  ],
  statusHistory: [
    { timestamp: new Date('2024-08-10T08:00:00Z'), status: 'Operational' },
    {
      timestamp: new Date('2024-08-11T09:30:00Z'),
      status: 'Minor issue detected',
    },
    { timestamp: new Date('2024-08-12T10:45:00Z'), status: 'Operational' },
    {
      timestamp: new Date('2024-08-13T11:50:00Z'),
      status: 'Maintenance required',
    },
  ],
};
