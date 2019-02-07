import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolTipPartnerComponent } from './tool-tip-partner.component';

describe('DiscountComponent', () => {
  let component: ToolTipPartnerComponent;
  let fixture: ComponentFixture<ToolTipPartnerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToolTipPartnerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToolTipPartnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
