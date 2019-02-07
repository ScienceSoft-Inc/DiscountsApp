import {Injectable} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {BehaviorSubject} from 'rxjs';
import {Observable} from 'rxjs/internal/Observable';

@Injectable()
export class LanguageService {

  private languageSubject = new BehaviorSubject('Eng');
  public readonly eng = 'Eng';
  public readonly rus = 'Рус';

  constructor(public translate: TranslateService) {
    translate.addLangs([this.rus, this.eng]);
    translate.setDefaultLang(this.eng);

    const browserLang = translate.getBrowserLang();
    translate.use(browserLang.match(/Рус|Eng/) ? browserLang : this.eng);
    this.languageSubject = new BehaviorSubject(browserLang.match(/Рус|Eng/) ? browserLang : this.eng);
  }

  use(language: string): void {
    this.translate.use(language);
    this.languageSubject.next(language);
  }

  getAll(): string[] {
    return this.translate.getLangs();
  }

  getCurrent(): Observable<string> {
    return this.languageSubject.asObservable();
  }

  getTranslate(key: string): string {
    return this.translate.instant(key);
  }
}
