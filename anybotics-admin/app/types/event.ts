export type EventStatus = 'NotStarted' | 'Running' | 'Finished';

export interface Event {
    id: string;
    name: string;
    description: string;
    createdAt: Date;
    modifiedAt: Date;
    status: EventStatus;
}
