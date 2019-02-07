import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs/internal/BehaviorSubject';
import {Observable} from 'rxjs/internal/Observable';

@Injectable()
export class MenuService {

  private menuDisplaySubject = new BehaviorSubject(false);

  constructor() {
  }

  offShowMenu(): void {
    this.menuDisplaySubject.next(false);
  }

  showMenu(menuDisplay): void {
    this.menuDisplaySubject.next(menuDisplay);
  }

  getMenuDisplay(): Observable<boolean> {
    return this.menuDisplaySubject.asObservable();
  }
}
