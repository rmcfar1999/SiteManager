import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';

import { AdminLayoutComponent } from '../theme/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from '../theme/auth-layout/auth-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
//import { LoginComponent } from './sessions/login/login.component';
import { ApplicationPaths } from '../core/authentication/api-authorization.constants';
import { LoginComponent } from '../core/authentication/login/login.component';
import { LogoutComponent } from '../core/authentication/logout/logout.component';
import { RegisterComponent } from '../core/authentication/registration/register.component';
import { ForgotPasswordComponent } from '../core/authentication/forgotpassword/forgotpassword.component';
import { AuthorizeGuard } from '@core';


const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [AuthorizeGuard],
    canActivateChild: [AuthorizeGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        component: DashboardComponent,
        data: { title: 'Dashboard', titleI18n: 'dashboard' },
      },
      {
        path: 'sessions',
        loadChildren: () => import('./sessions/sessions.module').then(m => m.SessionsModule),
        data: { title: 'Sessions', titleI18n: 'Sessions' },
      },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
        data: { title: 'Administration', titleI18n: 'Administration' },
      },
    ],
  },
  {
    path: 'authentication',
    component: AuthLayoutComponent,
    children: [
      {
        path: "userlogin", //UI For login...identity server will direct to below login callback
        component: LoginComponent,
        data: { title: 'Login', titleI18n: 'Login' },
      },
      {
        path: "login", //ApplicationPaths.Login, //internal identity/oidc callback handler
        component: LoginComponent,
        data: { title: 'Login', titleI18n: 'Login' },
      },
      {
        path: "logout", //ApplicationPaths.LogOut,
        component: LogoutComponent,
        data: { title: 'Logout', titleI18n: 'Logout' },
      },
      {
        path: "login-callback", //ApplicationPaths.LoginCallback,
        component: LoginComponent,
        data: { title: 'Login Callback', titleI18n: 'LoginCallback' },
      },
      {
        path: "register",
        component: RegisterComponent,
        data: { title: 'Register', titleI18n: 'Register' },
      },
      {
        path: "forgotpassword",
        component: ForgotPasswordComponent,
        data: { title: "Forgot Password", titleI18n: "forgotpassword"}
      },
      {
        path: "logout-callback",
        component: LogoutComponent,
        data: { title: 'Logout', titleI18n: 'Logout' },
      },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: environment.useHash, 
    }),
  ],
  exports: [RouterModule],
})
export class RoutesRoutingModule {}
