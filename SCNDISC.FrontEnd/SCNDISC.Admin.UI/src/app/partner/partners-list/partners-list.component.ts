import {Component, OnDestroy, OnInit} from '@angular/core';
import {PartnerService} from '../../core/services/partner.service';
import {ToolTipPartner} from '../../shared/models/tool-tip-partner';
import {Subscription} from 'rxjs';


@Component({
  selector: 'app-discounts',
  templateUrl: './partners-list.component.html'
})
export class PartnersListComponent implements OnInit, OnDestroy {


  public toolTipPartners: ToolTipPartner[] = [];
  private discountsSubscription: Subscription;

  constructor(private partnerService: PartnerService) {
  }

  ngOnInit(): void {
    this.discountsSubscription = this.partnerService.loadAll().subscribe((toolTipPartners: ToolTipPartner[]) => {
      this.toolTipPartners = toolTipPartners;
    });
  }

  ngOnDestroy(): void {
    this.discountsSubscription.unsubscribe();
  }
}
