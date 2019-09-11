import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {HomeComponent} from './home/home.component';
import {AuthGuard} from './core/auth/auth.guard';
import {ResolverService} from './core/services/resolver.service';
import {PartnerDialogComponent} from './partner/partner-dialog/partner-dialog.component';
import {WarningComponent} from './shared/alerts/warning.component';

const itemRoutes: Routes = [
  {path: 'partner/:id', component: PartnerDialogComponent, resolve: {data: ResolverService}},
  {path: 'categories', loadChildren: () => import('./category/category.module').then(m => m.CategoryModule)},
  {path: 'feedback', loadChildren: () => import('./feedback/feedback.module').then(f => f.FeedbackModule)},
  {path: 'notification', loadChildren: () => import('./notification/notification.module').then(n => n.NotificationModule)}
];

export const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {
    path: '',
    component: HomeComponent,
    children: itemRoutes,
    canActivate: [AuthGuard]
  },
  {path: 'error', component: WarningComponent},
  {path: '**', redirectTo: 'login'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
