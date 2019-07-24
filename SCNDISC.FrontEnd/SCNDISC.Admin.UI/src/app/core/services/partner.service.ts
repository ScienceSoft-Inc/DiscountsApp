import {Injectable} from '@angular/core';
import {Partner} from '../../shared/models/partner';
import {GalleryImage} from '../../shared/models/gallery-image';
import {BehaviorSubject} from 'rxjs';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {ToolTipPartner} from '../../shared/models/tool-tip-partner';
import {Observable} from 'rxjs/internal/Observable';
import {of} from 'rxjs/internal/observable/of';


@Injectable()
export class PartnerService {

  private partnerSubject = new BehaviorSubject([]);
  private partners: ToolTipPartner[] = [];
  private url = environment.URL;
  public readonly pathNewPartner = 'create';

  constructor(private http: HttpClient) {
  }

  loadAll(selectedCategories?: string[], discountName?: string): Observable<Partner[]> {

    let httpParams: HttpParams = new HttpParams();

    if (selectedCategories) {
      // case when we receive all partners with out categories
      httpParams = httpParams.append('selectedCategories', 'null');
      selectedCategories.forEach(id => {
        httpParams = httpParams.append('selectedCategories', id);
      });
    }

    if (discountName) {
      httpParams = httpParams.set('discountName', discountName);
    }

    this.http.get<Partner[]>(this.url + '/partners', {params: httpParams}).subscribe(res => {
      this.partners = res;
      this.partnerSubject.next(this.partners);
    });
    return this.partnerSubject.asObservable();
  }

  getAllTooltips(): Observable<ToolTipPartner[]> {

    return this.http.get<Partner[]>(this.url + '/partners');
  }

  getById(id: string): Observable<Partner> {
    return this.http.get<Partner>(this.url + '/Partners/' + id + '/details');
  }

  getNew(): Observable<Partner> {
    return of(new Partner());
  }

  saveChanges(partner: Partner): Observable<any> {
    return this.http.post(this.url + '/partners', partner, {headers: this.getHeaders()});

  }

  remove(partner: Partner): Observable<any> {
    return this.http.delete(this.url + '/partners/' + partner.id, {headers: this.getHeaders()});
  }

  getLogo(id: string): Observable<Blob> {
    return this.http.get(this.url + '/discounts/' + id + '/logo', { responseType: 'blob' });
  }

  saveGalleryImage(partnerId: string, image: string): Observable<any> {
    return this.http.post(`${this.url}/galleryimage`, new GalleryImage(partnerId, image), {headers: this.getHeaders()});
  }

  deleteGalleryImage(id: string) {
    return this.http.delete(`${this.url}/galleryimage/${id}`, {headers: this.getHeaders()});
  }

  private getHeaders(): HttpHeaders {
    const token = sessionStorage.getItem(environment.jwt);
    const httpHeaders = new HttpHeaders({
      'Authorization': 'Bearer ' + token,
      'Content-Type': 'application/json'
    });
    return httpHeaders;
  }
}
