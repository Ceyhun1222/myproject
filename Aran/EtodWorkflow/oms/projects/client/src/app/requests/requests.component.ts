
import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

// import { User } from '../_models';
// import { UserService } from '../_services';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';
import { RequestService } from 'projects/omslib/src/lib/_services/request.service';
import { ProjService } from 'projects/omslib/src/lib/_services/map/proj.service';
import { RequestFilter, GridSorting, GridSortingMode } from 'projects/omslib/src/lib/grid/grid';
import { RequestsGrid } from './requests-grid';
import { LangService } from 'projects/omslib/src/lib/_services/lang/langService';
import { Router } from '@angular/router';

@Component({
    selector: 'app-requests',
    templateUrl: './requests.component.html',
    styleUrls: ['./requests.component.css']
})

export class RequestsComponent implements OnInit {
    requestList: any[] = [];
    status = 'All';
    // loading:boolean = false;

    isLoaded: boolean = false;
    grid: RequestsGrid;
    filter: RequestFilter = new RequestFilter();

    airportList: any;

    constructor(private requestService: RequestService, private publicService: PublicService, public projService: ProjService, public lang: LangService,private router: Router) { }

    ngOnInit() {
        this.loadData();

        this.getAirportList();
        // this.getUserList();
        // Grid initization
        this.grid = new RequestsGrid(this.lang);
        this.grid.config.pagination.page = 1;
        this.grid.config.pagination.perPage = 10;
    }

    deleteRequest(id: number) {
        console.log(id);
        if (confirm(this.lang.get('Are you sure?'))) {
            this.requestService.deleteRequest(id).pipe(first()).subscribe(() => {
                this.publicService.alertMsg(this.lang.get('Successfully removed'), 3000);
                this.loadData();
            });
        } 
    }

    submitRequest(request) {
        if(request.submitted){
            this.publicService.alertMsg(this.lang.get("This request already submitted!"), 3000, "info");
        } else {
            this.requestService.submitRequest(request.id).pipe(first()).subscribe(data => {
                console.log(data);
                this.publicService.alertMsg("Submitted", 3000);
                this.loadData();
            });
        }
    }

    loadData() {
        this.isLoaded = false;
        this.requestList = [];
        this.requestService.getRequestList(true, this.filter).pipe(first()).subscribe(response => {
            this.isLoaded = true;
            if (response && response['data'] && response['count']) {
                this.requestList = response['data'];
            }
            this.grid.convert(this.requestList, response['count']);
        });
    }

    doFilter() {
        this.filter.page = this.grid.config.pagination.page = 1;
        this.loadData();
    }
    callbackGridSort(sorting: GridSorting) {
        console.log(sorting);
        this.filter.sortMode = (sorting.mode == GridSortingMode.ASC ? 'ASC' : 'DESC');
        this.filter.sortField = sorting.column;
        this.loadData();
    }

    callbackGridPage(page: number) {
        this.filter.page = page;
        this.loadData();
    }

    callbackGridPerPage(perPage: number) {
        // this.filter.page = this.grid.config.pagination.page = 1;
        this.filter.perPage = perPage;
        this.loadData();
    }
    callbackKeyPress(event) {
        this.filter.query = event.target.value;
        // if(event.keyCode === 13){
        //     this.doFilter();
        // }
        this.doFilter();
    }

    callbackAction(action: any) {
        console.log(action)
        let pureObject = action.row.pureObject;
        switch (action.alias) {
            case 'view':
                this.router.navigate(['/request/' + pureObject.id]);
                break;
            case 'submit':
                this.submitRequest(pureObject);
                break;
            case 'delete':
                this.deleteRequest(pureObject.id);
                break;
        }
    }

    getAirportList() {
        this.requestService.getAirportData()
            .subscribe(
                data => {
                    console.log(data);
                    this.airportList = data;
                    // name
                    this.publicService.sortByKey(this.airportList, 'name');

                    this.airportList.forEach(elem => {
                        elem.alias = elem.identifier;
                        // elem.name = elem.;
                    });
                    this.airportList.unshift({
                        alias: "Aerodrome",
                        name: this.lang.get('Airport')
                    })

                },
                error => {
                    console.log(error);
                });
    }

    callbackChangeAirport(aerodrom) {
        console.log(aerodrom);
        this.filter.aerodromeId = null;
        if (aerodrom.identifier) {
            this.filter.aerodromeId = aerodrom.identifier;
        }
        this.loadData();

    }



}