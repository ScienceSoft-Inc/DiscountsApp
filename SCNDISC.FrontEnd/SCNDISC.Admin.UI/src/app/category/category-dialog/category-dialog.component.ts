import {Component, OnInit} from '@angular/core';
import {CategoriesListComponent} from '../categories-list/categories-list.component';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-category-dialog',
  templateUrl: './category-dialog.component.html'
})
export class CategoryDialogComponent implements OnInit {

  constructor(private  modalDialogService: NgbModal) {
  }

  ngOnInit(): void {
    setTimeout(() => this.modalDialogService.open(CategoriesListComponent, {backdrop: 'static'}));
  }

}
