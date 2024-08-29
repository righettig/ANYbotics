import { FC } from 'react';

import styles from './sidebar.module.css';

interface SidebarProps {
    onLogout: () => void;
    onSelect: (view: 'events' | 'users') => void;
}

const Sidebar: FC<SidebarProps> = ({ onLogout, onSelect }) => {
    return (
        <div className={styles.sidebar}>
            <button onClick={() => onSelect('events')}>Events</button>
            <button onClick={() => onSelect('users')}>Users</button>
            <button onClick={onLogout}>Logout</button>
        </div>
    );
};

export default Sidebar;
