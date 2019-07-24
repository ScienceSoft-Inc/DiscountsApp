import {Component, OnDestroy, Input, OnInit} from '@angular/core';
import {NotificationService} from '../notification.service';
import {Notification} from '../notification';
import {Subscription} from 'rxjs/internal/Subscription';
import {NamePartner} from '../../shared/models/name-partner';
import {LanguageService} from '../../core/services/language.service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications-list.component.html',
  styleUrls: ['./notifications-list.component.less']
})
export class NotificationsListComponent implements OnInit, OnDestroy {

  private notificationSubscription: Subscription;
  public notifications: Notification[];
  public notificationsLength = 0;
  public numberOfPage = 0;
  public pages: number[];
  public currentPage = 0;
  public minNumber: number;
  public maxNumber: number;
  @Input() namePartners: NamePartner[];
  private language_en: boolean;
  private languageSubscription: Subscription;
  private isInit = true;

  constructor(private notificationService: NotificationService, public languageService: LanguageService) {
  }
  ngOnInit(): void {
    this.languageSubscription = this.languageService.getCurrent().subscribe((language: string) => {
      this.language_en = (language === this.languageService.eng);
      if (!this.isInit) {
        this.onClickedPage(0);
      }
    });

    this.notificationSubscription = this.notificationService.getAll(0,
      this.language_en ? this.languageService.eng : this.languageService.rus).subscribe(notifications => {
      this.notifications = notifications.data;
      this.notificationsLength = notifications.recordsFiltered;
      this.numberOfPage = this.notificationsLength / this.notificationService.length;
      this.pages = [];
      this.minNumber = this.currentPage * this.notificationService.length + 1;
      this.maxNumber = this.currentPage * this.notificationService.length + this.notificationService.length;
      let i = 0;
      while (i < this.numberOfPage) {
        this.pages.push(i);
        i++;
      }
    });
    this.isInit = false;
  }

  getNameById(documentId: string): string {
    if (documentId) {
      const foundPartner = this.namePartners.find(t => t.id === documentId);
      if (foundPartner) {
        return foundPartner.name;
      }
    }
    return '';
  }

  onClickedPage(number?: number, message?: string): void {
    if (number >= 0) {
      this.currentPage = number;
      this.getNotifications();
    } else if (this.currentPage > 0 && message === 'previous') {
      this.currentPage--;
      this.getNotifications();
    } else if (this.currentPage < Math.floor(this.notificationsLength / this.notificationService.length) && message === 'next') {
      const isThisCase = ((this.notificationsLength % this.notificationService.length === 0
        && this.currentPage + 1 === Math.floor(this.notificationsLength / this.notificationService.length)));
      if (!isThisCase) {
        this.currentPage++;
        this.getNotifications();
      }
    }
  }

  getNotifications(): void {
    this.notificationService.getAll(this.currentPage * this.notificationService.length,
      this.language_en ? this.languageService.eng : this.languageService.rus);
  }

  getStyleForNumberOfPage(i: number): object {
    if (this.currentPage === i) {
      return {'color': 'white', 'background-color': '#0082dd'};
    }
  }

  getClassForNext(): string {
    const isSpecialCase = (this.notificationsLength % this.notificationService.length) === 0;
    if (isSpecialCase) {
      if (this.currentPage === Math.floor(this.notificationsLength / this.notificationService.length) - 1) {
        return 'x-item-pre';
      } else {
        return 'x-item';
      }
    } else {
      if (this.currentPage === Math.floor(this.notificationsLength / this.notificationService.length)) {
        return 'x-item-pre';
      } else {
        return 'x-item';
      }
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
    this.notificationSubscription.unsubscribe();
    this.languageSubscription.unsubscribe();
  }

}
