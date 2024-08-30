const API_URL = 'https://localhost:7272/Admin';

const request = async (endpoint: string, method: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE', body?: any) => {
    try {
        const response = await fetch(`${API_URL}/${endpoint}`, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token') || ''}`,
            },
            body: body ? JSON.stringify(body) : null,
        });
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        // Try to parse JSON response if it exists
        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
            return await response.json();
        }

        // Return empty object or handle non-JSON response as needed
        return {};
    } catch (error) {
        console.error('API request failed:', error);
        throw error;
    }
};

export const fetchUsers = () => request('list', 'GET');
export const deleteUser = (uid: string) => request(`delete/${uid}`, 'DELETE');