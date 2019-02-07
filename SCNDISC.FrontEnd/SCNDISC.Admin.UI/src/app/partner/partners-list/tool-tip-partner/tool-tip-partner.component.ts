import {Component, OnInit, Input, OnDestroy} from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser';
import {Category} from '../../../shared/models/category';
import {LanguageService} from '../../../core/services/language.service';
import {ToolTipPartner} from '../../../shared/models/tool-tip-partner';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-discount',
  templateUrl: './tool-tip-partner.component.html',
  styleUrls: ['./tool-tip-partner.component.css']
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

  getLogoStyle(): object {
    if(this.toolTipPartner.logo){
      const imgUrl = 'data: image/png; base64,' + this.toolTipPartner.logo;
      return {'background-image': `url('${imgUrl}')`};
    } else {
      return {'background-image': `url("assets/images/camera-128x128.png")`};
    }

  }

  getColorClass(category: Category): object {
    return {'background-color': category.color};
  }

  ngOnDestroy(): void {
    if (this.languageSubscription) {
      this.languageSubscription.unsubscribe();
    }
  }
}
