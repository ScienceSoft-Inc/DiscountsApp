import {AfterViewInit, Component, Input, OnInit} from '@angular/core';
import {environment} from '../../../../../environments/environment';
import {PartnerService} from '../../../../core/services/partner.service';

@Component({
  selector: 'app-avatar-loading',
  templateUrl: './avatar-loading.component.html',
  styleUrls: ['./avatar-loading.component.less']
})
export class AvatarLoadingComponent implements AfterViewInit {

  @Input() id: string;
  backgroundStyle: object = {};

  constructor(private partnerService: PartnerService) {
  }

  ngAfterViewInit(): void {
    this.partnerService.getLogo(this.id)
      .subscribe(
        (data: Blob) => {
          if (data && data.size > 0) {
            this.backgroundStyle = {'background-image': `url('${this.getUrl()}')`};
          } else {
            this.backgroundStyle = {'background-image': `url("assets/images/camera-128x128.png")`};
          }
        },
        (error) => {
          this.backgroundStyle = {'background-image': `url("assets/images/camera-128x128.png")`};
          console.log(error);
        }
      );
  }

  getUrl(): string {
    return environment.URL + '/discounts/' + this.id + '/logo';
  }
}
