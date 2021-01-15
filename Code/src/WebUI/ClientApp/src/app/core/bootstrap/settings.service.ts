import { Injectable } from '@angular/core';
import { LocalStorageService } from '@shared/services/storage.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppSettings, defaults } from '../settings';
import { AuthorizeService } from '../authentication/authorize.service';


export const USER_KEY = 'usr';

export interface IAppUser {
  id: number;
  name?: string;
  email?: string;
  avatar?: string;
  roles?: string[];
}

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  constructor(private store: LocalStorageService, private authService: AuthorizeService) { }

  private options = defaults;

  get notify(): Observable<any> {
    return this.notify$.asObservable();
  }
  private notify$ = new BehaviorSubject<any>({});

  setLayout(options?: AppSettings): AppSettings {
    this.options = Object.assign(defaults, options);
    return this.options;
  }

  setNavState(type: string, value: boolean) {
    this.notify$.next({ type, value } as any);
  }

  getOptions(): AppSettings {
    return this.options;
  }

  /** User information */

  get user() {
    let user: IAppUser ;
    this.authService.getUser().subscribe(result => {
      if (result) {
        user = {
          id: result.sub,
          name: result.name
        }
      }
    });


    return user;
  }

  setUser(value: IAppUser ) {
    this.store.set(USER_KEY, value);
  }

  removeUser() {
    this.store.remove(USER_KEY);
  }

  /** System language */

  get language() {
    return this.options.language;
  }

  setLanguage(lang: string) {
    this.options.language = lang;
    this.notify$.next({ lang });
  }
}
