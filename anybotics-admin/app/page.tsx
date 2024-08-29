'use client';

import { useState } from 'react';

import Event from './features/events/types/event';
import EventCreator from './features/events/components/event-creator';
import EventList from './features/events/components/event-list';
import mockEvents from './features/events/data/events';

const Home = () => {
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
      />
    </div>
  );
};

export default Home;
