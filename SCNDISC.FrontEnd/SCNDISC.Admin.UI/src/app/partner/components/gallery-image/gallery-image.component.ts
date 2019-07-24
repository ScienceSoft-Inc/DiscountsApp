import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {environment} from '../../../../environments/environment';

@Component({
  selector: 'app-gallery-image',
  templateUrl: './gallery-image.component.html',
  styleUrls: ['./gallery-image.component.less']
})
export class GalleryImageComponent implements OnInit {

  @Output() removeGalleryImage = new EventEmitter<string>();
  @Input() galleryImageId: string;
  @Input() id: number;

  constructor() {
  }

  ngOnInit(): void {
  }

  onClickedRemove(): void {
    this.removeGalleryImage.emit(this.galleryImageId);
  }

  getImageStyle(): object {
    if (!this.galleryImageId) {
      return {'height': '180px'};
    } else {
      const imgUrl = `${environment.URL}/galleryImage/${this.galleryImageId}`;
      return {'background-image': `url('${imgUrl}')`, 'background-size': 'cover'};
    }
  }
}
