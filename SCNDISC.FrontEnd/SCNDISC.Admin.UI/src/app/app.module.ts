import {BrowserModule} from '@angular/platform-browser';
import {ErrorHandler, NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HomeComponent} from './home/home.component';
import {TranslateModule, TranslateLoader} from '@ngx-translate/core';
import {HttpClientModule, HttpClient} from '@angular/common/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {HttpLoaderFactory, SharedModule} from './shared/shared.module';
import {CoreModule} from './core/core.module';
import {HeaderComponent } from './home/header/header.component';
import {FilterModule} from './filter/filter.module';
import {PartnerModule} from './partner/partner.module';
import {LoginModule} from './login/login.module';
import {GlobalErrorHandler} from './core/services/global-error-handler.service';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent
  ],
  imports: [
    CoreModule,
    LoginModule,
    BrowserModule,
    NgbModule,
    FormsModule,
    PartnerModule,
    FilterModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  bootstrap: [AppComponent],
  providers: [{provide: ErrorHandler, useClass: GlobalErrorHandler}]
})
export class AppModule {
}
