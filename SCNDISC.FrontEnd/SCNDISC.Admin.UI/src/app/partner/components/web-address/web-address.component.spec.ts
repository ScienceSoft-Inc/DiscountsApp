import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WebAddressComponent } from './web-address.component';

describe('WebAddressComponent', () => {
  let component: WebAddressComponent;
  let fixture: ComponentFixture<WebAddressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WebAddressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
