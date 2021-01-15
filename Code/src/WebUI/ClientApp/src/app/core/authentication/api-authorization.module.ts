import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { ApplicationPaths } from './api-authorization.constants';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';
import { RegisterComponent } from './registration/register.component';
import { ForgotPasswordComponent} from './forgotpassword/forgotpassword.component';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    SharedModule
    //Decided to keep these in main routes-routing module
    //RouterModule.forChild(
    //  [
    //    { path: ApplicationPaths.Register, component: LoginComponent },
    //    { path: ApplicationPaths.Profile, component: LoginComponent },
    //    { path: ApplicationPaths.Login, component: LoginComponent },
    //    { path: ApplicationPaths.LoginFailed, component: LoginComponent },
    //    { path: ApplicationPaths.LoginCallback, component: LoginComponent },
    //    { path: ApplicationPaths.LogOut, component: LogoutComponent },
    //    { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
    //    { path: ApplicationPaths.LogOutCallback, component: LogoutComponent }
    //  ]
    //)
  ],
  declarations: [LoginMenuComponent, LoginComponent, LogoutComponent, RegisterComponent, ForgotPasswordComponent],
  exports: [LoginMenuComponent, LoginComponent, LogoutComponent, RegisterComponent, ForgotPasswordComponent, RouterModule]
})
export class ApiAuthorizationModule { }
