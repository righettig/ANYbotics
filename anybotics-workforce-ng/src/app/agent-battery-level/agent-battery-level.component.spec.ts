import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatIconModule } from '@angular/material/icon';
import { AgentBatteryLevelComponent } from './agent-battery-level.component';
import { By } from '@angular/platform-browser';

describe('AgentBatteryLevelComponent', () => {
  let component: AgentBatteryLevelComponent;
  let fixture: ComponentFixture<AgentBatteryLevelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatIconModule, AgentBatteryLevelComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AgentBatteryLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should display battery_alert icon when batteryLevel is less than 5', () => {
    component.batteryLevel = 4;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_alert');
  });

  it('should display battery_2_bar icon when batteryLevel is between 5 and 10', () => {
    component.batteryLevel = 7;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_2_bar');
  });

  it('should display battery_3_bar icon when batteryLevel is between 11 and 25', () => {
    component.batteryLevel = 20;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_3_bar');
  });

  it('should display battery_4_bar icon when batteryLevel is between 26 and 50', () => {
    component.batteryLevel = 30;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_4_bar');
  });

  it('should not display any icon when batteryLevel is above 50', () => {
    component.batteryLevel = 60;
    fixture.detectChanges();
    const icons = fixture.debugElement.queryAll(By.css('mat-icon'));
    expect(icons.length).toBe(0);
  });

  it('should handle edge case where batteryLevel is exactly 5', () => {
    component.batteryLevel = 5;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_2_bar');
  });

  it('should handle edge case where batteryLevel is exactly 10', () => {
    component.batteryLevel = 10;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_2_bar');
  });

  it('should handle edge case where batteryLevel is exactly 25', () => {
    component.batteryLevel = 25;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_3_bar');
  });

  it('should handle edge case where batteryLevel is exactly 50', () => {
    component.batteryLevel = 50;
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('mat-icon')).nativeElement;
    expect(icon.textContent).toContain('battery_4_bar');
  });
});
