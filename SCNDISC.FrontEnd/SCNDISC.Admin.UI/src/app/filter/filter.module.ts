import { NgModule } from '@angular/core';
import {FilterByCategoryComponent} from './filter-by-category/filter-by-category.component';
import {FilterByNameComponent} from './filter-by-name/filter-by-name.component';
import {SharedModule} from '../shared/shared.module';
import {FilterService} from './filter.service';


@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [FilterByCategoryComponent, FilterByNameComponent],
  providers: [FilterService],
  exports: [FilterByCategoryComponent, FilterByNameComponent]
})

export class FilterModule { }
