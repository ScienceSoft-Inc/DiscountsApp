import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {CategoryService} from '../../../core/services/category.service';
import {Category} from '../../models/category';
import {Subscription} from 'rxjs';
import {debounceTime} from 'rxjs/operators';
import {Subject} from 'rxjs/internal/Subject';

@Component({
  selector: 'app-category-filter',
  templateUrl: './categories-list.component.html',
  styleUrls: ['./categories-list.component.css']
})
export class CategoriesListComponent implements OnInit, OnDestroy {

  @Output() changeCategoryIds = new EventEmitter();
  @Input() categoryIds: string[];
  @Input() selectAllCategories: boolean;
  @Input() categoriesForPartner: boolean;
  public categories: Category[];
  private clicksCategory = new Subject();
  private categoriesSubscription: Subscription;
  private clicksCategorySubscription: Subscription;

  constructor(private categoryService: CategoryService) {
  }

  ngOnInit(): void {

    this.clicksCategorySubscription = this.clicksCategory.pipe(
      debounceTime(300)
    ).subscribe(e => this.changeCategoryIds.emit(e));

    if (this.categoriesForPartner) {
      this.categoriesSubscription = this.categoryService.getLoadedCategories().subscribe((categories: Category[]) => {
        this.categories = categories;
      });
    } else {
      this.categoriesSubscription = this.categoryService.getAll().subscribe((categories: Category[]) => {
        this.categories = categories;
      });
    }

  }

  onClickedCategory(): void {
    this.clicksCategory.next(event);
  }

  ngOnDestroy(): void {
    this.categoriesSubscription.unsubscribe();
    this.clicksCategorySubscription.unsubscribe();
  }

}
