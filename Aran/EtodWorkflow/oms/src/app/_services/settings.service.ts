import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { _config } from '../../../projects/omslib/src/lib/_config/_config';


@Injectable()
export class SettingsService {
    constructor(private http: HttpClient) { }

    getSelectedSlot() {
        return this.http.get<any[]>(_config.links.slotSelected );
    }

    getSlotList() {
        return this.http.get<any[]>(_config.links.slotList );
    }

    slotSave(data) {
        console.log(data);
        return this.http.post(_config.links.slotSave, data);
    }

    getNotificationCount() {
        return this.http.get(_config.links.userNotificationCount );
    }
}