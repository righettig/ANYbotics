'use client';

import { useState } from 'react';
import { Event } from './types/event';

import mockEvents from './data/events';

const Home = () => {
  const [eventList, setEventList] = useState<Event[]>(mockEvents);
  const [newEventName, setNewEventName] = useState('');
  const [newEventDescription, setNewEventDescription] = useState('');

  const addEvent = () => {
    const newEvent: Event = {
      id: (eventList.length + 1).toString(),
      name: newEventName,
      description: newEventDescription,
      createdAt: new Date(),
      modifiedAt: new Date(),
      status: 'NotStarted',
    };
    setEventList([...eventList, newEvent]);
    setNewEventName('');
    setNewEventDescription('');
  };

  const deleteEvent = (id: string) => {
    setEventList(eventList.filter(event => event.id !== id));
  };

  return (
    <div>
      <h1>Event Management</h1>
      <h2>Create New Event</h2>
      <input
        type="text"
        placeholder="Event Name"
        value={newEventName}
        onChange={(e) => setNewEventName(e.target.value)}
      />
      <input
        type="text"
        placeholder="Event Description"
        value={newEventDescription}
        onChange={(e) => setNewEventDescription(e.target.value)}
      />
      <button onClick={addEvent}>Add Event</button>

      <h2>Event List</h2>
      <ul>
        {eventList.map(event => (
          <li key={event.id} className="event-item">
            <div className="event-details">
              <strong>{event.name}</strong>
              <p>{event.description}</p>
            </div>
            <div className="event-meta">
              <p>Status: {event.status}</p>
              <p>Created At: {new Date(event.createdAt).toLocaleString()}</p>
            </div>
            <button onClick={() => deleteEvent(event.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Home;
