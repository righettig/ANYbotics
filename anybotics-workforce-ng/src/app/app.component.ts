import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AgentsComponent } from './agents/agents.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AgentsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'anybotics-workforce-ng';
}
