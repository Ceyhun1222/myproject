import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../_services';
import { first, catchError } from 'rxjs/operators';
import { UserFormComponent } from '../user-form/user-form.component';
import { _config } from '../../../projects/omslib/src/lib/_config/_config';
import { PublicService } from '../_services/public/public.service';

@Component({
  selector: 'app-user-view',
  templateUrl: './user-view.component.html',
  styleUrls: ['./user-view.component.css']
})
export class UserViewComponent implements OnInit {
  action = "signup";//create, edit
  userData: any;
  requestFailed: boolean = false;
  accountCreated: boolean = false;
  @ViewChild(UserFormComponent) child: UserFormComponent;

  constructor(private userService: UserService, private activatedRoute: ActivatedRoute, private router: Router, private publicService: PublicService) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      if (params != undefined && params.action != undefined) {
        this.action = params.action;
        if (params.id > 0) {
          this.userService.get(params.id).pipe(first()).subscribe((response) => {
            this.userData = response;
            this.child.setUser(response);
          });
        }
      }
    });
  }

  save(user) {
    console.log(user);
    let url = this.action == 'signup' ? _config.links.signup : _config.links.user;
    this.userService.save(user, url)
      .pipe(first())
      .subscribe(
        data => {
          this.child.loadingSet(false);
          if (this.action === 'signup') {
            this.accountCreated = true;
          } else {
            this.publicService.alertMsg("Successfully saved", 1500);
            this.router.navigate(['/users/All']);
          }
        },
        error => {
          this.child.loadingSet(false);
          if (error.status === 409) {
            this.child.duplicateUserSet(user.userName);
          } else {
            this.requestFailed = true;
          }
        })
  }
}