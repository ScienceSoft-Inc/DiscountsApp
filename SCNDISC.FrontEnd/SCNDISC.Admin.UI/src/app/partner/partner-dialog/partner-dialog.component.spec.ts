import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartnerOpenComponent } from './partner-open.component';

describe('PartnerOpenComponent', () => {
  let component: PartnerOpenComponent;
  let fixture: ComponentFixture<PartnerOpenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartnerOpenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartnerOpenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
