import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {FeedbackResponse} from './feedback-response';
import {Observable} from 'rxjs/internal/Observable';

@Injectable()
export class FeedbackService {

  private feedbacksSubject = new BehaviorSubject(new FeedbackResponse());
   private feedbacks: FeedbackResponse;
  public length = 10;

  constructor(private http: HttpClient) {
  }


  getAll(start: number): Observable<FeedbackResponse> {
    this.http.get<FeedbackResponse>(environment.URL + '/feedbacks?draw=5&start=' + start + '&length=' + this.length)
      .subscribe(res => {
        this.feedbacks = res;
        this.feedbacksSubject.next(this.feedbacks);
      });
    return this.feedbacksSubject.asObservable();
  }
}
