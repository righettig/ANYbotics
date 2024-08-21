import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorybookSampleComponent } from './storybook-sample.component';

describe('StorybookSampleComponent', () => {
  let component: StorybookSampleComponent;
  let fixture: ComponentFixture<StorybookSampleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StorybookSampleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StorybookSampleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
