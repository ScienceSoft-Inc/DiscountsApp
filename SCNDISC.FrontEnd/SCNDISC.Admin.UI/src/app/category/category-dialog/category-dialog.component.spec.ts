import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryOpenDialogComponent } from './category-dialog.component';

describe('CategoryDialogComponent', () => {
  let component: CategoryOpenDialogComponent;
  let fixture: ComponentFixture<CategoryOpenDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryOpenDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryOpenDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
