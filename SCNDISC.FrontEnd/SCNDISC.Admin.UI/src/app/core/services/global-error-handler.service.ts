import {ErrorHandler, Injectable, Injector, NgZone} from '@angular/core';
import {Router} from '@angular/router';


@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private injector: Injector) {
  }

  handleError(error: any): void {
    console.log(error);
    const router = this.injector.get(Router);
    const ngZone = this.injector.get(NgZone);
    ngZone.run(() => {
      if (error.status == 401) {
        router.navigate(['login'], {skipLocationChange: true});
      } else {
        router.navigate(['error'], {skipLocationChange: true});
      }
    });
  }
}
