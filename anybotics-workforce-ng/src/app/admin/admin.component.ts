import { Component } from '@angular/core';
import { UserListComponent } from '../user-list/user-list.component';
import { UserCreateComponent } from '../user-create/user-create.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [UserListComponent, UserCreateComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {

}
