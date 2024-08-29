import { FC } from 'react';

import Event from '../types/event';
import EventItem from './event-item';

interface EventListProps {
    events: Event[];
    onDelete: (id: string) => void;
    onEdit: (event: Event) => void;
    onStart: (event: Event) => void;
}

const EventList: FC<EventListProps> = ({ events, onDelete, onEdit, onStart }) => {
    return (
        <div>
            <h2>Event List</h2>
            <ul>
                {events.map(event => (
                    <EventItem
                        key={event.id}
                        event={event}
                        onDelete={onDelete}
                        onEdit={onEdit}
                        onStart={onStart}
                    />
                ))}
            </ul>
        </div>
    );
};

export default EventList;
