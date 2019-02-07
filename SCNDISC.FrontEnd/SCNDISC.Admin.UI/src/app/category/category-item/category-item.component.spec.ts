import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryForManagerComponent } from './category.component';

describe('CategoryComponent', () => {
  let component: CategoryForManagerComponent;
  let fixture: ComponentFixture<CategoryForManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryForManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryForManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
