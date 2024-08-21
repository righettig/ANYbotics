import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-storybook-sample',
  standalone: true,
  imports: [],
  templateUrl: './storybook-sample.component.html',
  styleUrl: './storybook-sample.component.scss'
})
export default class StorybookSampleComponent {
  /**
   * The shape of the task object
  */
  @Input() task: any;

  // tslint:disable-next-line: no-output-on-prefix
  @Output() onPinTask = new EventEmitter<Event>();

  // tslint:disable-next-line: no-output-on-prefix
  @Output() onArchiveTask = new EventEmitter<Event>();
}