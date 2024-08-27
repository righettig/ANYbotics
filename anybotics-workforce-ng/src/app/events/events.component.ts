import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [FormsModule, MonacoEditorModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.scss'
})
export class EventsComponent {
  editorOptions = {theme: 'vs-dark', language: 'javascript'};
  
  // TODO: hardcode "code" based on current user
  code: string= `
    update(world: WorldData) {
      return "moveForward";
	  }
  `;

  constructor(
    private route: ActivatedRoute,
  ) {}
  
  ngOnInit(): void {
    const agentId = this.route.snapshot.paramMap.get('id');
  }
}
