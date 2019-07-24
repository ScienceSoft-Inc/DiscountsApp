import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {NotificationsResponse} from './notifications-response';
import {Observable} from 'rxjs/internal/Observable';
import {Notification} from './notification';
import {RestrictionsByNotificationResponse} from './restrictions-by-notification';

@Injectable()
export class NotificationService {

  private notificationsSubject = new BehaviorSubject(new NotificationsResponse());
  private notifications: NotificationsResponse;
  public length = 10;

  constructor(private http: HttpClient) {
  }

  postData(notification: Notification, lang: string) {
    let httpParams: HttpParams = new HttpParams();
    httpParams = httpParams.append('lang', lang);
    return this.http.post(environment.URL + '/notification', notification, {headers: this.getHeaders(), params: httpParams});
  }

  getAll(start: number, language: string): Observable<NotificationsResponse> {
    console.log('getAllMethodCall: ' + language);
    let httpParams: HttpParams = new HttpParams();
    httpParams = httpParams.append('start', start.toString());
    httpParams = httpParams.append('length', this.length.toString());
    httpParams = httpParams.append('lang', language);
    this.http.get<NotificationsResponse>(environment.URL + '/notifications', {params: httpParams})
      .subscribe(res => {
        this.notifications = res;
        this.notificationsSubject.next(this.notifications);
      });
    return this.notificationsSubject.asObservable();
  }

  getCountDown(lang: string): Observable<RestrictionsByNotificationResponse> {
    let httpParams: HttpParams = new HttpParams();
    httpParams = httpParams.append('lang', lang);
    return this.http.get<RestrictionsByNotificationResponse>(environment.URL + '/restrictionsByNotification', {params: httpParams});
  }

  private getHeaders(): HttpHeaders {
    const token = sessionStorage.getItem(environment.jwt);
    const httpHeaders = new HttpHeaders({
      'Authorization': 'Bearer ' + token,
      'Content-Type': 'application/json'
    });

    return httpHeaders;
  }
}
