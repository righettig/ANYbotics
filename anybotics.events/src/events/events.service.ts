import { Injectable, OnModuleInit, OnModuleDestroy } from '@nestjs/common';
import { CosmosClient, Container, Database } from '@azure/cosmos';
import { Event } from './event.interface';

import * as https from 'https';

// Create an HTTPS agent that accepts self-signed certificates
const agent = new https.Agent({ rejectUnauthorized: false });

@Injectable()
export class EventsService implements OnModuleInit, OnModuleDestroy {
  private client: CosmosClient;
  private database: Database;
  private container: Container;

  constructor() {
    this.client = new CosmosClient({
      endpoint: 'https://localhost:8081', // Cosmos DB Emulator endpoint
      key: 'C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==',
      agent
    });
  }

  async onModuleInit() {
    this.database = this.client.database('anybotics');
    this.container = this.database.container('events');
  }

  async createEvent(event: Event) {
    const { resource } = await this.container.items.create(event);
    return resource;
  }

  async readEvent(eventId: string) {
    const { resource } = await this.container.item(eventId).read();
    return resource;
  }

  async updateEvent(eventId: string, updatedEvent: Partial<Event>) {
    const { resource } = await this.container.item(eventId).read();
    if (resource) {
      const updated = { ...resource, ...updatedEvent };
      const { resource: updatedResource } = await this.container.item(eventId).replace(updated);
      return updatedResource;
    }
    throw new Error('Event not found');
  }

  async deleteEvent(eventId: string) {
    await this.container.item(eventId).delete();
    return { message: 'Event deleted successfully' };
  }

  async listEvents() {
    const { resources } = await this.container.items.query('SELECT * FROM c').fetchAll();
    return resources;
  }

  async onModuleDestroy() {
    this.client.dispose();
  }
}
