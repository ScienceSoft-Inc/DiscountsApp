import {Component, OnDestroy, OnInit} from '@angular/core';
import {FilterService} from '../filter.service';
import {debounceTime} from 'rxjs/operators';
import {Subscription} from 'rxjs/internal/Subscription';
import {Subject} from 'rxjs/internal/Subject';
import {Category} from '../../shared/models/category';
import {CategoryService} from '../../core/services/category.service';

@Component({
  selector: 'app-filter-by-category',
  templateUrl: './filter-by-category.component.html',
  styleUrls: ['./filter-by-category.component.css']
})
export class FilterByCategoryComponent implements OnInit, OnDestroy {

  private clicksSelectAllSubscription: Subscription;
  private CategoriesSubscription: Subscription;
  private loadedCategoriesSubscription: Subscription;
  private clicksSelectAll = new Subject();
  private lengthOfCategories: number;
  public selectAllCategories = true;
  public categoryIds: string[];

  constructor(private filterService: FilterService, private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    this.clicksSelectAllSubscription = this.clicksSelectAll.pipe(
      debounceTime(300)
    ).subscribe(() => this.selectAll());

    this.filterService.initCategoryIds();
    this.lengthOfCategories = this.filterService.getLengthOfCategoryIds();
    this.CategoriesSubscription = this.filterService.getCategoryIds().subscribe(
      (categories: string[]) => {
        this.categoryIds = categories;
        this.lengthOfCategories = this.filterService.getLengthOfCategoryIds();
      });
  }

  onClickedSelectAll(): void {
    this.clicksSelectAll.next();
  }

  selectAll(): void {
    if (this.selectAllCategories) {
      this.loadedCategoriesSubscription = this.categoryService.getLoadedCategories().subscribe((categories: Category []) => {
        this.categoryIds = categories.map(x => x.id);
        this.filterService.applyFilterByCategory(this.categoryIds);
      });
    } else {
      for (let i = 0; i < this.lengthOfCategories; i++) {
        this.categoryIds.pop();
      }

      this.filterService.applyFilterByCategory(this.categoryIds);
    }

    this.categoryService.selectAll(!this.selectAllCategories);
  }

  changeCategoryIds(): void {
    if (this.lengthOfCategories > this.categoryIds.length) {
      this.selectAllCategories = false;
    } else {
      this.selectAllCategories = true;
    }

    this.filterService.applyFilterByCategory(this.categoryIds);
  }

  ngOnDestroy(): void {
    if (this.loadedCategoriesSubscription) {
      this.loadedCategoriesSubscription.unsubscribe();
    }
    this.clicksSelectAllSubscription.unsubscribe();
    this.CategoriesSubscription.unsubscribe();
  }

}
