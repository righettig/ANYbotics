import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private readonly apiUrl = 'https://localhost:7272/Admin/list';

  async getUsers(): Promise<{ uid: string; email: string }[]> {
    const response = await fetch(this.apiUrl, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        // Uncomment and use the authorization header if needed
        // 'Authorization': this.authService.accessToken!
      }
    });

    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.statusText}`);
    }

    return response.json(); // Parse the response body as JSON
  }
}
