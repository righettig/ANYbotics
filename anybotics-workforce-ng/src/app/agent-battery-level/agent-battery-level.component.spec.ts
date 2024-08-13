import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentBatteryLevelComponent } from './agent-battery-level.component';

describe('AgentBatteryLevelComponent', () => {
  let component: AgentBatteryLevelComponent;
  let fixture: ComponentFixture<AgentBatteryLevelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgentBatteryLevelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AgentBatteryLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
