import {Component, Input, OnInit} from '@angular/core';
import {Partner} from '../../../shared/models/partner';

@Component({
  selector: 'app-files',
  templateUrl: './images.component.html',
  styleUrls: ['./images.component.less']
})
export class ImagesComponent implements OnInit {

  @Input() partner: Partner;
  public image: string;
  public logo: string;

  constructor() {
  }

  ngOnInit(): void {
    this.logo = this.partner.logo;
    this.image = this.partner.image;
  }

  onClickedChangeImage(event): void {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.onload = (Event: any) => {
        this.image = Event.target.result;
        const imageBase64 = this.image.toString().split(',');
        this.partner.image = imageBase64[1];
      };
      reader.readAsDataURL(event.target.files[0]);
    }
  }

  onClickedChangeLogo(event): void {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.onload = (Event: any) => {
        this.logo = Event.target.result;
        const LogoBase64 = this.logo.toString().split(',');
        this.partner.logo = LogoBase64[1];
      };

      reader.readAsDataURL(event.target.files[0]);
    }
  }

  getLogoStyle(): object {
    if (!this.partner.logo) {
      return {'height': '65px'};
    } else {
      const imgUrl = 'data: image/png; base64,' + this.partner.logo;
      return {'background-image': `url('${imgUrl}')`};
    }

  }

  getImageStyle(): object {
    if (!this.partner.image) {
      return {'height': '180px'};
    } else {
      const imgUrl = 'data: image/png; base64,' + this.partner.image;
      return {'background-image': `url('${imgUrl}')`, 'background-size': 'cover'};
    }

  }
}
