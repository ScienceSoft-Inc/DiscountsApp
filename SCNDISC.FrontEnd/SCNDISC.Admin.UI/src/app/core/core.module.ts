import {NgModule} from '@angular/core';
import {CategoryService} from './services/category.service';
import {LanguageService} from './services/language.service';
import {PartnerService} from './services/partner.service';
import {ResolverService} from './services/resolver.service';
import {AuthService} from './auth/auth.service';
import {MenuService} from './services/menu.service';
import {SharedModule} from '../shared/shared.module';

@NgModule({
  imports: [
    SharedModule
  ],
  providers: [
    CategoryService,
    LanguageService,
    PartnerService,
    ResolverService,
    AuthService,
    MenuService
  ]
})
export class CoreModule { }
