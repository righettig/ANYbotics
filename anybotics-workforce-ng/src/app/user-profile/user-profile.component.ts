import { Component, inject } from '@angular/core';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { AuthService } from '../services/auth.service';
import { MatMenuModule } from '@angular/material/menu';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { Roles } from '../models/roles.enum';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [MatIcon, MatMenuModule, MatButtonModule, MatIconModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss',
})
export class UserProfileComponent {
  private router = inject(Router);

  isLoggedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.isLoggedIn$.subscribe(
      (status) => (this.isLoggedIn = status)
    );

    this.authService.currentUserRole$.subscribe(
      (role) => (this.isAdmin = role === Roles.Admin)
    );
  }

  onLogout() {
    this.authService.logout().then(() => {
      this.router.navigate(['/login']);
    });
  }
}
