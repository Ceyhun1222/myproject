import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { UserService } from '../_services/user.service';
import { _config } from '../_config/_config';
import { PublicService } from '../_services/public.service';
import { LangService } from '../_services/lang/langService';
import { RequestService } from '../_services/request.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})

export class UserFormComponent implements OnInit {
  registerForm: any;
  phoneConfig = _config.phone;

  duplicateUser: boolean = false;
  duplicateEmail: boolean = false;
  passwordMatchError: boolean = false;

  passwordShow: boolean = false;
  confirmPasswordShow: boolean = false;

  airportList: any;

  loading = false;
  submitted = false;
  action = 'signup'; //user
  successed: boolean = false;
  requestFailed: boolean = false;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private requestService: RequestService,
    private publicService: PublicService,
    public lang:LangService,
    private location:Location) {
  }

  ngOnInit() {
    if (this.router.routerState.snapshot.url.includes("user")) {
      this.action = "user";
    }

    if (this.router.routerState.snapshot.url.includes("myprofile")) {
      this.action = "myprofile";
    }

    if (this.action == "myprofile") {
      this.userService.getMyProfile().pipe(first()).subscribe((response) => {
        console.log(response);
        this.submitted = true;
        this.registerForm = response;
      });
    }

    this.activatedRoute.params.subscribe(params => {
      if (params && params.id > 0 && this.action == "user") {
        this.userService.get(params.id).pipe(first()).subscribe((response) => {
          this.submitted = true;
          this.registerForm = response;
        });
      }
    });

    this.registerForm = {
      id: 0,
      userName: '',
      email: '',
      firstName: '',
      lastName: '',
      phoneNumber: '',
      password: '',
      confirmPassword: '',
      company: {
        name: '',
        airportName: "",
        airportId: "",
        address: '',
        zip: '',
        telephone: '',
        fax: '',
      }
    }
    this.getAirportList();
  }

  airportOnChange(val) {
    let airport = this.airportList.find(x => x.identifier === val);
    if (airport) {
      this.registerForm.company.airportName = airport.name;
    }
  }
  getAirportList() {
    console.log('getAirportList');
    this.requestService.getAirportData()
      .subscribe(
        data => {
          this.airportList = data;
          console.log(data)
        },
        error => {
          console.log(error);
        });
  }
  userDuplicateCheck(value) {
    console.log(value)
    if (this.registerForm.id > 0) return;
    this.userService.userDuplicateCheck(value)
      .pipe(first())
      .subscribe(
        data => {
          this.duplicateUser = false;
        },
        error => {
          if (error != undefined && error.status === 409) {
            this.duplicateUser = true;
          }
        });
  }
  emailDuplicateCheck(value) {
    if (this.registerForm.id > 0) return;
    this.userService.emailDuplicateCheck(value)
      .pipe(first())
      .subscribe(
        data => {
          this.duplicateEmail = false;
        },
        error => {
          this.duplicateEmail = true;
        });
  }
  passwordMatchValidator() {
    this.passwordMatchError = this.registerForm.password !== this.registerForm.confirmPassword;
  }

  onSubmit(form): void {
    this.submitted = true;
    console.log(form.invalid, this.duplicateUser, this.duplicateEmail, this.passwordMatchError);
    if (form.invalid || this.duplicateUser || this.duplicateEmail || this.passwordMatchError) {
      return;
    }
    this.loading = true;
    let url = this.action == 'signup' ? _config.links.signup : _config.links.user;
    this.userService.save(this.registerForm, url)
      .pipe(first())
      .subscribe(
        data => {
          this.loading = false;
          if (this.action === 'signup') {
            this.successed = true;
          } else {
            this.publicService.alertMsg("Successfully saved", 1500);
            this.goBack();
          }
        },
        error => {
          this.loading = false;
          if (error.status === 409) {
            this.duplicateUser = true;
          } else {
            this.requestFailed = true;
          }
        })
  }

  navigateTo(path) {
    this.router.navigate([path]);
  }

  goBack(){
    this.location.back();
  }
}