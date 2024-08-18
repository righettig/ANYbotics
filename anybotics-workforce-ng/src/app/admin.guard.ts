import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { map, Observable, of, switchMap, take } from 'rxjs';
import { Roles } from './models/roles.enum';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.isLoggedIn$.pipe(
      take(1),
      switchMap((loggedIn) => {
        if (!loggedIn) {
          this.router.navigate(['/login']);
          return of(false);
        }
        return this.authService.currentUserRole$.pipe(
          take(1),
          map((role) => {
            if (role === Roles.Admin) {
              return true;
            } else {
              this.router.navigate(['/not-authorized']);
              return false;
            }
          })
        );
      })
    );
  }
}
