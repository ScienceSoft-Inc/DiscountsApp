import {Injectable, OnDestroy} from '@angular/core';
import {PartnerService} from '../core/services/partner.service';
import {CategoryService} from '../core/services/category.service';
import {Category} from '../shared/models/category';
import {BehaviorSubject, Observable, Subscription} from 'rxjs/index';

@Injectable()
export class FilterService implements OnDestroy {

  private categoryIds = [];
  private lengthOfCategories: number;
  private filterName: string;
  private changeCategoriesIdsSubject = new BehaviorSubject([]);
  private categorySubscription: Subscription;
  private categoryStaticSubscription: Subscription;

  constructor(private partnerService: PartnerService, private categoryService: CategoryService) {
  }

  initCategoryIds(): void {
    this.categorySubscription = this.categoryService.getAll().subscribe((categories: Category []) => {
      this.categoryIds = categories.map(x => x.id);
      this.lengthOfCategories = categories.length;
      this.changeCategoriesIdsSubject.next(this.categoryIds);
    });
  }

  getLengthOfCategoryIds(): number {
    return this.lengthOfCategories;
  }

  getCategoryIds(): Observable<string[]> {
    return this.changeCategoriesIdsSubject.asObservable();
  }

  getCategories(): string[] {
    return this.categoryIds;
  }

  applyFilterByCategory(categoryIds: string[]): void {
    this.categoryIds = categoryIds;
    this.partnerService.loadAll(categoryIds, this.filterName);
    this.changeCategoriesIdsSubject.next(categoryIds);
  }

  applyFilterByName(filterName): void {
    this.partnerService.loadAll(this.categoryIds, filterName);
  }

  setFilterName(filterName): void {
    this.filterName = filterName;
  }

  getFilterName(): string {
    return this.filterName;
  }

  ngOnDestroy(): void {
    this.categoryStaticSubscription.unsubscribe();
    this.categorySubscription.unsubscribe();
  }

}
