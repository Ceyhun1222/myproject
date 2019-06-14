
import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

// import { User } from '../_models';
// import { UserService } from '../_services';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';
import { RequestService } from 'projects/omslib/src/lib/_services/request.service';
import { ProjService } from 'projects/omslib/src/lib/_services/map/proj.service';
import { RequestsGrid } from './requests-grid';
import { GridSorting, GridSortingMode, RequestFilter } from 'projects/omslib/src/lib/grid/grid';
import { Router } from '@angular/router';
import { UserService } from 'projects/omslib/src/lib/_services/user.service';
// import {} from 'sweetalert2'
// import { } from 'sweetalert2';

@Component({
    selector: 'app-requests',
    templateUrl: './requests.component.html',
    styleUrls: ['./requests.component.css']
})

export class RequestsComponent implements
    OnInit {
    requestList: any[] = [];
    status = 'All';
    isLoaded: boolean = false;
    grid: RequestsGrid;
    filter: RequestFilter = new RequestFilter();

    airportList: any;
    userList: any[];


    constructor(private requestService: RequestService,
        public publicService: PublicService,
        public projService: ProjService, private userService: UserService,
        private router: Router) {

    }

    ngOnInit() {
        this.loadData();
        this.getAirportList();
        this.getUserList();
        // Grid initization
        this.grid = new RequestsGrid();
        this.grid.config.pagination.page = 1;
        this.grid.config.pagination.perPage = 10;
    }

    // submit2Aixm(obj) {
    //     if(obj.submitted2Aixm){
    //         this.publicService.alertMsg("This request already submitted!", 1500);
    //     } else {
    //         this.requestService.submit2Aixm(obj.id).pipe(first()).subscribe((response) => {
    //             console.log(response);
    //             this.publicService.alertMsg("Submitted", 1500);
    //             this.loadData();
    //         });
    //     }
    // }

    deleteRequest(id: number) {
        console.log(id);
        if (confirm('Are you sure you want to delete it?')) {
            this.requestService.deleteRequest(id).pipe(first()).subscribe(() => {
                this.publicService.alertMsg("Removed", 1500);
                this.loadData();
            });
        } 
    }

    loadData() {
        console.log(this.filter);
        this.isLoaded = false;
        this.requestService.getRequestList(false, this.filter).subscribe(response => {
            this.isLoaded = true;
            console.log(response);
            this.requestList = [];
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
            case 'submit2Aixm':
                // this.submit2Aixm(pureObject);
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
                        name: 'Aerodrome'
                    })

                },
                error => {
                    console.log(error);
                });
    }

    getUserList() {
        this.userService.getUserListBasic().pipe(first()).subscribe(users => {
            this.userList = users;

            this.userList.forEach(elem => {
                elem.alias = elem.id;
                elem.name = elem.fullname;
            });
            this.userList.unshift({
                alias: "User name",
                name: 'User name'
            })
            console.log(this.userList);
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

    callbackChangeUser(user) {
        console.log(user);
        this.filter.userId = null;
        if (user.id) {
            this.filter.userId = user.id;
        }
        this.loadData();
    }
}