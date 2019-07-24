import {Component, OnInit, Input, OnDestroy} from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser';
import {Category} from '../../../shared/models/category';
import {LanguageService} from '../../../core/services/language.service';
import {ToolTipPartner} from '../../../shared/models/tool-tip-partner';
import {Subscription} from 'rxjs';
import {ColorHelper} from '../../../Helpers/color-helper';

@Component({
  selector: 'app-discount',
  templateUrl: './tool-tip-partner.component.html',
  styleUrls: ['./tool-tip-partner.component.less']
})
export class ToolTipPartnerComponent implements OnInit, OnDestroy {

  @Input() toolTipPartner: ToolTipPartner;
  public lang_en: boolean;
  private languageSubscription: Subscription;

  constructor(private sanitizer: DomSanitizer, private translate: LanguageService) {
  }

  ngOnInit(): void {
    this.languageSubscription = this.translate.getCurrent().subscribe((language: string) => {
      this.lang_en = (language === this.translate.eng);
    });
  }

  getColorClass(category: Category): object {
    return {'background-color': category.color, 'color': ColorHelper.GetColorByBackHex(category.color)};
  }

  ngOnDestroy(): void {
    if (this.languageSubscription) {
      this.languageSubscription.unsubscribe();
    }
  }
}
