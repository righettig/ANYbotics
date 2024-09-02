import { createApiService } from './api.service';

import { EventDto } from '@/app/features/events/types/event';

const EVENTS_API_URL = process.env.EVENTS_API_URL!;

const eventsService = createApiService(EVENTS_API_URL);

export const fetchEvents = () =>
    eventsService.request('', 'GET');

export const addEvent = (event: EventDto) =>
    eventsService.request('', 'POST', event);

export const updateEvent = (id: string, event: EventDto) =>
    eventsService.request(`${id}`, 'PUT', event);

export const deleteEvent = (id: string) =>
    eventsService.request(`${id}`, 'DELETE');

export const startEvent = (id: string) =>
    eventsService.request(`${id}`, 'POST');
