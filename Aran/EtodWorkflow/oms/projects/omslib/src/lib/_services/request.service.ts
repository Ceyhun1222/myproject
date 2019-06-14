import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { _config } from '../_config/_config';
import { RequestFilter } from '../grid/grid';


@Injectable()
export class RequestService {
    constructor(private http: HttpClient) { }

    getRequestList(isMine: boolean = false, filter: RequestFilter = null) {
        console.log(filter);
        var reqUrl = isMine ? _config.links.requestMine : _config.links.request;
        if(filter){
            reqUrl += "?page=" + filter.page + "&perPage=" + filter.perPage;
            if (filter.sortField && filter.sortField.length > 0 && filter.sortMode) {
                reqUrl += "&sortBy=" + filter.sortField + "&order=" + filter.sortMode;
            }

            if (filter.aerodromeId && filter.aerodromeId.length > 0) {
                reqUrl += "&aerodromeId=" + filter.aerodromeId;
            }

            if (filter.userId && filter.userId > 0) {

                console.log(filter.userId);
                reqUrl += "&userId=" + filter.userId;
            }
            
            if(filter.query && filter.query.length > 0){
                
                reqUrl += "&designator=" + filter.query;
            }
        }
        console.log(reqUrl);
        return this.http.get<any[]>(reqUrl);
    }
    getRequest(id: number, isMine: boolean = false) {
        var reqUrl = isMine ? _config.links.requestMine : _config.links.request;
        return this.http.get(reqUrl + `/` + id);
    }
    deleteRequest(id: number) {
        return this.http.delete(_config.links.request + `/` + id);
    }
    saveRequest(data) {
        if (data && data.id > 0) {
            return this.http.put(_config.links.request, data);
        }
        return this.http.post(_config.links.request, data);
    }
    submitRequest(id) {
        return this.http.post<any[]>(_config.links.requestSubmit + id, { id: id });
    }

    getReportTree(id) {
        return this.http.get(_config.links.reportTree + id);
    }
    getReportData(id) {
        return this.http.get(_config.links.reportData + id);
    }

    checkRequest(id) {
        return this.http.post(_config.links.requestCheck + id, {});
    }

    verticalStructuresDto() {
        return this.http.get(_config.links.verticalStructuresDto);
    }
    submit2Aixm(id) {
        return this.http.post(_config.links.submit2Aixm + id, {});
    }

    getAttachment(id: number) {
        return this.http.get(_config.links.getAttachment + id);
    }
    downloadReport(id: number) {
        return this.http.get(_config.links.downloadReport + id, {responseType:'blob'});
    }

    getAirportData() {
        return this.http.get(_config.links.airports);
    }
}