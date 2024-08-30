import { createApiService } from './api.service';

const EVENTS_API_URL = 'http://localhost:3001/events';

const eventsService = createApiService(EVENTS_API_URL);

export const fetchEvents = () =>
    eventsService.request('', 'GET');

export const addEvent = (event: any) => // Replace 'any' with your Event type
    eventsService.request('', 'POST', event);

export const updateEvent = (event: any) => // Replace 'any' with your Event type
    eventsService.request('', 'PUT', event);

export const deleteEvent = (id: string) =>
    eventsService.request(`/${id}`, 'DELETE');

export const startEvent = (id: string) =>
    eventsService.request(`/${id}`, 'POST');
