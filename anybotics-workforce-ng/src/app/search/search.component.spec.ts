import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { SearchComponent } from './search.component';
import { SearchService } from '../services/search.service';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
  let searchServiceMock: jasmine.SpyObj<SearchService>;

  beforeEach(async () => {
    searchServiceMock = jasmine.createSpyObj('SearchService', ['updateSearchTerm', 'clearSearchTerm']);

    await TestBed.configureTestingModule({
      imports: [FormsModule, MatButtonModule, MatInputModule, NoopAnimationsModule, SearchComponent],
      providers: [{ provide: SearchService, useValue: searchServiceMock }]
    }).compileComponents();

    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call searchService.updateSearchTerm with the search term on onSearch()', () => {
    component.searchTerm = 'test search';
    component.onSearch();
    expect(searchServiceMock.updateSearchTerm).toHaveBeenCalledWith('test search');
  });

  it('should clear the search term and call searchService.clearSearchTerm on clearSearch()', () => {
    component.searchTerm = 'test search';
    component.clearSearch();
    expect(component.searchTerm).toBe('');
    expect(searchServiceMock.clearSearchTerm).toHaveBeenCalled();
  });

  it('should trigger onSearch() when the search button is clicked', () => {
    spyOn(component, 'onSearch');
    const searchButton: DebugElement = fixture.debugElement.query(By.css('#search-btn'));
    searchButton.triggerEventHandler('click', null);
    expect(component.onSearch).toHaveBeenCalled();
  });

  it('should trigger clearSearch() when the clear button is clicked', () => {
    spyOn(component, 'clearSearch');
    component.searchTerm = 'something'; // Set the searchTerm to make the clear button appear
    fixture.detectChanges(); // Update the view

    const clearButton: DebugElement = fixture.debugElement.query(By.css('#clear-search-btn'));
    clearButton.triggerEventHandler('click', null);
    expect(component.clearSearch).toHaveBeenCalled();
  });

  it('should not display the clear button if searchTerm is empty', () => {
    component.searchTerm = '';
    fixture.detectChanges();

    const clearButton: DebugElement = fixture.debugElement.query(By.css('#clear-search-btn'));
    expect(clearButton).toBeNull();
  });

  it('should display the clear button if searchTerm is not empty', () => {
    component.searchTerm = 'not empty';
    fixture.detectChanges();

    const clearButton: DebugElement = fixture.debugElement.query(By.css('#clear-search-btn'));
    expect(clearButton).not.toBeNull();
  });
});
