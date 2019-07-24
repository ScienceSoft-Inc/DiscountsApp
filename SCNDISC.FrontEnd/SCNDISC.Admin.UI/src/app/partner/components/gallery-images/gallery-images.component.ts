import {Component, Input, OnInit} from '@angular/core';
import {Partner} from '../../../shared/models/partner';
import {PartnerService} from '../../../core/services/partner.service';

@Component({
  selector: 'app-gallery-images',
  templateUrl: './gallery-images.component.html',
  styleUrls: ['./gallery-images.component.less']
})
export class GalleryImagesComponent implements OnInit {

  @Input() partner: Partner;

  constructor(private partnerService: PartnerService) {
  }
  ngOnInit() {
  }

  onClickedAddGalleryImage(event): void {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.onload = (Event: any) => {
        const image = Event.target.result;
        const imageBase64 = image.toString().split(',');
        this.saveGalleryImage(imageBase64[1]);
      };
      reader.readAsDataURL(event.target.files[0]);
    }
  }

  saveGalleryImage(imageSource: string): void {
    this.partnerService.saveGalleryImage(this.partner.id, imageSource)
      .subscribe(
        (result: any) => {
          console.log(result);
          if (result) {
            this.partner.gallery.push(result.id);
          }
        },
        (error) => {
          console.log(error);
        }
      );
  }

  onClickedRemove(galleryImageId): void {
    this.partnerService.deleteGalleryImage(galleryImageId)
      .subscribe(
        (success: boolean) => {
          if (success) {
            const position = this.partner.gallery.indexOf(galleryImageId);
            this.partner.gallery.splice(position, 1);
          }
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
