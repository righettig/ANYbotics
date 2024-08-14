import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { AuthService } from '../services/auth.service';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [MatIcon, MatMenuModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss',
})
export class UserProfileComponent {
  isLoggedIn = false;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.isLoggedIn.subscribe(status => this.isLoggedIn = status);
  }

  onLogin() {
    this.authService.login();
  }

  onLogout() {
    this.authService.logout();
  }
}
