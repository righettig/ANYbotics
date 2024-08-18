import { Injectable } from '@angular/core';
import { AuthService } from './services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor(private authService: AuthService) {}

  async fetch(input: RequestInfo, init?: RequestInit): Promise<Response> {
    // Get the token from your AuthService
    const token = this.authService.accessToken;

    // Create or clone the headers object
    const headers = new Headers(init?.headers || {});
    headers.set('Authorization', `${token}`);
    headers.set('Content-Type', 'application/json');

    // Create the modified fetch options
    const modifiedInit: RequestInit = {
      ...init,
      headers,
    };

    // Perform the fetch with the modified options
    return fetch(input, modifiedInit);
  }
}
