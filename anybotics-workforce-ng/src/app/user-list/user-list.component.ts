import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [MatCardModule, MatListModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'] // Fixed typo here; should be styleUrls
})
export class UserListComponent implements OnInit {
  users: { uid: string, email: string }[] = [];

  async ngOnInit() {
    try {
      this.users = await this.getUsers(); // Update component state with fetched users
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  }

  async getUsers(): Promise<{ uid: string, email: string }[]> {
    const response = await fetch("https://localhost:7272/Admin/list", {
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
