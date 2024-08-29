import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AdminService } from '../services/admin.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [MatCardModule, MatListModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  isLoading: boolean = false;
  users: { uid: string; email: string }[] = [];

  constructor(private adminService: AdminService) { }

  async ngOnInit() {
    await this.loadUsers();
  }

  async loadUsers() {
    try {
      this.isLoading = true;
      this.users = await this.adminService.getUsers();
    } catch (error) {
      console.error('Error fetching users:', error);
    } finally {
      this.isLoading = false;
    }
  }

  async deleteUser(uid: string) {
    if (!confirm(`Are you sure you want to delete user with ID ${uid}?`)) {
      return;
    }

    try {
      await this.adminService.deleteUser(uid);
      this.users = this.users.filter(user => user.uid !== uid);
    } catch (error) {
      console.error('Error deleting user:', error);
    }
  }
}
