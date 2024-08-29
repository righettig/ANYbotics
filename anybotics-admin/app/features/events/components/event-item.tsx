// components/EventItem.tsx
import { FC } from 'react';
import Event from '../types/event';

interface EventItemProps {
    event: Event;
    onDelete: (id: string) => void;
    onEdit: (event: Event) => void;
}

const EventItem: FC<EventItemProps> = ({ event, onDelete, onEdit }) => {
    const handleDelete = () => {
        const confirmed = window.confirm('Are you sure you want to delete this event?');
        if (confirmed) {
            onDelete(event.id);
        }
    };

    return (
        <li className="event-item">
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
                <button onClick={handleDelete}>Delete</button>
            </div>
        </li>
    );
};

export default EventItem;
