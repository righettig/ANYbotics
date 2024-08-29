import { FC, useState } from 'react';

import Event from '../types/event';

interface EventCreatorProps {
    onAdd: (event: Event) => void;
}

const EventCreator: FC<EventCreatorProps> = ({ onAdd }) => {
    const [newEventName, setNewEventName] = useState('');
    const [newEventDescription, setNewEventDescription] = useState('');

    const handleAddEvent = () => {
        const newEvent: Event = {
            id: (Math.random() * 1000).toString(), // Using random ID for demo purposes
            name: newEventName,
            description: newEventDescription,
            createdAt: new Date(),
            modifiedAt: new Date(),
            status: 'NotStarted',
        };
        onAdd(newEvent);
        setNewEventName('');
        setNewEventDescription('');
    };

    return (
        <div>
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
            <button onClick={handleAddEvent}>Add Event</button>
        </div>
    );
};

export default EventCreator;
