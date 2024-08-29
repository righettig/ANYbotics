import { useState } from 'react';

import Event from '../types/event';
import EventCreator from './event-creator';
import EventList from './event-list';
import mockEvents from '../data/events';

const Events = () => {
    const [eventList, setEventList] = useState<Event[]>(mockEvents);
    const [eventToEdit, setEventToEdit] = useState<Event | null>(null);

    const handleAddEvent = (newEvent: Event) => {
        setEventList([...eventList, newEvent]);
    };

    const handleUpdateEvent = (updatedEvent: Event) => {
        setEventList(eventList.map(event =>
            event.id === updatedEvent.id ? updatedEvent : event
        ));
        setEventToEdit(null);
    };

    const handleDeleteEvent = (id: string) => {
        setEventList(eventList.filter(event => event.id !== id));
    };

    const handleEditEvent = (event: Event) => {
        setEventToEdit(event);
    };

    const handleStartEvent = (updatedEvent: Event) => {
        setEventList(eventList.map(event =>
            event.id === updatedEvent.id ? updatedEvent : event
        ));
    };

    return (
        <div>
            <h1>Event Management</h1>
            <EventCreator
                onAdd={handleAddEvent}
                onUpdate={handleUpdateEvent}
                eventToEdit={eventToEdit}
            />
            <EventList
                events={eventList}
                onDelete={handleDeleteEvent}
                onEdit={handleEditEvent}
                onStart={handleStartEvent}
            />
        </div>
    );
};

export default Events;
