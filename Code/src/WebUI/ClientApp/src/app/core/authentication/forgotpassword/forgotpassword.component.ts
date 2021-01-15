import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountClient, ForgotPasswordCommand } from '../../../SiteManager.V4-api'
import { LoginActions, QueryParameterNames, ApplicationPaths, ReturnUrlType } from '../api-authorization.constants';

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);
  public loginForm: FormGroup;
  public error: string;
  public success: boolean; 

  constructor(
    private fb: FormBuilder,
    private accountClient: AccountClient,
    private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  async ngOnInit() {

  }

  get email() {
    return this.loginForm.get('email');
  }


  public async resetPassword() {
    var mdl = ForgotPasswordCommand.fromJS({
      email: this.email.value,
    });

    this.accountClient.forgotPassword(mdl)
      .subscribe(
        result => {
          this.success = true; 
          //console.log(result);
        },
        error => {
          this.success = true;
          //console.log(error);
        }
      );


  }

  public navigateToLogin() {
    this.router.navigateByUrl("/authentication/userlogin"); 

  }
  //private redirectToLogin(): any {
  //  this.redirectToApiAuthorizationPath(
  //    `${ApplicationPaths.Login}?returnUrl=${encodeURI('/dashboard')}`);
  //}

  //private redirectToApiAuthorizationPath(apiAuthorizationPath: string) {
  //  // It's important that we do a replace here so that when the user hits the back arrow on the
  //  // browser they get sent back to where it was on the app instead of to an endpoint on this
  //  // component.
  //  const redirectUrl = `${window.location.origin}${apiAuthorizationPath}`;
  //  window.location.replace(redirectUrl);
  //}
}

interface INavigationState {
  [ReturnUrlType]: string;
}
