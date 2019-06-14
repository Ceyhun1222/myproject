import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let user: any = JSON.parse(localStorage.getItem('user')) || JSON.parse(sessionStorage.getItem('user'));
        if ( user && user.access && user.access.length > 0 ) {
            request = request.clone({
                setHeaders: { 
                    Authorization: `Bearer ${user.access}`
                }
            });
        }
        // console.log(request);
        return next.handle(request);
    }
}