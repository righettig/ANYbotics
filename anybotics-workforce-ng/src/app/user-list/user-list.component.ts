import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [MatCardModule, MatListModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: { uid: string, email: string }[] = [];

  constructor(private adminService: AdminService) {}

  async ngOnInit() {
    try {
      this.users! = await this.adminService.getUsers();
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  }
}
