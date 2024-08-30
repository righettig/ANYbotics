import { FC, useEffect, useState } from 'react';
import User from '../types/user';
import UserList from './user-list';
import { fetchUsers } from '@/app/common/admin.service';

const Users: FC = () => {
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const getUsers = async () => {
            try {
                const userList = await fetchUsers();
                setUsers(userList);
            } catch (err) {
                setError('Failed to fetch users.');
            } finally {
                setLoading(false);
            }
        };

        getUsers();
    }, []);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return <UserList users={users} />;
};

export default Users;
