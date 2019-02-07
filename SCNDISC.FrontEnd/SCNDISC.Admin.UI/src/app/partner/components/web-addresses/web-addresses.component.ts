import {Component, Input, OnInit} from '@angular/core';

import {WebAddress} from '../../../shared/models/webAddress';
import {Partner} from '../../../shared/models/partner';

@Component({
  selector: 'app-web-addresses',
  templateUrl: './web-addresses.component.html',
  styleUrls: ['./web-addresses.component.css']
})
export class WebAddressesComponent implements OnInit {

  @Input() partner: Partner;

  constructor() {
  }

  ngOnInit() {
  }

  onClickedAddWebAddress(): void {
    const webAddress = new WebAddress();
    this.partner.webAddresses.push(webAddress);
  }

  onClickedRemove(webAddress): void {
    const position = this.partner.webAddresses.indexOf(webAddress);
    this.partner.webAddresses.splice(position, 1);
  }

}
