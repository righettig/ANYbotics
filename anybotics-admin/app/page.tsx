'use client';

import { useState } from 'react';

import Event from './features/events/types/event';
import EventCreator from './features/events/components/EventCreator';
import EventList from './features/events/components/EventList';
import mockEvents from './features/events/data/events';

const Home = () => {
  const [eventList, setEventList] = useState<Event[]>(mockEvents);

  const handleAddEvent = (newEvent: Event) => {
    setEventList([...eventList, newEvent]);
  };

  const handleDeleteEvent = (id: string) => {
    setEventList(eventList.filter(event => event.id !== id));
  };

  return (
    <div>
      <h1>Event Management</h1>
      <EventCreator onAdd={handleAddEvent} />
      <EventList events={eventList} onDelete={handleDeleteEvent} />
    </div>
  );
};

export default Home;
