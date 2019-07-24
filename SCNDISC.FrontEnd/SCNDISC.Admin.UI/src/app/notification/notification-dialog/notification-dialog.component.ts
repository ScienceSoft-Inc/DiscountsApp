import { Component, OnInit } from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {SendNotificationComponent} from '../send-notification/send-notification.component';

@Component({
  selector: 'app-notification-dialog',
  templateUrl: './notification-dialog.component.html'
})
export class NotificationDialogComponent implements OnInit {

  constructor(private modalDialog: NgbModal) { }

  ngOnInit(): void {
    setTimeout( () => this.modalDialog.open(SendNotificationComponent, {size: 'lg', backdrop: 'static'}));
  }

}
