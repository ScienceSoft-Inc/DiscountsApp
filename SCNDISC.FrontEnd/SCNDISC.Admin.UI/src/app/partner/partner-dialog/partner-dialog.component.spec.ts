import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartnerDialogComponent } from './partner-dialog.component';

describe('PartnerOpenComponent', () => {
  let component: PartnerDialogComponent;
  let fixture: ComponentFixture<PartnerDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartnerDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartnerDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
