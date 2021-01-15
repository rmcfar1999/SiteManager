import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { AccountClient, RegisterCommand } from '../../../SiteManager.V4-api'
import { LoginActions, QueryParameterNames, ApplicationPaths, ReturnUrlType } from '../api-authorization.constants';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  accountClient: AccountClient;
  activatedRoute: ActivatedRoute;
  error: string;

  constructor(private fb: FormBuilder, private client: AccountClient, private activeRoute: ActivatedRoute) {
    this.accountClient = client;
    this.activatedRoute = activeRoute;

    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [this.passwordValidator]],
      confirmPassword: ['', [this.confirmValidator]],
    });
  }

  ngOnInit() {}

  get username() {
    return this.registerForm.get('username');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  public async registerNewUser() {
    var mdl = RegisterCommand.fromJS({
      userName: this.username.value,
      password: this.password.value,
      confirmPassword: this.confirmPassword.value, 
      email: this.username.value, 
      phoneNumber: "",
      returnUrl: this.getReturnUrl, 
      registrationUrl: ""
    });

    this.accountClient.register(mdl).subscribe(
      result => {
        console.log(result);
      },
      error => {
        console.log(error);
        var e = JSON.parse(error.response);
        for (var err in e.errors) {
          if (e.errors[err]) {
            this.error = e.errors[err];
            break;
          }        
        }
      }
    ); 
    
  }

  private getReturnUrl(): string {
    const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;
    // If the url is comming from the query string, check that is either
    // a relative url or an absolute url
    if (fromQuery &&
      !(fromQuery.startsWith(`${window.location.origin}/`) ||
        /\/[^\/].*/.test(fromQuery))) {
      // This is an extra check to prevent open redirects.
      throw new Error('Invalid return url. The return url needs to have the same origin as the current page.');
    }
    return fromQuery || "/"
  }

  confirmValidator = (control: FormControl): { [k: string]: boolean } => {
    if (!control.value) {
      return { error: true, required: true };
    } else if (control.value !== this.registerForm.controls.password.value) {
      return { error: true, confirm: true };
    }
    return {};
  };

  passwordValidator = (control: FormControl): { [k: string]: boolean } => {
    // {6,100}           - Assert password is between 8 and 100 characters
    // (?=.*[0-9])       - Assert a string has at least one number
    if (control.value) {
      var regex = "^(?=.*[0-9])"
        + "(?=.*[a-z])(?=.*[A-Z])"
        + "(?=.*[@#$%^&+=_!])"
        + "(?=\\S+$).{6,20}$";
      if (control.value.match(regex)) {
      //.match(/^(?=.*[0-9])[a-zA-Z0-9!@#$%^&*]{6,100}$/)) {
        return {};
      } else {
        return { error: true, required: true };
      }
    }
  }
}

interface INavigationState {
  [ReturnUrlType]: string;
}


