import {AfterViewInit, Component, Input} from '@angular/core';
import {environment} from '../../../../../environments/environment';

@Component({
  selector: 'app-image-loading',
  templateUrl: './image-loading.component.html',
  styleUrls: ['./image-loading.component.css']
})
export class ImageLoadingComponent implements AfterViewInit {

  @Input() id: string;

  constructor() {
  }

  ngAfterViewInit(): void {
    const url = this.getUrl();
    const img = new Image();
    img.height = 180;
    img.width = 340;
    img.src = url;
    const id = this.id;
      img.onload = () => {
        const element = document.getElementById(id);
         if(element){
           element.appendChild(img);
       }
      };
      this.getStyle();
  }

  getStyle(){
    setTimeout(()=> {
      if(document.getElementById(this.id)){
        document.getElementById(this.id).style.backgroundImage = `url("assets/images/camera-128x128.png")`;
      }
    }, 5000);
  }

  getUrl(): string {
    return environment.URL + '/discounts/' + this.id + '/image';
  }

}
