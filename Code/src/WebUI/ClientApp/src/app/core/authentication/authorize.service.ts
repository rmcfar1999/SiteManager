import { Injectable } from '@angular/core';
import { User, UserManager, Log, WebStorageStateStore } from 'oidc-client';
import { BehaviorSubject, concat, from, Observable, Subscription } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import { ApplicationPaths, ApplicationName } from './api-authorization.constants';
import { Routes, RouterModule, Router } from '@angular/router';
import { UserIdleService } from 'angular-user-idle';
import { MenuService } from '../bootstrap/menu.service';

export type IAuthenticationResult =
  SuccessAuthenticationResult |
  FailureAuthenticationResult |
  RedirectAuthenticationResult;

export interface SuccessAuthenticationResult {
  status: AuthenticationResultStatus.Success;
  state: any;
}

export interface FailureAuthenticationResult {
  status: AuthenticationResultStatus.Fail;
  message: string;
}

export interface RedirectAuthenticationResult {
  status: AuthenticationResultStatus.Redirect;
}

export enum AuthenticationResultStatus {
  Success,
  Redirect,
  Fail
}

export interface IUser {
  name: string;
  sub: number;
  profile: any;
  role: any;
}

@Injectable({
  providedIn: 'root'
})


export class AuthorizeService {

