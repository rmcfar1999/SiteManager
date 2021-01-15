import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
//import { User, UserManager, Log, WebStorageStateStore } from 'oidc-client';
import { AuthorizeService, AuthenticationResultStatus } from '../authentication/authorize.service';
import { MenuService } from './menu.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root',
})
export class StartupService {
  constructor(
    private menu: MenuService,
    private http: HttpClient,
    private settings: SettingsService,
    private authService: AuthorizeService
   // private userManager: UserManager
  ) {}

  load(): Promise<any> {

    //let userRoles: string[] = ["Public"];
    //this.authService.isAuthenticated().subscribe(authd => {
    this.authService.getUser().subscribe(authd => {
      if (authd) {
       // userRoles = authd.role;
        this.authService.startIdleTimer();
      }
    });
    return new Promise((resolve, reject) => {
      this.http
        .get('assets/data/menu.json?_t=' + Date.now()) //ToDo RMC: consider moving this to api and handle the permissions there (see perms recurs below)
        .pipe(
          catchError(res => {
            resolve();
            return throwError(res);
          })
        )
        .subscribe(
          (res: any) => {
            this.menu.recursMenuForTranslation(res.menu, 'menu');

            this.menu.recursMenuForPermissions(res.menu, ["Public"]); //initialize menu with public visibility...login will update
            this.menu.set(res.menu);
          },
          () => {
            reject();
          },
          () => {
            resolve();
          }
        );
    });
  }
}
