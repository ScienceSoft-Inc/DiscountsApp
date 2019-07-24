import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';
import {PartnerDialogComponent } from './partner-dialog/partner-dialog.component';
import {PartnerDetailsComponent} from './partner-details/partner-details.component';
import {BasicInformationComponent} from './components/basic-information/basic-information.component';
import {ContactsComponent} from './components/contacts/contacts.component';
import {ContactComponent} from './components/contact/contact.component';
import {ImagesComponent} from './components/images/images.component';
import {WebAddressComponent} from './components/web-address/web-address.component';
import {WebAddressesComponent} from './components/web-addresses/web-addresses.component';
import {SharedModule} from '../shared/shared.module';
import {PartnersListComponent} from './partners-list/partners-list.component';
import {ToolTipPartnerComponent} from './partners-list/tool-tip-partner/tool-tip-partner.component';
import {AvatarLoadingComponent} from './partners-list/tool-tip-partner/avatar-loading/avatar-loading.component';
import {GalleryImageComponent} from './components/gallery-image/gallery-image.component';
import {GalleryImagesComponent} from './components/gallery-images/gallery-images.component';

@NgModule({
  imports: [
    SharedModule,
    RouterModule
  ],
  declarations: [
    PartnerDialogComponent,
    PartnerDetailsComponent,
    BasicInformationComponent,
    ContactsComponent,
    ContactComponent,
    ImagesComponent,
    WebAddressComponent,
    WebAddressesComponent,
    PartnersListComponent,
    ToolTipPartnerComponent,
    AvatarLoadingComponent,
    GalleryImagesComponent,
    GalleryImageComponent
  ],
  entryComponents: [PartnerDetailsComponent],
  exports: [ToolTipPartnerComponent, PartnersListComponent, PartnerDialogComponent]
})
export class PartnerModule { }
