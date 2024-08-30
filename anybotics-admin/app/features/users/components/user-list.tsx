import { FC } from 'react';

import User from '../types/user';

import styles from './user-list.module.css';

interface UserListProps {
    users: User[];
}

const UserList: FC<UserListProps> = ({ users }) => {
    return (
        <div className={styles.userList}>
            <h2>User List</h2>
            <ul>
                {users.map(user => (
                    <li key={user.uid}>
                        <strong>{user.email}</strong>
                        <p>Email: {user.email}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default UserList;
