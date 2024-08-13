import { Component, Output, EventEmitter } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';

@Component({
  selector: 'app-sorting',
  standalone: true,
  imports: [MatSelectModule, MatOptionModule],
  templateUrl: './sorting.component.html',
  styleUrl: './sorting.component.scss'
})
export class SortingComponent {
  @Output() sortChange = new EventEmitter<string>();

  sortOption: string = 'nameAsc';

  onSortOptionChange(value: string): void {
    this.sortOption = value;
    this.sortChange.emit(value);
  }
}
