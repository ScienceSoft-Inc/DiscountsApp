import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WebAddressesComponent } from './web-addresses.component';

describe('WebAddressesComponent', () => {
  let component: WebAddressesComponent;
  let fixture: ComponentFixture<WebAddressesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WebAddressesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebAddressesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
