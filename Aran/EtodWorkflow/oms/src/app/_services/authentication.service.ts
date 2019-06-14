import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { _config } from '../../../projects/omslib/src/lib/_config/_config';

@Injectable()
export class AuthenticationService {
    constructor(private http: HttpClient) { }

    login(user: any) {
        return this.http.post<any>(_config.links.login, user)
            .pipe(map(userData => {
                // console.log(userData);
                if (userData !== undefined && userData.access !== undefined && userData.refresh !== undefined) {
                    localStorage.setItem('remember', user.remember);
                    sessionStorage.setItem('user', JSON.stringify(userData));
                    if (user.remember) {
                        localStorage.setItem('user', JSON.stringify(userData));
                    }
                }
                return userData;
            }));
    }

    logout() {
        this.http.post<any>(_config.links.logout, {})
            .pipe(map(response => {
                return response;
            })).pipe()
            .subscribe(
                data => {
                    console.log(data);
                },
                error => {
                    console.log(error);
                });
        localStorage.removeItem('user');
        sessionStorage.removeItem('user');
        location.reload();
    }

    refresh() {
        console.log('refresh');
        var user = JSON.parse(localStorage.getItem('user'));

        return this.http.post<any>(_config.links.refresh, { 'access': user.access, 'refresh': user.refresh })
            .pipe(
                map(response => {
                    console.log(response);
                    if (response !== undefined && response.access !== undefined && response.refresh !== undefined) {
                        user.access = response.access;
                        user.refresh = response.refresh;
                        if (localStorage.getItem('remember')) {
                            localStorage.setItem('user', JSON.stringify(user));
                        } else {
                            sessionStorage.setItem('user', user);
                        }
                    }
                }));
    }
}