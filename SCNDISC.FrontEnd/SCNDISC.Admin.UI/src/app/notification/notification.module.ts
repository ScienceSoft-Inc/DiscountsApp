import { NgModule } from '@angular/core';
import {NotificationDialogComponent} from './notification-dialog/notification-dialog.component';
import {SendNotificationComponent} from './send-notification/send-notification.component';
import {NotificationsListComponent} from './notifications-list/notifications-list.component';
import {SharedModule} from '../shared/shared.module';
import {RouterModule} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {NotificationService} from './notification.service';
import {SecondsToTimePipe} from './send-notification/seconds.pipe';

@NgModule({
  imports: [
    SharedModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forChild([
      {path: '', component: NotificationDialogComponent}
    ])
  ],
  declarations: [
    NotificationDialogComponent,
    SendNotificationComponent,
    NotificationsListComponent,
    SecondsToTimePipe
  ],
  entryComponents: [SendNotificationComponent],
  providers: [NotificationService]
})
export class NotificationModule {
}
