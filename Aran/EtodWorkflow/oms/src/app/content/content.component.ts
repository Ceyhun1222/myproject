
import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { Router, ActivatedRoute } from '@angular/router';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';
import { UserService } from 'projects/omslib/src/lib/_services/user.service';

@Component({
    selector: 'app-content',
    templateUrl: './content.component.html',
    styleUrls: ['./content.component.css']
})

export class ContentComponent implements OnInit {
    users: any[] = [];
    status = 'All';
    loading:boolean = false;

    constructor(private userService: UserService,
        private router: Router,
        private publicService: PublicService,
        private activatedRoute: ActivatedRoute) {

    }

    ngOnInit() {
        console.log('------ User list Component Init ----')
        this.activatedRoute.params.subscribe(data => {
            console.log(data);
            if(data && data.status){
                this.status = data.status;
                this.loadAllUsers();
            }
        })
        this.loadAllUsers();
    }

    getUser(id: number) {
        this.userService.get(id).pipe(first()).subscribe((data) => {
            // this.loadAllUsers()
            console.log(data);
        });
    }

    deleteUser(id: number) {
        // var result = confirm("Are you sure?");
        // if (result) {
            this.userService.delete(id).pipe(first()).subscribe(() => {
                this.publicService.alertMsg("Removed", 1500);
                this.loadAllUsers();
            });
        // }
    }

    editUser(id: any) {
        this.router.navigate(["user", { id: id }]);
    }

    loadAllUsers() {
        this.loading = true;
        this.userService.getUserList("?status=" + this.status).pipe(first()).subscribe(users => {
            this.loading = false;
            this.users = users;
        });
    }

    confirmStatus(user, value) {
        if(user.id > 0){
            this.userService.confirmStatus(user.id, value).pipe(first()).subscribe(data => {
                // console.log(data);
                // this.loadAllUsers();
                this.router.navigate(["users/All"]);
            });
        }
    }
    ngAfterViewInit() {
        //     $('.sidebar-main-toggle').on('click', function (e) {
        //       e.preventDefault();

        //       document.body.classList.toggle('sidebar-xs').classList.remove('sidebar-mobile-main');
        //       $('.sidebar-main').find('.nav-sidebar').children('.nav-item-submenu').hover(function() {
        //         var totalHeight = 0,
        //             $this = $(this),
        //             navSubmenuClass = 'nav-group-sub',
        //             navSubmenuReversedClass = 'nav-item-submenu-reversed';

        //         totalHeight += $this.find('.' + navSubmenuClass).filter(':visible').outerHeight();
        //         if($this.children('.' + navSubmenuClass).length) {
        //             if(($this.children('.' + navSubmenuClass).offset().top + $this.find('.' + navSubmenuClass).filter(':visible').outerHeight()) > document.body.clientHeight) {
        //                 $this.addClass(navSubmenuReversedClass)
        //             }
        //             else {
        //                 $this.classList.remove(navSubmenuReversedClass)
        //             }
        //         }
        //     });
        //   });
    }
}