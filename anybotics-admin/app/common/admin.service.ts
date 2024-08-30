import { createApiService } from './api.service';

const ADMIN_API_URL = 'https://localhost:7272/Admin';

const adminService = createApiService(ADMIN_API_URL);

export const fetchUsers = () => 
    adminService.request('list', 'GET');

export const deleteUser = (uid: string) => 
    adminService.request(`delete/${uid}`, 'DELETE');