  constructor(private router: Router, private idleTimer: UserIdleService, private menuService: MenuService) {
    Log.logger = console;
    Log.level = Log.DEBUG;
  }
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  private popUpDisabled = true;
  private userManager: UserManager;
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);
  private idleTimerSubscription: Subscription;

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public getUser(): Observable<IUser | null> {
    //console.log("session status", this.userManager.querySessionStatus());

    var a = concat(
      this.userSubject.pipe(take(1), filter(u => !!u)),
      this.getUserFromStorage().pipe(filter(u => !!u),
      tap(u => this.userSubject.next(u))),
      this.userSubject.asObservable());
    return a; 
  }

  public getAccessToken(): Observable<string> {
    return from(this.ensureUserManagerInitialized())
      .pipe(mergeMap(() => from(this.userManager.getUser())),
        map(user => user && user.access_token));
  }

  // We try to authenticate the user in three different ways:
  // 1) We try to see if we can authenticate the user silently. This happens
  //    when the user is already logged in on the IdP and is done using a hidden iframe
  //    on the client.
  // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
  //    redirect flow.
  public async signIn(state: any): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    let user: User = null;
    try {
      user = await this.userManager.signinSilent(this.createArguments());
      
      this.userSubject.next(user.profile);
      return this.success(state);
    } catch (silentError) {
      /// User might not be authenticated, fallback to popup authentication
      console.log('Silent authentication error: ', silentError);

      try {
        if (this.popUpDisabled) {
          throw new Error('Popup disabled. Change \'authorize.service.ts:AuthorizeService.popupDisabled\' to false to enable it.');
        }
        user = await this.userManager.signinPopup(this.createArguments());
        this.userSubject.next(user.profile);
        return this.success(state);
      } catch (popupError) {
        if (popupError.message === 'Popup window closed') {
          // The user explicitly cancelled the login action by closing an opened popup.
          return this.error('The user closed the window.');
        } else if (!this.popUpDisabled) {
          console.log('Popup authentication error: ', popupError);
        }

        //// PopUps might be blocked by the user, fallback to redirect
        try {
          return this.redirect();
        } catch (redirectError) {
          console.log('Redirect authentication error: ', redirectError);
          return this.error(redirectError);
        }
      }
    }
  }

  public async startIdleTimer() {

    this.idleTimer.stopTimer();
    this.idleTimer.stopWatching;
    if (this.idleTimerSubscription) {
      this.idleTimerSubscription.unsubscribe();
    }

    //Start watching for user inactivity.
    console.log("Starting idle timer");
    this.idleTimer.setConfigValues({ idle: 600, timeout: 1, ping: 120 }); //until there is a popup warning for timeout...just timeout
    this.idleTimer.startWatching(); //set in /app.module currently 

    //this.idleTimer.ping$.pipe(take(5), map(val => this.doThis(val))).subscribe(res => {
    //  //this.userManager.querySessionStatus().then(val => console.log("Session status", val)); 
    //});


    // begins firing after above idle count (i.e. 600 s)
    // can be used as a modal timeout warning/countdown
    this.idleTimer.onTimerStart().subscribe(count => {
      console.log("Timing out in: ", count);
    });

    // fires after idle timeout + warning timer 
    this.idleTimerSubscription = this.idleTimer.onTimeout().pipe(take(1)).subscribe((res) => {
      console.log('Signing out due to inactivity.', res);
      this.signOut({});
    });

  }
  public async redirectToPermissionDenied()
  {
    try {
      await this.router.navigateByUrl('/sessions/403');
    } catch (redirectError) {
      console.log('Redirect authentication error: ', redirectError);
      return null; //this.error(redirectError);
    }
  }

  public async redirectToSignin(state?: any) : Promise<User> {
    
    try {

      //redirect method..reloads app but works
      //signin silent seems to have some funky issues with it's iframe...probably fine in a prod
      //environment but spaProxy and what not in vs/vscode is funky Frame Timeout issues
      await this.userManager.signinRedirect(this.createArguments(state));
    
    } catch (redirectError) {
      console.log('Redirect authentication error: ', redirectError);
      return null; //this.error(redirectError);
    }
  }

  public async completeSignIn(url: string): Promise<IAuthenticationResult> {
    try {
      await this.ensureUserManagerInitialized();
      const user = await this.userManager.signinCallback(url);
      this.userSubject.next(user && user.profile);
      this.menuService.getAll().subscribe(menu => {
        console.log("menu", menu);
        this.menuService.recursMenuForPermissions(menu, user.profile.role);
      });

      return this.success(user && user.state);
    } catch (error) {
      console.log('There was an error signing in: ', error);
      return this.error('There was an error signing in.');
    }
  }

  public async signOut(state: any): Promise<IAuthenticationResult> {
    try {
      if (this.popUpDisabled) {
        throw new Error('Popup disabled. Change \'authorize.service.ts:AuthorizeService.popupDisabled\' to false to enable it.');
      }

      await this.ensureUserManagerInitialized();
      await this.userManager.signoutPopup(this.createArguments());
      this.userSubject.next(null);
      return this.success(state);
    } catch (popupSignOutError) {
      console.log('Popup signout error: ', popupSignOutError);
      try {
        await this.userManager.signoutRedirect(this.createArguments(state));
        return this.redirect();
      } catch (redirectSignOutError) {
        console.log('Redirect signout error: ', popupSignOutError);
        return this.error(redirectSignOutError);
      }
    }
  }

  public async completeSignOut(url: string): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    try {
      const state = await this.userManager.signoutCallback(url);
      this.userSubject.next(null);
      return this.success(state && state.data);
    } catch (error) {
      console.log(`There was an error trying to log out '${error}'.`);
      return this.error(error);
    }
  }

  private createArguments(state?: any): any {
    return { useReplaceToNavigate: true, data: state };
  }

  private error(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(state: any): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, state };
  }

  private redirect(): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Redirect };
  }

  private async ensureUserManagerInitialized(): Promise<void> {
    if (this.userManager !== undefined) {
      return;
    }

    const response = await fetch(ApplicationPaths.ApiAuthorizationClientConfigurationUrl);
    if (!response.ok) {
      throw new Error(`Could not load settings for '${ApplicationName}'`);
    }

    const settings: any = await response.json();
    //see "other optional settings" at https://gist.github.com/davidamidon/24c8a6980e116e62f781be4d6239d10d
    //settings.silent_redirect_uri = "https://localhost:44312/authentication/login-callback";
    settings.automaticSilentRenew = true;
    settings.includeIdTokenInSilentRenew = true;
    this.userManager = new UserManager(settings);
    
    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager.removeUser();
      this.userSubject.next(null);
    });
  }

  private getUserFromStorage(): Observable<IUser> {
    return from(this.ensureUserManagerInitialized())
      .pipe(
        mergeMap(() => this.userManager.getUser()),
        map(u => u && u.profile));
  }
}
