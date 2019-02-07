import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartnerDetailsComponent } from './partner-details.component';

describe('PartnerComponent', () => {
  let component: PartnerDetailsComponent;
  let fixture: ComponentFixture<PartnerDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartnerDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartnerDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
