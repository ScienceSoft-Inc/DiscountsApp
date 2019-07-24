import {Component, OnInit} from '@angular/core';
import {MenuService} from '../core/services/menu.service';
import {PartnerService} from '../core/services/partner.service';
import {TranslateService} from '@ngx-translate/core';


@Component({
  selector: 'app-page-of-discounts',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent implements OnInit {

  public path: string;
  constructor(private menuService: MenuService, private partnerService: PartnerService, private translate: TranslateService) {
  }

  ngOnInit(): void {
    this.translate.onLangChange.subscribe(() => {
      document.title = this.translate.instant('Home.Title');
    });
    window.document.body.onclick = () => this.menuService.offShowMenu();
    this.path = this.partnerService.pathNewPartner;
  }
}

