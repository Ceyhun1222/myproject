import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl, FormArray } from '@angular/forms';
import { first, catchError } from 'rxjs/operators';

import { UserService, AirportService } from '../_services';
import { _config } from '../../../projects/omslib/src/lib/_config/_config';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})

export class UserFormComponent implements OnInit {
  registerForm: FormGroup;
  duplicateUser: string = "";
  duplicateEmail: string = ""
  passwordMatchError: any = false;
  passwordShow: boolean = false;
  confirmPasswordShow: boolean = false;
  codeFilter: any = null;
  acceptedUserData: any;
  airportList: any;

  loading = false;
  submitted = false;
  action = 'signup';
  successed: boolean = false;
  requestFailed: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private publicService: PublicService,
    private airportService: AirportService) {
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      console.log(params);
      if (params != undefined && params.action != undefined) {
        this.action = params.action;
        if (params.id > 0) {
          this.userService.get(params.id).pipe(first()).subscribe((response) => {
            this.setUser(response);
          });
        }
      }
    });

    this.registerForm = this.fb.group({
      id: [0],
      userName: ['', [Validators.required, Validators.minLength(5)]],
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      phoneNumber: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(12)]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20), Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$')]],
      confirmPassword: ['', [Validators.required]],
      status: ['', [Validators.required]],
      company: this.fb.group({
        name: ['', [Validators.required]],
        airportId: ['', [Validators.required, Validators.minLength(1)]],
        airportName: ['', [Validators.required]],
        address: ['', [Validators.required]],
        zip: ['', [Validators.required]],
        telephone: ['', [Validators.required]],
        fax: ['', [Validators.required]]
      }),
    });
    var fakeUser = {
      id: [0],
      userName: 'vugarDadalov',
      email: 'email@user.com',
      firstName: 'Vugar',
      lastName: 'Dadalov',
      phoneNumber: '70 831 76 35',
      password: 'risk@123A',
      confirmPassword: 'risk@123A',
      status: 'Pending',
      company: {
        name: 'R.I.S.K.',
        airportName: "NAKOTNE BALTIJAS HELIKOPTERS",
        airportId: "6f4b5928-b1cf-4987-8e6b-fff9dde078b3",
        address: 'Address',
        zip: 'Zip',
        telephone: '12 831 76 35',
        fax: '12 831 76 35',
      }
    }
    this.setUser(fakeUser);
    this.getAirportList();
  }

  ngAfterViewInit() {
    var input = document.querySelector('#user-phone-number-pattern');
    input.addEventListener('input', function () {
      this.value = this.value.replace(/[^0-9 \,]/, '');
    });
  }

  get f() { return this.registerForm.controls; }
  get f2() { return this.registerForm.get('company')['controls'] }

  setUser(user: any) {
    console.log(user);
    this.submitted = true;
    this.acceptedUserData = Object.assign({}, user);
    this.registerForm.patchValue(user);
  }

  airportOnChange(val) {
    let _a = this.airportList.find(x => x.identifier === val);
    if (_a && _a.name) {
      this.registerForm.patchValue({ company: { airportName: _a.name } });
    }
  }

  loadingSet(val: boolean) {
    this.loading = val;
  }
  duplicateUserSet(val: string) {
    this.duplicateUser = val;
  }

  formatPhoneNumber = function (e: any = null) {
    var keyCodes = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 8];//only number key code
    if (e != undefined && keyCodes.includes(e.keyCode)) {
      var number = this.f.phoneNumber.value.replace(/\s/g, '');
      var number_ = number.slice(0, 2);
      number_ += " " + number.slice(2, 5);
      number_ += " " + number.slice(5, 7);
      number_ += " " + number.slice(7, 9);
      this.f.phoneNumber.setValue(number_.trim());
    }
  }
  getAirportList() {
    console.log('getAirportList');
    this.airportService.getAirportData()
      .subscribe(
        data => {
          this.airportList = data;
          console.log(data)
        },
        error => {
          console.log(error);
        });
  }
  userDuplicateCheck() {
    if (this.acceptedUserData && this.acceptedUserData.id > 0) return;
    this.userService.userDuplicateCheck(this.f.userName.value)
      .pipe(first())
      .subscribe(
        data => {
          this.duplicateUser = "";
        },
        error => {
          if (error != undefined && error.status === 409) {
            this.duplicateUser = this.f.userName.value;
          }
        });
  }
  emailDuplicateCheck() {
    this.userService.emailDuplicateCheck(this.f.email.value)
      .pipe(first())
      .subscribe(
        data => {
          this.duplicateEmail = "";
        },
        error => {
          if (this.acceptedUserData) {
            if (this.acceptedUserData.email != this.f.email.value) {
              this.duplicateEmail = this.f.email.value;
            }
          } else {
            this.duplicateEmail = this.f.email.value;
          }
        });
  }
  passwordMatchValidator() {
    this.passwordMatchError = this.f.password.value !== this.f.confirmPassword.value;
  }

  onSubmit() {
    // console.log(this.registerForm.value);
    this.submitted = true;
    if (this.f.userName.invalid || this.f.email.invalid || this.duplicateEmail === this.f.email.value || this.f.firstName.invalid || this.f.lastName.invalid || this.f.phoneNumber.invalid) {
      console.log("catch 1");
      return;
    }

    if (!this.acceptedUserData && (this.duplicateUser === this.f.userName.value || this.f.password.invalid || this.passwordMatchError)) {
      console.log("catch 2");
      return;
    }

    if (this.f2.name.invalid || this.f2.airportId.invalid || this.f2.address.invalid || this.f2.zip.invalid || this.f2.telephone.invalid || this.f2.fax.invalid) {
      console.log("catch 3");
      return;
    }

    console.log("Request Sent");
    this.loading = true;
    let url = this.action == 'signup' ? _config.links.signup : _config.links.user;
    this.userService.save(this.registerForm.value, url)
      .pipe(first())
      .subscribe(
        data => {
          this.loading = false;
          if (this.action === 'signup') {
            this.successed = true;
          } else {
            this.publicService.alertMsg("Successfully saved", 1500);
            this.router.navigate(['/users/All']);
          }
        },
        error => {
          this.loading = false;
          if (error.status === 409) {
            this.duplicateUserSet(this.registerForm.value.userName);
            this.duplicateUser = this.registerForm.value.userName;
          } else {
            this.requestFailed = true;
          }
        })
  }
}