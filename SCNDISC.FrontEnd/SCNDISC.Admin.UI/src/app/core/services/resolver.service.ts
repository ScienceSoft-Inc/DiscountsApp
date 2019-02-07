import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {PartnerService} from './partner.service';
import {Observable} from 'rxjs/internal/Observable';
import {Partner} from '../../shared/models/partner';

@Injectable()
export class ResolverService implements Resolve<Observable<Partner>> {

  constructor(private partnerService: PartnerService) {
  }

  resolve(route: ActivatedRouteSnapshot): Observable<Partner> {
    if (route.params.id === this.partnerService.pathNewPartner) {
      return this.partnerService.getNew();
    } else {
      return this.partnerService.getById(route.params.id);
    }
  }

}
