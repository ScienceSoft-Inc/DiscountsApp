import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {CategoriesListComponent} from './filter/categories-list/categories-list.component';
import {CategoryComponent} from './filter/category/category.component';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {HttpClient} from '@angular/common/http';
import {MenuComponent} from './menu/menu.component';
import {WarningComponent} from './alerts/warning.component';
import {LanguageComponent} from './language/language.component';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {ImageLoadingComponent} from '../partner/partners-list/tool-tip-partner/image-loading/image-loading.component';
import {RouterModule} from '@angular/router';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NgbModule,
    FormsModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  declarations: [
    CategoriesListComponent,
    CategoryComponent,
    WarningComponent,
    MenuComponent,
    LanguageComponent,
    ImageLoadingComponent
  ],
  exports: [
    CommonModule,
    RouterModule,
    NgbModule,
    FormsModule,
    TranslateModule,
    CategoriesListComponent,
    CategoryComponent,
    MenuComponent,
    LanguageComponent,
    ImageLoadingComponent
  ],
  entryComponents: [WarningComponent]
})

export class SharedModule { }
