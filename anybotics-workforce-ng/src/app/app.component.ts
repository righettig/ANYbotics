import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AgentsComponent } from './agents/agents.component';
import { LayoutComponent } from "./layout/layout.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AgentsComponent, LayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'anybotics-workforce-ng';
}
