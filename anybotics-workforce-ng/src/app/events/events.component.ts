import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { BehaviourService } from '../services/behaviour.service';
import { BehaviourSnippet } from '../models/behaviour-dto.model';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';

// TODO: need something like: https://microsoft.github.io/monaco-editor/playground.html?source=v0.51.0#example-extending-language-services-configure-javascript-defaults
// To allow ICommand, IWorldState to be recognised as valid types and also in order to enable auto-completion

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [FormsModule, MonacoEditorModule, MatSelectModule, MatOptionModule, MatButtonModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.scss'
})
export class EventsComponent {
  editorOptions = { theme: 'vs-dark', language: 'typescript' };
  code: string = '';
  selectedLanguage: string = '';
  behaviours: BehaviourSnippet[] = [];

  constructor(
    private behaviourService: BehaviourService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('id');

    this.behaviourService.getBehaviours().subscribe((data) => {
      // Clean up the code by removing leading and trailing whitespace
      this.behaviours = data.map(behaviour => ({
        ...behaviour,
        code: behaviour.code.trim()
      }));

      if (this.behaviours.length > 0) {
        this.selectedLanguage = this.behaviours[0].language;
        this.updateEditorContent(this.behaviours[0].code, this.selectedLanguage);
      }
    });
  }

  onLanguageChange(language: string): void {
    const behaviour = this.behaviours.find(b => b.language === language);
    if (behaviour) {
      this.updateEditorContent(behaviour.code, behaviour.language);
    }
  }

  private updateEditorContent(code: string, language: string): void {
    this.code = code;
    this.editorOptions = { ...this.editorOptions, language };
  }
}
