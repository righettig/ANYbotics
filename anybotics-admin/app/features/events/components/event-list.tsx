import { FC } from 'react';

import Event from '../types/event';
import EventItem from './event-item';

interface EventListProps {
    events: Event[];
    onDelete: (id: string) => void;
    onEdit: (event: Event) => void;
}

const EventList: FC<EventListProps> = ({ events, onDelete, onEdit }) => {
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
                    />
                ))}
            </ul>
        </div>
    );
};

export default EventList;
