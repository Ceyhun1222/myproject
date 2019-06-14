import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { _config } from '../_config/_config';

@Injectable({
    providedIn:'root'
})
export class UserService {
    constructor(private http: HttpClient) { }

    userDuplicateCheck(username: String) {
        return this.http.get(_config.links.checkUserName + username);
    }

    emailDuplicateCheck(email: String) {
        return this.http.get(_config.links.checkEmail + email);
    }

    save(data, url) {
        if(data && data.id > 0){
            return this.http.put(url, data);
        }
        return this.http.post(url, data);
    }

    get(id: number) {
        return this.http.get(_config.links.user + `/` + id);
    }

    delete(id: number) {
        return this.http.delete(_config.links.user + `/` + id);
    }

    getUserList(params) {
        // console.log(params);
        return this.http.get<any[]>(_config.links.user + params );
    }
    getUserListBasic() {
        return this.http.get<any[]>(_config.links.userBasicInfo );
    }

    confirmStatus(id, value) {
        return this.http.put<any[]>(_config.links.user + '/' + id + '/' +value, {});
    }

    getMyProfile() {
        return this.http.get(_config.links.profile);
    }
    
    getNotificationCount() {
        return this.http.get(_config.links.requestNotificationCount);
    }

    resetPassword(data) {
        console.log(data);
        return this.http.post<any>(_config.links.resetPassword, data);
    }

    forgotPassword(email: string) {
        return this.http.post<any>(_config.links.forgotPassword, { email: email });
    }
}