import { FC } from 'react';

import Event from '../types/event';

import styles from './event-item.module.css';

interface EventItemProps {
    event: Event;
    onDelete: (id: string) => void;
    onEdit: (event: Event) => void;
    onStart: (event: Event) => void;
}

const EventItem: FC<EventItemProps> = ({ event, onDelete, onEdit, onStart }) => {
    const handleDelete = () => {
        const confirmed = window.confirm('Are you sure you want to delete this event?');
        if (confirmed) {
            onDelete(event.id);
        }
    };

    const handleStart = () => {
        const updatedEvent = { ...event, status: 'Running', modifiedAt: new Date() } as Event;
        onStart(updatedEvent);
    };

    return (
        <li className={styles.eventItem}>
            <div className={styles.eventDetails}>
                <strong>{event.name}</strong>
                <p>{event.description}</p>
            </div>
            <div className={styles.eventMeta}>
                <p>Status: {event.status}</p>
                <p>Created At: {new Date(event.createdAt).toLocaleString()}</p>
            </div>
            <div className={styles.eventActions}>
                {event.status === 'NotStarted' && (
                    <button onClick={handleStart}>Start</button>
                )}
                <button onClick={() => onEdit(event)}>Edit</button>
                <button onClick={handleDelete}>Delete</button>
            </div>
        </li>
    );
};

export default EventItem;
