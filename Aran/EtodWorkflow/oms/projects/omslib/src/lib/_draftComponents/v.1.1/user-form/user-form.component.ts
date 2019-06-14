// import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

// import { Router, ActivatedRoute } from '@angular/router';
// import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl } from '@angular/forms';
// import { first, catchError } from 'rxjs/operators';

// import { UserService } from '../_services';
// import { PublicService } from '../_services/public/public.service';

// @Component({
//   selector: 'app-user-form',
//   templateUrl: './user-form.component.html',
//   styleUrls: ['./user-form.component.css']
// })

// export class UserFormComponent implements OnInit {
//   registerForm: FormGroup;
//   loading = false;
//   submitted = false;
//   duplicateUser: string = "";
//   duplicateEmail: string = ""
//   passwordNotSame: any = false;
//   passwordShow: boolean = false;
//   confirmPasswordShow: boolean = false;
//   accountCreated: boolean = false;

//   days: any = [];
//   months: any = [];
//   years: any = [];
//   codeFilter: any = null;
//   userDetail: any;

//   @Input() actionName: any;//user, signup
//   @Output() saveClicked = new EventEmitter<any>();

//   constructor(
//     private formBuilder: FormBuilder,
//     private router: Router,
//     private route: ActivatedRoute,
//     private userService: UserService,
//     private publicService: PublicService) {
//   }

//   ngOnInit() {
//     this.registerForm = this.formBuilder.group({
//       id: [0],
//       userName: ['dfdeww', [Validators.required, Validators.minLength(5)]],
//       email: ['fewfwe@ferf', [Validators.required, Validators.email]],
//       firstName: ['gfwefw', [Validators.required, Validators.minLength(2)]],
//       lastName: ['fwefwe', [Validators.required, Validators.minLength(2)]],
//       phoneNumber: ['70 831 76 35', [Validators.required, Validators.minLength(8), Validators.maxLength(12)]],
//       birthday: ['', [Validators.required, Validators.minLength(8)]],
//       month: ['11', [Validators.required, Validators.minLength(0)]],
//       year: ['2000', [Validators.required, Validators.minLength(4)]],
//       day: ['11', [Validators.required, Validators.minLength(1)]],
//       password: ['w111111', [Validators.required, Validators.minLength(6)]],
//       confirmPassword: ['w111111', [Validators.required]],
//       status: ['Pending', [Validators.required]],
//       gender: ['Male', [Validators.required]]
//     });

//     this.years = this.publicService.getYearList();
//     this.months = this.publicService.months;
//     this.loadDays();
//   }
//   ngAfterViewInit() {
//     var input = document.querySelector('#user-phone-number-pattern');
//     input.addEventListener('input', function () {
//       this.value = this.value.replace(/[^0-9 \,]/, '');
//     });
//   }

//   get f() { return this.registerForm.controls; }

//   setUser(user: any) {
//     console.log(user)
//     this.submitted = true;
//     this.userDetail = Object.assign({}, user);
//     this.f.id.setValue(user.id);
//     this.f.userName.setValue(user.userName);
//     this.f.firstName.setValue(user.firstName);
//     this.f.lastName.setValue(user.lastName);
//     this.f.password.setValue(user.password);
//     this.f.confirmPassword.setValue(user.confirmPassword);
//     this.f.birthday.setValue(user.birthday);
//     this.f.email.setValue(user.email);
//     this.f.phoneNumber.setValue(user.phoneNumber);
//     this.f.gender.setValue(user.gender);
//     this.f.status.setValue(user.status);//todo
//     if (user.birthday != undefined) {
//       var d = new Date(user.birthday);
//       this.f.year.setValue(d.getUTCFullYear().toString());
//       this.f.month.setValue((d.getUTCMonth()+1).toString());
//       this.f.day.setValue(d.getUTCDate().toString());
//     }
//   }
//   loadingSet(val: boolean) {
//     this.loading = val;
//   }
//   duplicateUserSet(val: string) {
//     this.duplicateUser = val;
//   }

//   loadDays() {
//     this.days = this.publicService.getDayListOfMonth(this.f.year.value, this.f.month.value);
//     // console.log(this.days);
//   }

//   formatPhoneNumber = function (e: any = null) {
//     var keyCodes = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 8];//only number key code
//     if (e != undefined && keyCodes.includes(e.keyCode)) {
//       var number = this.f.phoneNumber.value.replace(/\s/g, '');
//       var number_ = number.slice(0, 2);
//       number_ += " " + number.slice(2, 5);
//       number_ += " " + number.slice(5, 7);
//       number_ += " " + number.slice(7, 9);
//       this.f.phoneNumber.setValue(number_.trim());
//     }
//   }
//   userDuplicateCheck() {
//     if (this.userDetail && this.userDetail.id > 0) return;
//     this.userService.userDuplicateCheck(this.f.userName.value)
//       .pipe(first())
//       .subscribe(
//         data => {
//           this.duplicateUser = "";
//         },
//         error => {
//           if (error != undefined && error.status === 409) {
//             this.duplicateUser = this.f.userName.value;
//           }
//         });
//   }
//   emailDuplicateCheck() {
//     this.userService.emailDuplicateCheck(this.f.email.value)
//       .pipe(first())
//       .subscribe(
//         data => {
//           this.duplicateEmail = "";
//         },
//         error => {
//           if (this.userDetail) {
//             if (this.userDetail.email != this.f.email.value) {
//               this.duplicateEmail = this.f.email.value;
//             }
//           } else {
//             this.duplicateEmail = this.f.email.value;
//           }
//         });
//   }
//   passwordMatchValidator() {
//     this.passwordNotSame = this.f.password.value !== this.f.confirmPassword.value;
//   }

//   onSubmit() {
//     // this.emailDuplicateCheck();
//     // this.userDuplicateCheck();
//     this.f.birthday.setValue(this.f.month.value + "-" + this.f.day.value + "-" + this.f.year.value);
//     this.submitted = true;

//     if (this.f.userName.invalid || this.f.firstName.invalid || this.f.lastName.invalid || this.f.phoneNumber.invalid || this.f.birthday.invalid || this.f.gender.invalid || this.f.email.invalid || this.duplicateEmail === this.f.email.value) {
//       return;
//     }
//     if (this.userDetail == undefined && (this.duplicateUser === this.f.userName.value || this.passwordNotSame || this.f.password.invalid || this.f.confirmPassword.invalid)) {
//       return;
//     }

//     this.loading = true;
//     this.saveClicked.emit(this.registerForm.value);
//   }
// }