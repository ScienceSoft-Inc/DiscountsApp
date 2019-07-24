/// <reference types="@types/googlemaps" />
import {ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Contact} from '../../../shared/models/contact';
import {LanguageService} from '../../../core/services/language.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContactComponent implements OnInit, OnDestroy {

  @Input() contact: Contact;
  public geocoder: google.maps.Geocoder;
  public errorCoordinates: boolean;
  public errorAddress: boolean;
  public language_en: boolean;
  private languageSubscription: Subscription;

  constructor(private changeDetectorRef: ChangeDetectorRef, private translate: LanguageService) {
  }

  ngOnInit(): void {
    this.geocoder = new google.maps.Geocoder();
    this.languageSubscription = this.translate.getCurrent().subscribe((language: string) => {
      this.language_en = (language === this.translate.eng);
    });
  }

  onClickedOpenMap(): boolean {
    if (!this.contact.coordinates) {
      this.errorCoordinates = true;
      return false;
    }

    const coordinateParts = this.contact.coordinates.split(',');
    if (!coordinateParts || coordinateParts.length !== 2) {
      this.errorCoordinates = true;
      return false;
    } else {
      const lat = coordinateParts[0];
      const lon = coordinateParts[1];
      const url = 'http://maps.google.com/maps?z=12&t=m&q=loc:' + lat + '+' + lon;
      window.open(url, '_blank');
      this.errorCoordinates = false;
    }
    return false;
  }

  onClickedGetCoordinates(): void {
    if (this.language_en) {
      this.getCoordinatesByAddress(this.contact.address_En);
    } else {
      this.getCoordinatesByAddress(this.contact.address_Ru);
    }
  }

  getCoordinatesByAddress(address: string): void {
    this.geocoder.geocode({'address': address}, (results, status) => {
      if (status === google.maps.GeocoderStatus.OK) {
        this.contact.coordinates = results[0].geometry.location.lat().toString() + ',' + results[0].geometry.location.lng().toString();
        this.errorAddress = false;
        this.changeDetectorRef.detectChanges();
      } else {
        this.errorAddress = true;
        this.changeDetectorRef.detectChanges();
      }
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription.unsubscribe();
  }
}

