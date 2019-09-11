import {Component, Input, OnInit} from '@angular/core';
import {Rating} from '../../../shared/models/partner-rating';

@Component({
  selector: 'app-partner-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.less']
})
export class PartnerRatingComponent implements OnInit {

  @Input() rating: Rating;
  ratingValue: number;
  countOfVotes: number;

  constructor() {
  }

  ngOnInit(): void {
    this.ratingValue = (this.rating && this.rating.ratingCount && this.rating.ratingCount !== 0) ?
      this.rating.ratingSum / this.rating.ratingCount : 0;
    this.countOfVotes = this.rating && this.rating.ratingCount ?
      this.rating.ratingCount : 0;
  }
}
