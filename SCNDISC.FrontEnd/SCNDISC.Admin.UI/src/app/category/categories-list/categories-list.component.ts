import {Component, OnDestroy, OnInit} from '@angular/core';
import {CategoryService} from '../../core/services/category.service';
import {Category} from '../../shared/models/category';
import {LanguageService} from '../../core/services/language.service';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Router} from '@angular/router';
import {Subscription} from 'rxjs';


@Component({
  selector: 'app-manager-of-categories',
  templateUrl: './categories-list.component.html',
  styleUrls: ['./categories-list.component.css'],

})
export class CategoriesListComponent implements OnInit, OnDestroy {

  public selectedLanguage: string;
  public LangClass: string;
  public categories = [];
  public errorSaveCategories: boolean;
  public errorCategories: boolean;
  private languageSubscription: Subscription;
  private categoriesSubscription: Subscription;

  constructor(private router: Router, private activeModal: NgbActiveModal, private CategoriesService: CategoryService,
              public translate: LanguageService) {
  }


  ngOnInit(): void {
    this.languageSubscription = this.translate.getCurrent().subscribe((language: string) => {
      this.selectedLanguage = language;
      this.LangClass = (language === this.translate.eng) ? 'x-lang-bar x-lang-bar-ru' : 'x-lang-bar x-lang-bar-en';
    });

    this.categoriesSubscription = this.CategoriesService.getLoadedCategories().subscribe((categories: Category[]) => {
      Object.assign(this.categories, categories);
    });
  }

  onClickedSave(): void {
    for (const category of this.categories) {
      this.errorCategories = false;
      for (const categoryOne of this.categories) {
        if (categoryOne.name_En === category.name_En && categoryOne.name_Ru === category.name_Ru) {
          if (categoryOne.name_En === category.name_En && this.errorCategories) {
            this.errorSaveCategories = true;
            const position = this.categories.indexOf(categoryOne);
            this.categories.splice(position, 1);
          }

          this.errorCategories = true;
        }
      }
    }

    if (this.CategoriesService.saveChanges(this.categories)) {
      this.activeModal.close('Save click');
    }
  }

  onClickedCancel(): void {
    this.activeModal.close('Cancel click');
  }

  onClickedAddNewCategory(): void {
    const cat = new Category();
    this.categories.push(cat);
  }

  onClickedRemove(cat: Category): void {
    const position = this.categories.indexOf(cat);
    this.categories.splice(position, 1);
  }

  ngOnDestroy(): void {
    this.categoriesSubscription.unsubscribe();
    this.languageSubscription.unsubscribe();
    this.router.navigate(['']);
  }
}
