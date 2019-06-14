import { Injectable } from '@angular/core';
import { HttpRequest, HttpResponse, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { delay, mergeMap, materialize, dematerialize } from 'rxjs/operators';

@Injectable()
export class FakeBackendInterceptor implements HttpInterceptor {

    constructor() { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // return next.handle(request);
        // array in local storage for registered users
        let users: any[] = JSON.parse(localStorage.getItem('users')) || [];
        // wrap in delayed observable to simulate server api call
        return of(null).pipe(mergeMap(() => {
            // authenticate
            
            console.log(request);
            if (request.url.endsWith('/OAuth/Login') && request.method === 'POST') {
                // find if any user matches login credentials
                let body = {
                    "refresh": "MHSurqWhq6EB4hPGCamu39S7OFA+pPtlnKwoI1qhKQc=",
                    "access": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIm5iZiI6MTUzNzk2MzQ5MiwiZXhwIjoxNTM3OTYzNzkyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNzciLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjEwNzcifQ.DBhF7N5B5IJfJiyROFl4aQIxgHRLvM0j8Ob_29a21Ow",
                    "fullName": "Vugar Ahmadoglu",
                    "id":1
                }
                return of(new HttpResponse({ status: 200, body: body }));
                // let filteredUsers = users.filter(user => {
                //     return user.username === request.body.username && user.password === request.body.password;
                // });
                // console.log(filteredUsers);
                // if (filteredUsers.length) {
                //     // if login details are valid return 200 OK with user details and fake jwt token
                //     let body = {
                //         "refresh": "MHSurqWhq6EB4hPGCamu39S7OFA+pPtlnKwoI1qhKQc=",
                //         "access": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIm5iZiI6MTUzNzk2MzQ5MiwiZXhwIjoxNTM3OTYzNzkyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNzciLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjEwNzcifQ.DBhF7N5B5IJfJiyROFl4aQIxgHRLvM0j8Ob_29a21Ow",
                //         "fullName": "Vugar Ahmadoglu",
                //         "id":1
                //     }
                //     return of(new HttpResponse({ status: 200, body: body }));
                // } else {
                //     // else return 400 bad request
                //     // ded = new HttpErrorResponse({error:"Username or password is incorrect", status:404})
                //     return throwError(new HttpErrorResponse({ status: 401, error: "Username or password is incorrect!!!" }));
                //     // return throwError({ error: { message: 'Username or password is incorrect' } });
                // }
            }

        //     if (request.url.endsWith('/OAuth/LogOut') && request.method === 'POST') {
        //         console.log("LogOut");
        //         return of(new HttpResponse({ status: 200, body: {} }));
        //     }

        //     // authenticate refresh token
        //     if (request.url.endsWith('/users/tokenrefresh') && request.method === 'POST') {
        //         // find if any user matches login credentials
        //         let filteredUsers = request.body.access;
        //         console.log(filteredUsers);
        //         if (filteredUsers.length) {
        //             // if login details are valid return 200 OK with user details and fake jwt token
        //             let user = filteredUsers[0];
        //             let body = {
        //                 id: user.id,
        //                 username: user.username,
        //                 firstName: user.firstName,
        //                 lastName: user.lastName,
        //                 token: 'fake-jwt-token',
        //                 tokenrefresh: "fake-refresh-jwt-token"
        //             };

        //             return of(new HttpResponse({ status: 200, body: body }));
        //         } else {
        //             // else return 400 bad request
        //             return throwError({ error: { message: 'Username or password is incorrect' } });
        //         }
        //     }

        //     // get users
        //     if (request.url.endsWith('/users') && request.method === 'GET') {
        //         // check for fake auth token in header and return users if valid, this security is implemented server side in a real application
        //         if (request.headers.get('Authorization') === 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIm5iZiI6MTUzNzk2MzQ5MiwiZXhwIjoxNTM3OTYzNzkyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNzciLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjEwNzcifQ.DBhF7N5B5IJfJiyROFl4aQIxgHRLvM0j8Ob_29a21Ow') {
        //             return of(new HttpResponse({ status: 200, body: users }));
        //         } else {
        //             // return 401 not authorised if token is null or invalid
        //             return throwError(new HttpErrorResponse({ status: 401, error: "Unauthorised!", statusText: "Token does not mach!" }));
        //         }
        //     }

        //     // get user by id
        //     if (request.url.match(/\/users\/\d+$/) && request.method === 'GET') {
        //         if (request.headers.get('Authorization') === 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIm5iZiI6MTUzNzk2MzQ5MiwiZXhwIjoxNTM3OTYzNzkyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNzciLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjEwNzcifQ.DBhF7N5B5IJfJiyROFl4aQIxgHRLvM0j8Ob_29a21Ow') {
        //             let urlParts = request.url.split('/');
        //             let id = parseInt(urlParts[urlParts.length - 1]);
        //             let matchedUsers = users.filter(user => { return user.id === id; });
        //             let user = matchedUsers.length ? matchedUsers[0] : null;

        //             return of(new HttpResponse({ status: 200, body: user }));
        //         } else {
        //             // return 401 not authorised if token is null or invalid
        //             return throwError(new HttpErrorResponse({ status: 401, error: "Unauthorised!", statusText: "Token does not mach!" }));
        //         }
        //     }

        //     // register user
        //     if (request.url.endsWith('/api/OAuth/SignUp') && request.method === 'POST') {
        //         // get new user object from post body
        //         let newUser = request.body;
        //         console.log(newUser);
        //         // validation
        //         let duplicateUser = users.filter(user => { return user.username === newUser.username; }).length;
        //         if (duplicateUser) {
        //             // return throwError({ error: { message: 'Username "' + newUser.username + '" is already taken' } });
        //             return throwError(new HttpErrorResponse({ status: 409, error: 'Username "' + newUser.username + '" is already taken' }));
        //         }

        //         // save new user
        //         newUser.id = users.length + 1;
        //         users.push(newUser);
        //         localStorage.setItem('users', JSON.stringify(users));

        //         // respond 200 OK
        //         return of(new HttpResponse({ status: 204, body: "Registration successful 123 " }));
        //     }

        //     // user check
        //     if (request.url.endsWith('/users/userDuplicateCheck') && request.method === 'POST') {
        //         // username check duplicate
        //         console.log(request.body);
        //         // validation
        //         let duplicateUser = users.filter(user => { return user.username === request.body; }).length;
        //         console.log(duplicateUser);
        //         if (duplicateUser) {
        //             return of(new HttpResponse({ status: 200, body: true }));
        //         }
        //         return of(new HttpResponse({ status: 200, body: false }));
        //     }

        //     // delete user
        //     if (request.url.match(/\/users\/\d+$/) && request.method === 'DELETE') {
        //         // check for fake auth token in header and return user if valid, this security is implemented server side in a real application
        //         if (request.headers.get('Authorization') === 'Bearer fake-jwt-token') {
        //             // find user by id in users array
        //             let urlParts = request.url.split('/');
        //             let id = parseInt(urlParts[urlParts.length - 1]);
        //             for (let i = 0; i < users.length; i++) {
        //                 let user = users[i];
        //                 if (user.id === id) {
        //                     // delete user
        //                     users.splice(i, 1);
        //                     localStorage.setItem('users', JSON.stringify(users));
        //                     break;
        //                 }
        //             }

        //             // respond 200 OK
        //             // return throwError(new HttpErrorResponse({status:401, error:"Username or password is incorrect!!!"}));
        //             return of(new HttpResponse({ status: 200 }));
        //         } else {
        //             // return 401 not authorised if token is null or invalid
        //             return throwError(new HttpErrorResponse({ status: 401, error: "Unauthorised!" }));
        //         }
        //     }

        //     // recovery
        //     if (request.url.endsWith('/ForgotPassword') && request.method === 'POST') {
        //         console.log('request recovery');
        //         console.log(request);
        //         if (5 > 0) {
        //             let body = {
        //                 username: 'test user',
        //                 token: 'fake-jwt-token'
        //             };
        //             return of(new HttpResponse({ status: 200, body: body }));
        //         } else {
        //             return throwError({ error: { message: 'Username or password is incorrect' } });
        //         }
        //     }

            // pass through any requests not handled above
            return next.handle(request);

        }))

            // call materialize and dematerialize to ensure delay even if an error is thrown (https://github.com/Reactive-Extensions/RxJS/issues/648)
            .pipe(materialize())
            .pipe(delay(500))
            .pipe(dematerialize());
    }
}

export let fakeBackendProvider = {
    // use fake backend in place of Http service for backend-less development
    provide: HTTP_INTERCEPTORS,
    useClass: FakeBackendInterceptor,
    multi: true
};
