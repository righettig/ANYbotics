import { FC, useState } from 'react';

import styles from './sidebar.module.css';

interface SidebarProps {
    onLogout: () => void;
    onSelect: (view: 'events' | 'users') => void;
}

const Sidebar: FC<SidebarProps> = ({ onLogout, onSelect }) => {
    const [isCollapsed, setIsCollapsed] = useState(false);

    const handleToggle = () => {
        setIsCollapsed(!isCollapsed);
    };

    return (
        <div className={`${styles.sidebar} ${isCollapsed ? styles.collapsed : ''}`}>
            <button onClick={handleToggle} className={styles.toggleButton}>
                {isCollapsed ? '>' : '<'}
            </button>
            <div className={styles.menu}>
                <button onClick={() => onSelect('events')}>Events</button>
                <button onClick={() => onSelect('users')}>Users</button>
                <button onClick={onLogout}>Logout</button>
            </div>
        </div>
    );
};

export default Sidebar;
