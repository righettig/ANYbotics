import { Controller, Get, Post, Put, Delete, Body, Param } from '@nestjs/common';
import { Event } from './event.interface';
import { EventsService } from './events.service';

@Controller('events')
export class EventsController {
    constructor(private readonly eventsService: EventsService) { }

    @Post()
    async create(@Body() event: Event) {
        return this.eventsService.createEvent(event);
    }

    @Get(':eventId')
    async read(@Param('eventId') eventId: string) {
        return this.eventsService.readEvent(eventId);
    }

    @Put(':eventId')
    async update(
        @Param('eventId') eventId: string,
        @Body() updatedEvent: Partial<Event>
    ) {
        return this.eventsService.updateEvent(eventId, updatedEvent);
    }

    @Delete(':eventId')
    async delete(@Param('eventId') eventId: string) {
        return this.eventsService.deleteEvent(eventId);
    }

    @Get()
    async list() {
        return this.eventsService.listEvents();
    }
}
