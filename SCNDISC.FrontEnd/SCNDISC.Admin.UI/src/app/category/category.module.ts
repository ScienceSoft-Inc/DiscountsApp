import { NgModule } from '@angular/core';
import {CategoryDialogComponent} from './category-dialog/category-dialog.component';
import {CategoryItemComponent} from './category-item/category-item.component';
import {CategoriesListComponent} from './categories-list/categories-list.component';
import {SharedModule} from '../shared/shared.module';
import {RouterModule} from '@angular/router';

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: CategoryDialogComponent}
    ])
  ],
  declarations: [
    CategoryDialogComponent,
    CategoryItemComponent,
    CategoriesListComponent
  ],
  entryComponents: [CategoriesListComponent]
})
export class CategoryModule { }
