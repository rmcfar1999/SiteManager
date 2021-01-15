import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpResponse, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthorizeService } from './authorize.service';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeInterceptor implements HttpInterceptor {
  constructor(
    private authorize: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var requestIntercept = this.authorize.getAccessToken()
      .pipe(mergeMap(token => this.processRequestWithToken(token, req, next)))
      .pipe(tap(() => { },
        (err: any) => {
          if (err instanceof HttpErrorResponse) {
            if (![401, 403].includes(err.status)) {
              return;
            }
            this.authorize.redirectToPermissionDenied();
            //this.router.navigateByUrl("/sessions/403");
          }
        }));

    return requestIntercept;

  
    ///////Handle api level auth denials...session timeout
    //return next.handle(req).pipe(tap(() => { },
    //  (err: any) => {
    //    if (err instanceof HttpErrorResponse) {
    //      if (![401, 403].includes(err.status)) {
    //        return;
    //      }
    //      //this.authorize.redirectToPermissionDenied();
    //      this.router.navigateByUrl("/sessions/403");
    //    }
    //  }));
  }

  // Checks if there is an access_token available in the authorize service
  // and adds it to the request in case it's targeted at the same origin as the
  // single page application.
  private processRequestWithToken(token: string, req: HttpRequest<any>, next: HttpHandler) {
    if (!!token && this.isSameOriginUrl(req)) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req);
  }

  private isSameOriginUrl(req: any) {
    // It's an absolute url with the same origin.
    if (req.url.startsWith(`${window.location.origin}/`)) {
      return true;
    }

    // It's a protocol relative url with the same origin.
    // For example: //www.example.com/api/Products
    if (req.url.startsWith(`//${window.location.host}/`)) {
      return true;
    }

    // It's a relative url like /api/Products
    if (/^\/[^\/].*/.test(req.url)) {
      return true;
    }

    // It's an absolute or protocol relative url that
    // doesn't have the same origin.
    return false;
  }
}
