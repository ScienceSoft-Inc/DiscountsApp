import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Category} from '../../models/category';
import {CategoryService} from '../../../core/services/category.service';
import {LanguageService} from '../../../core/services/language.service';
import {Subscription} from 'rxjs';
import {ColorHelper} from '../../../Helpers/color-helper';

@Component({
  selector: 'app-category-for-filter',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.less']
})
export class CategoryComponent implements OnInit, OnDestroy {

  @Output() changeCategoryIds = new EventEmitter();
  @Input() categoryIds: string[];
  @Input() category: Category;
  public enterCategory: boolean;
  public lang_en: boolean;
  private selectCategorySubscription: Subscription;
  private languageSubscription: Subscription;

  constructor(private categoryService: CategoryService, private translate: LanguageService) {
  }

  ngOnInit(): void {
    this.selectCategorySubscription = this.categoryService.getSubjectSelectAll().subscribe((selectCategory: boolean) => {
      this.enterCategory = !selectCategory;
    });
    this.enterCategory = false;
    if (this.categoryIds) {
      for (const Id of this.categoryIds) {
        if (Id === this.category.id) {
          this.enterCategory = true;
        }
      }

    }

    this.languageSubscription = this.translate.getCurrent().subscribe((language: string) => {
      this.lang_en = (language === 'Eng');
    });
  }

  getColorClassForLabel(): object {
    const textColor = ColorHelper.GetColorByBackHex(this.category.color);
    return {'background-color': this.category.color, 'color': textColor};
  }

  getColorClassForCheckBox(): object {
    if (this.enterCategory) {
      const background = (ColorHelper.IsBlackColorByBackHex(this.category.color))
        ? `url('assets/images/accept-black.png') no-repeat center center ${this.category.color}`
        : `url('assets/images/accept-white.png') no-repeat center center ${this.category.color}`;
      return {'background': background};
    } else {
      return {'background-color': this.category.color};
    }
  }

  onClickedCategory(): void {
    setTimeout(() => {
      if (this.enterCategory) {
        this.categoryIds.push(this.category.id);
      } else {
        const position = this.categoryIds.findIndex(x => x === this.category.id);
        this.categoryIds.splice(position, 1);
      }
      this.changeCategoryIds.emit();
    });
  }

  ngOnDestroy(): void {
    this.languageSubscription.unsubscribe();
    this.selectCategorySubscription.unsubscribe();
  }
}
