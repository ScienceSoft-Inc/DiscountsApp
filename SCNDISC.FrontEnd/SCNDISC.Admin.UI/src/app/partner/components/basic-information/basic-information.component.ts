import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Partner} from '../../../shared/models/partner';
import {LanguageService} from '../../../core/services/language.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-basic-information',
  templateUrl: './basic-information.component.html',
  styleUrls: ['./basic-information.component.css']
})
export class BasicInformationComponent implements OnInit, OnDestroy {

  @Input() partner: Partner;
  @Input() categoryIds: string[];
  public selectedLanguage: string;
  public languageClass: string;
  public language_en: boolean;
  private languageSubscription: Subscription;

  constructor(public translate: LanguageService) {
  }

  ngOnInit(): void {
    this.languageSubscription = this.translate.getCurrent().subscribe((language: string) => {
      this.selectedLanguage = language;
      this.language_en = (language === this.translate.eng);
      this.languageClass = (language === this.translate.eng) ? 'x-lang-bar x-lang-bar-en' : 'x-lang-bar x-lang-bar-ru';
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription.unsubscribe();
  }
}
