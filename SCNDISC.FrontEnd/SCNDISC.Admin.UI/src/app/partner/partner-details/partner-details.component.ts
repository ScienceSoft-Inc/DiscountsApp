import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Partner} from '../../shared/models/partner';
import {Router} from '@angular/router';
import {PartnerService} from '../../core/services/partner.service';
import {CategoryService} from '../../core/services/category.service';
import {Subscription} from 'rxjs';
import {LanguageService} from '../../core/services/language.service';
import {FilterService} from '../../filter/filter.service';

@Component({
  selector: 'app-partner',
  templateUrl: './partner-details.component.html',
  styleUrls: ['./partner-details.component.less']
})

export class PartnerDetailsComponent implements OnInit, OnDestroy {

  @Input() partner: Partner;
  public categoryIds: string[];
  public errorName: boolean;
  public errorDescription: boolean;
  public errorDiscountValue: boolean;
  public errorPhone: boolean;
  public errorCoordinates: boolean;
  public errorDiscount: boolean;
  public errorAddress: boolean;
  private languageSubscription: Subscription;
  private currentLang: string;


  constructor(public activeModal: NgbActiveModal, private partnerService: PartnerService, private languageService: LanguageService,
              private categoryService: CategoryService, private router: Router, private filterService: FilterService) {
  }

  ngOnInit(): void {
    if (this.partner.categories) {
      this.categoryIds = this.partner.categories.map(x => x.id);
    }
    this.languageSubscription = this.languageService.getCurrent().subscribe(lang => this.currentLang = lang);
  }

  onClickedCancel(): void {
    this.activeModal.close('Close click');
  }

  onClickedRemove(): void {
    if (confirm(this.languageService.getTranslate('partner.DeleteQuestion'))) {
      this.partnerService.remove(this.partner).subscribe(() => {
        this.partnerService.loadAll(this.filterService.getCategories(), this.filterService.getFilterName());
      });
      this.onClickedCancel();
    }
  }

  onClickedSave(): void {
    this.checkNameAndDescription();
    this.checkDiscountValue();
    this.checkContacts();
    if (!this.errorName && !this.errorDiscountValue && !this.errorDescription && !this.errorDiscount &&
      !this.errorCoordinates && !this.errorPhone) {
      this.changeCategories();
      this.partnerService.saveChanges(this.partner).subscribe(() => {
        this.partnerService.loadAll(this.filterService.getCategories(), this.filterService.getFilterName());
      });
      this.onClickedCancel();
    }
  }

  checkContacts(): void {
    if (this.partner.contacts) {
      for (const contact of this.partner.contacts) {
        const regularForPhone = /^[\d\(\)\+\ -]{3,22}\d$/;
        const regularForCoordinates = /^-?\d+(\.\d+)?, ?-?\d+(\.\d+)?$/;
        const validNumber1 = regularForPhone.test(contact.phoneNumber1);
        const validNumber2 = regularForPhone.test(contact.phoneNumber2);
        const validCoordinates = regularForCoordinates.test(contact.coordinates);
        if ((validNumber1 || !contact.phoneNumber1) && (validNumber2 || !contact.phoneNumber2)) {
          this.errorPhone = false;
          if (validCoordinates) {
            this.errorCoordinates = false;
          } else {
            this.errorCoordinates = true;
            return;
          }
        } else {
          this.errorPhone = true;
          return;
        }
      }
      return;
    }
  }

  checkNameAndDescription(): void {
    if (this.currentLang === this.languageService.eng) {
      this.errorName = this.partner.name_En ? false : true;
      this.errorDescription = this.partner.description_En ? false : true;
    } else if (this.currentLang === this.languageService.rus) {
      this.errorName = this.partner.name_Ru ? false : true;
      this.errorDescription = this.partner.description_Ru ? false : true;
    }
    if (this.partner.discount) {
      this.errorDiscountValue = false;
    } else {
      this.errorDiscountValue = true;
    }
  }

  checkDiscountValue(): void {
    let test: boolean;
    if (this.partner.discount) {
      const regularForDiscount = /^[\d]{0,4}$/;
      test = regularForDiscount.test(this.partner.discount.toString());
    }
    if ((this.partner.selectDiscount === '0' && this.partner.discount < 100 && this.partner.discount > 0 && test) ||
      (this.partner.selectDiscount === '1' && this.partner.discount < 1000 && this.partner.discount > 0 && test)) {
      this.errorDiscount = false;
    } else {
      this.errorDiscount = true;
    }
  }

  changeCategories(): void {
    this.partner.categories.splice(0, this.partner.categories.length);
    for (const id of this.categoryIds) {
      const category = this.categoryService.getById(id);
      this.partner.categories.push(category);
    }
  }

  ngOnDestroy(): void {
    this.languageSubscription.unsubscribe();
    this.router.navigate(['']);
  }
}
