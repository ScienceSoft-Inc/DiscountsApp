import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WarningComponent } from './warning.component';

describe('ErrorServerModalComponent', () => {
  let component: WarningComponent;
  let fixture: ComponentFixture<WarningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WarningComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
