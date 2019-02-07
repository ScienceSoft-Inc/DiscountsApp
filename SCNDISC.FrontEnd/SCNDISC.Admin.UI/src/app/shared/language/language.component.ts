import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from 'rxjs/index';
import {LanguageService} from '../../core/services/language.service';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit, OnDestroy {

  public selectedLanguage: string;
  public langClass: string;
  private languageSubscription: Subscription;

  constructor(public languageService: LanguageService) {
  }

  ngOnInit(): void {
    this.languageSubscription = this.languageService.getCurrent().subscribe((language: string) => {
      this.selectedLanguage = language;
      this.langClass = (language === this.languageService.rus) ? 'x-lang-bar x-lang-bar-ru' : 'x-lang-bar x-lang-bar-en';
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription.unsubscribe();
  }

}
