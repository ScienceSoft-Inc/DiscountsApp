import {AfterViewInit, Component, Input} from '@angular/core';
import {environment} from '../../../../../environments/environment';

@Component({
  selector: 'app-image-loading',
  templateUrl: './image-loading.component.html',
  styleUrls: ['./image-loading.component.less']
})
export class ImageLoadingComponent implements AfterViewInit {

  @Input() id: string;

  constructor() {
  }

  ngAfterViewInit(): void {
    const url = this.getUrl();
    const img = new Image();
    img.src = `${url}?v=${new Date().getTime()}`;
    img.classList.add('theme-image-settings');

    img.onload = () => {
      const element = document.getElementById(this.id);
      if (element && element.childElementCount === 0) {
        element.appendChild(img);
      }
    };
    this.getStyle();
  }

  getStyle() {
    setTimeout(() => {
      if (document.getElementById(this.id)) {
        document.getElementById(this.id).style.backgroundImage = `url("assets/images/camera-128x128.png")`;
      }
    }, 5000);
  }

  getUrl(): string {
    return environment.URL + '/discounts/' + this.id + '/image';
  }

}
