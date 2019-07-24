import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../core/auth/auth.service';
import {Router} from '@angular/router';
import {LanguageService} from '../core/services/language.service';
import {Subscription} from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit, OnDestroy {

  private loginSubscription: Subscription;
  public login: string;
  public password: string;
  public errorLoginAndPassword: boolean;

  constructor(private router: Router, private authService: AuthService, public translate: LanguageService ) {
  }

  ngOnInit(): void {
  }

  public onClickedEnter(): void {
    this.loginSubscription = this.authService.testLogin(this.login, this.password).subscribe((res) => {
      if (res) {
        this.errorLoginAndPassword = false;
        this.router.navigate(['']);
      } else {
        this.errorLoginAndPassword = true;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }
  }
}
