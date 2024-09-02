import { createApiService } from './api.service';

// TODO: should be passed as configuration
const AUTH_API_URL = 'http://localhost:3001/api/Auth';

const authService = createApiService(AUTH_API_URL);

export const login = (email: string, password: string) => 
    authService.request('login', 'POST', { email, password });

export const logout = () => 
    authService.request('logout', 'POST');

export const refreshToken = () => 
    authService.request('refresh-token', 'POST');
