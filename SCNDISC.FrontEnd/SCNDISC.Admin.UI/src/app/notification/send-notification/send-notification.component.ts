import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Notification} from '../notification';
import {PartnerService} from '../../core/services/partner.service';
import {ToolTipPartner} from '../../shared/models/tool-tip-partner';
import {NamePartner} from '../../shared/models/name-partner';
import {Subscription} from 'rxjs';
import {LanguageService} from '../../core/services/language.service';
import {NotificationService} from '../notification.service';
import {NotificationsListComponent} from '../notifications-list/notifications-list.component';
import {interval, timer } from 'rxjs';
import {RestrictionsByNotificationResponse} from '../restrictions-by-notification';
import { NgForm} from '@angular/forms';

@Component({
  selector: 'app-notification',
  templateUrl: './send-notification.component.html',
  styleUrls: ['./send-notification.component.less']
})
export class SendNotificationComponent implements OnInit, OnDestroy {
  serverResponse: any;
  responseSuccess = false;
  responseError = false;
  notifyMessage = '';
  isCountingDown: boolean;
  isNotifyCounting: boolean;
  public language_en: boolean;
  private languageSubscription: Subscription;
  public toolTipPartners: ToolTipPartner[] = [];
  public namePartners: NamePartner[] = [];
  private namePartnersRu: NamePartner[] = [];
  private namePartnersEn: NamePartner[] = [];
  private restrictions: string [];
  private discountsSubscription: Subscription;
  public notification: Notification = new Notification();
  @ViewChild(NotificationsListComponent, { static: true })
  private notificationsList: NotificationsListComponent;
  // countdown for restrictions
  private countDownSubscription1: Subscription;
  private count1: number;
  countDown1;
  sub1: Subscription;
  // countdown for notifications
  private countDownSubscription2: Subscription;
  private count2: number;
  countDown2;
  sub2: Subscription;

  constructor(private router: Router, private activeModal: NgbActiveModal,
              private partnerService: PartnerService,
              public languageService: LanguageService,
              private notificationService: NotificationService) {
  }

  ngOnInit(): void {
    this.notification.isSendToAllDevices = true;
    this.languageSubscription = this.languageService.getCurrent().subscribe((language: string) => {
      this.language_en = (language === this.languageService.eng);
      if (this.language_en) {
        this.namePartners = this.namePartnersEn;
      } else {
        this.namePartners = this.namePartnersRu;
      }
      this.checkCountDown(language);
    });

    this.discountsSubscription = this.partnerService.getAllTooltips().subscribe((toolTipPrtners: ToolTipPartner[]) => {
      this.toolTipPartners = toolTipPrtners;
      this.namePartnersEn = toolTipPrtners.map( t => new NamePartner( t.id, t.name_En));
      this.namePartnersRu = toolTipPrtners.map( t => new NamePartner( t.id, t.name_Ru));
      if (this.language_en) {
        this.namePartners = this.namePartnersEn;
      } else {
        this.namePartners = this.namePartnersRu;
      }
    });

    this.checkCountDown(this.language_en ? this.languageService.eng : this.languageService.rus);
  }

  onClickedSend(form: NgForm): void {
    if (confirm(this.languageService.getTranslate('Notification.SendQuestion'))) {
      this.notificationService.postData(this.notification, this.language_en ? this.languageService.eng : this.languageService.rus)
        .subscribe(
          (data: any) => {
                    this.notifyMessage = '';
                    this.serverResponse = data;
                    this.responseSuccess = true;
                    this.responseError = false;
                    this.clearForm(form);
                    this.startCountDown2(5);
                    this.notificationsList.onClickedPage(0);
                    this.checkCountDown(this.language_en ? this.languageService.eng : this.languageService.rus);
                },
          (error) => {
                    this.notifyMessage = '';
                    this.responseSuccess = false;
                    this.responseError = true;
                    // restriction errors
                    if (error.error instanceof Array) {
                      error.error.forEach(e => (this.notifyMessage = this.notifyMessage + (this.notifyMessage === '' ? '' : '<br>') + e));
                    }
                    this.notifyMessage = this.notifyMessage === ''  ? error.message : this.notifyMessage;
                    this.startCountDown2(5);
                    console.log(error);
                }
        );
    }
  }

  onClickedCancel(): void {
      this.activeModal.close('Close click');
  }

  ngOnDestroy(): void {
    this.router.navigate(['']);
    this.languageSubscription.unsubscribe();
    this.discountsSubscription.unsubscribe();
    if (this.countDown1) {
      this.countDown1.unsubscribe();
    }
    if (this.sub1) {
      this.sub1.unsubscribe();
    }
    if (this.countDownSubscription1) {
      this.countDownSubscription1.unsubscribe();
    }
    if (this.countDown2) {
      this.countDown2.unsubscribe();
    }
    if (this.sub2) {
      this.sub2.unsubscribe();
    }
    if (this.countDownSubscription2) {
      this.countDownSubscription2.unsubscribe();
    }
  }

  checkCountDown(language: string): void {
    if (this.countDownSubscription1) {
      this.countDownSubscription1.unsubscribe();
    }
    this.countDownSubscription1 = this.notificationService.getCountDown(language)
      .subscribe((restriction: RestrictionsByNotificationResponse) => {
      if (restriction && restriction.secondsToCountdown as number && restriction.secondsToCountdown !== 0) {
        this.restrictions = restriction.restrictions;
        console.log(this.restrictions);
        this.startCountDown1(restriction.secondsToCountdown);
      } else {
        this.isCountingDown = false;
      }
    });
  }

  startCountDown1(seconds: number): void {
    this.isCountingDown = true;
    if (this.countDown1) {
      this.countDown1.unsubscribe();
    }
    this.count1 = seconds;
    this.countDown1 = timer(0, 1000)
      .subscribe(x => {
        this.count1 = this.count1 - 1;
      });

    if (this.sub1) {
      this.sub1.unsubscribe();
    }
    this.sub1 = interval(500)
      .subscribe(x => {
        if (this.count1 === 0) {
          this.isCountingDown = false;
          this.countDown1.unsubscribe();
        }
      });
  }

  startCountDown2(seconds: number): void {
    this.isNotifyCounting = true;
    if (this.countDown2) {
      this.countDown2.unsubscribe();
    }
    this.count2 = seconds;
    this.countDown2 = timer(0, 1000)
      .subscribe(x => {
        this.count2 = this.count2 - 1;
      });

    if (this.sub2) {
      this.sub2.unsubscribe();
    }
    this.sub2 = interval(500)
      .subscribe(x => {
        if (this.count2 === 0) {
          this.isNotifyCounting = false;
          this.countDown2.unsubscribe();
        }
      });
  }

  clearForm(form: NgForm): void {
    this.notification.documentId = null;
    this.notification.isSendToAllDevices = true;
    form.resetForm();
  }
}
