import { FC } from 'react';

import Event from '../types/event';

interface EventListProps {
    events: Event[];
    onDelete: (id: string) => void;
    onEdit: (event: Event) => void;
}

const EventList: FC<EventListProps> = ({ events, onDelete, onEdit }) => {
    const handleDelete = (id: string) => {
        const confirmed = window.confirm('Are you sure you want to delete this event?');
        if (confirmed) {
            onDelete(id);
        }
    };

    return (
        <div>
            <h2>Event List</h2>
            <ul>
                {events.map(event => (
                    <li key={event.id} className="event-item">
                        <div className="event-details">
                            <strong>{event.name}</strong>
                            <p>{event.description}</p>
                        </div>
                        <div className="event-meta">
                            <p>Status: {event.status}</p>
                            <p>Created At: {new Date(event.createdAt).toLocaleString()}</p>
                        </div>
                        <div className="event-actions">
                            <button onClick={() => onEdit(event)}>Edit</button>
                            <button onClick={() => handleDelete(event.id)}>Delete</button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default EventList;
