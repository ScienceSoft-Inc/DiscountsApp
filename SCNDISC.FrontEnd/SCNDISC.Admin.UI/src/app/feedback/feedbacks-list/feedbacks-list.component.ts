import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FeedbackService} from '../feedback.service';
import {Feedback} from '../feedback';
import {Subscription} from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedbacks-list.component.html',
  styleUrls: ['./feedbacks-list.component.css']
})
export class FeedbacksListComponent implements OnInit, OnDestroy {

  private feedbackSubscription: Subscription;
  public feedbacks: Feedback[];
  public feedbacksLenght = 0;
  public numberOfPage = 0;
  public pages: number[];
  public currentPage = 0;
  public minNumber: number;
  public maxNumber: number;

  constructor(private router: Router, private activeModal: NgbActiveModal, private feedbackService: FeedbackService) {
  }


  ngOnInit(): void {
    this.feedbackSubscription = this.feedbackService.getAll(0).subscribe(feedbacks => {
      this.feedbacks = feedbacks.data;
      this.feedbacksLenght = feedbacks.recordsFiltered;
      this.numberOfPage = this.feedbacksLenght / this.feedbackService.length;
      this.pages = [];
      this.minNumber = this.currentPage * this.feedbackService.length + 1;
      this.maxNumber = this.currentPage * this.feedbackService.length + this.feedbackService.length;
      let i = 0;
      while (i < this.numberOfPage) {
        this.pages.push(i);
        i++;
      }
    });
  }

  onClickedCancel(): void {
    this.activeModal.close('Close click');
  }

  onClickedPage(number?: number, message?: string): void {
    if (number >= 0) {
      this.currentPage = number;
      this.getFeedbacks();
    } else if (this.currentPage > 0 && message === 'previous') {
      this.currentPage--;
      this.getFeedbacks();
    } else if (this.currentPage < Math.floor(this.feedbacksLenght / this.feedbackService.length) && message === 'next') {
      this.currentPage++;
      this.getFeedbacks();
    }

  }

  getFeedbacks(): void {
    this.feedbackService.getAll(this.currentPage * this.feedbackService.length);
  }

  getStyleForNumberOfPage(i: number): object {
    if (this.currentPage === i) {
      return {'color': 'white', 'background-color': '#ABABAB'};
    }
  }

  getClassForNext(): string {
    if (this.currentPage === Math.floor(this.feedbacksLenght / this.feedbackService.length)) {
      return 'x-item-pre';
    } else {
      return 'x-item';
    }
  }

  getClassForPrevious(): string {
    if (this.currentPage === 0) {
      return 'x-item-pre';
    } else {
      return 'x-item';
    }
  }

  ngOnDestroy(): void {
    this.feedbackSubscription.unsubscribe();
    this.router.navigate(['']);
  }

}
