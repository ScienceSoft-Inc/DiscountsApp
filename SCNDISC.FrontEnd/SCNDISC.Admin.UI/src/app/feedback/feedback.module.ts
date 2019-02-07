import { NgModule } from '@angular/core';
import {FeedbackDialogComponent} from './feedback-dialog/feedback-dialog.component';
import {FeedbacksListComponent} from './feedbacks-list/feedbacks-list.component';
import {SharedModule} from '../shared/shared.module';
import {RouterModule} from '@angular/router';
import {FeedbackService} from './feedback.service';

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: FeedbackDialogComponent}
    ])
  ],
  declarations: [
    FeedbackDialogComponent,
    FeedbacksListComponent
  ],
  entryComponents: [FeedbacksListComponent],
  providers: [FeedbackService]
})
export class FeedbackModule { }
