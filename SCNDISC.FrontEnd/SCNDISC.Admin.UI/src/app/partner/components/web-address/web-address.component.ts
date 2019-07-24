import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {WebAddress} from '../../../shared/models/webAddress';

@Component({
  selector: 'app-web-address',
  templateUrl: './web-address.component.html',
  styleUrls: ['./web-address.component.less']
})
export class WebAddressComponent implements OnInit {

  @Output() removeAddress = new EventEmitter<WebAddress>();
  @Input() webAddress: WebAddress;
  @Input() id: number;

  constructor() {
  }

  ngOnInit(): void {
  }

  onClickedRemove(): void {
    this.removeAddress.emit(this.webAddress);
  }
}
