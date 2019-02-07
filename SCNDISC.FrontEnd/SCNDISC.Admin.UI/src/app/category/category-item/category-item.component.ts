import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Category} from '../../shared/models/category';
import {LanguageService} from '../../core/services/language.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-category-for-manager',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.css']
})
export class CategoryItemComponent implements OnInit, OnDestroy {

  @Input() category: Category;
  @Output() remove = new EventEmitter<Category>();
  public lang_en: boolean;
  private languageSubscription: Subscription;

  constructor(private translate: LanguageService) {
  }

  ngOnInit(): void {
    this.languageSubscription = this.translate.getCurrent().subscribe((language: string) => {
      this.lang_en = (language === this.translate.eng);
    });
  }

  onClickedRemove(): void {
    this.remove.emit(this.category);
  }

  ngOnDestroy(): void {
    this.languageSubscription.unsubscribe();
  }
}
