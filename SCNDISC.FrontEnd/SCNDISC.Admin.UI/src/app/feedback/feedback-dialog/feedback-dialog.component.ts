import { Component, OnInit } from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {FeedbacksListComponent} from '../feedbacks-list/feedbacks-list.component';

@Component({
  selector: 'app-feedback-dialog',
  templateUrl: './feedback-dialog.component.html'
})
export class FeedbackDialogComponent implements OnInit {

  constructor(private modalDialog: NgbModal) { }

  ngOnInit(): void {
    setTimeout( () => this.modalDialog.open(FeedbacksListComponent, {size: 'lg', backdrop: 'static'}));
  }

}
