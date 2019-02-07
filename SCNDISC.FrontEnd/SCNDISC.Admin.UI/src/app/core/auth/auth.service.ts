import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {BehaviorSubject} from 'rxjs/internal/BehaviorSubject';
import {Observable} from 'rxjs/internal/Observable';
import {Token} from '../../shared/models/token';

@Injectable()
export class AuthService {

  private token: Token;
  private isLoggedInBehavior = new BehaviorSubject(true);

  constructor(private http: HttpClient) {
  }

  public testLogin(login: string, password: string): Observable<boolean> {
    if (login == null) {
      login = '';
    }
    if (password == null) {
      password = '';
    }
    this.http.post<Token>(environment.URL + '/token', {login: login, password: password}).subscribe((res) => {
      if (res.access_token) {
        this.token = res;
        sessionStorage.setItem(environment.jwt, res.access_token);
        this.isLoggedInBehavior.next(true);
      } else {
        this.isLoggedInBehavior.next(false);
      }
    }, () => {
      this.isLoggedInBehavior.next(false);
    });
    return this.isLoggedInBehavior.asObservable();
  }
}
