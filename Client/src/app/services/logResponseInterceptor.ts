import { Observable } from 'rxjs';
import { HttpInterceptor, HttpRequest, HttpEvent, HttpHandler, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/operators';

@Injectable()
export class LogResponseInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // console.log(`LogResInt ${req.url}`);
    return next.handle(req)
    .pipe(
      tap(event => {
        if (event.type === HttpEventType.Response) {
          // console.log(event.body);
        }
      })
    );
  }
}
