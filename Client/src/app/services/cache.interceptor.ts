import { HttpCacheService } from './http-cache.service';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpEvent, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class CacheInterceptor implements HttpInterceptor {
  constructor(private cacheService: HttpCacheService) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // pass along non cacheable requests and invalidates cache
    if (req.method !== 'GET') {
      console.log(`Invalidate cache ${req.method} ${req.url}`);
      this.cacheService.invalidate();
      return next.handle(req);
    }
    // attempt to retrieve a cached response
    const cachedResponse: HttpResponse<any>
     = this.cacheService.get(req.url);
    // return cached response
    if (cachedResponse) {
      console.log(`Returning CachedResponse from ${cachedResponse}`);
      console.log(`CachedResponse ${cachedResponse.url}`);
      return of(cachedResponse);
    }
    // send request to server and add response to cache
    return next.handle(req)
    .pipe(
      tap((event: HttpResponse<any>) => {
        if (event instanceof HttpResponse) {
          console.log( `Adding item to cache ${req.url}`);
          this.cacheService.put(req.url, event);
        }
      })
    );
  }
}
