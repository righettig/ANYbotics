import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { AdminService } from '../services/admin.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [MatCardModule, MatListModule, MatProgressSpinnerModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  isLoading: boolean = false;
  users: { uid: string; email: string }[] = [];

  constructor(private adminService: AdminService) {}

  async ngOnInit() {
    try {
      this.isLoading = true;
      this.users! = await this.adminService.getUsers();
      this.isLoading = false;
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  }
}
