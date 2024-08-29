import { FC } from 'react';

import Event from '../types/event';

interface EventListProps {
    events: Event[];
    onDelete: (id: string) => void;
}

const EventList: FC<EventListProps> = ({ events, onDelete }) => {
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
                        <button onClick={() => onDelete(event.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default EventList;
