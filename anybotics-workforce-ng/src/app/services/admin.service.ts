import { Injectable } from '@angular/core';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private readonly apiUrl = 'https://localhost:7272/Admin/list';

  constructor(private http: HttpService) {}

  async getUsers(): Promise<{ uid: string; email: string }[]> {
    const response = await this.http.fetch(this.apiUrl, {
      method: 'GET',
    });

    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.statusText}`);
    }

    return response.json();
  }
}
