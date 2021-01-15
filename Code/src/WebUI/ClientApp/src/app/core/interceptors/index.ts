import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { NoopInterceptor } from './noop-interceptor';
import { AuthorizeInterceptor } from '../authentication/authorize.interceptor';
import { LoggingInterceptor } from './logging-interceptor';

/** Http interceptor providers in outside-in order */
export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: NoopInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: LoggingInterceptor, multi: true },
];
