// import { Component, OnInit } from '@angular/core';
// import { Router, ActivatedRoute } from '@angular/router';
// import { FormBuilder, FormGroup, Validators } from '@angular/forms';
// import { first } from 'rxjs/operators';

// import { AuthenticationService, UserService } from '../_services';
// import { PublicService } from 'projects/omslib/src/lib/_services/public.service';
// // import { PublicService } from '../_services/public/public.service';

// @Component({
//   selector: 'app-reset-password',
//   templateUrl: './reset-password.component.html',
//   styleUrls: ['./reset-password.component.css']
// })

// export class ResetPasswordComponent implements OnInit {
//   resetPasswordForm: FormGroup;
//   loading = false;
//   submitted = false;
//   requestFailed = false;
//   passwordNotSame;
//   emailExist: string = "";
//   passwordShow:boolean = false;
//   confirmPasswordShow:boolean = false;

//   constructor(
//     private formBuilder: FormBuilder,
//     private activatedRoute: ActivatedRoute,
//     private authenticationService: AuthenticationService,
//     private userService: UserService, private router: Router,
//     private publicService: PublicService) { }

//   ngOnInit() {
//     this.resetPasswordForm = this.formBuilder.group({
//       email: ['', [Validators.required]],
//       password: ['', [Validators.required, Validators.minLength(6)]],
//       confirmPassword: ['', [Validators.required]],
//       code: ['', [Validators.required]]
//     });

//     this.activatedRoute.params.subscribe(params => {
//       console.log(params);
//       if (params != undefined && params.code && params.email) {
//         this.f.code.setValue(params.code);
//         this.f.email.setValue(params.email);
//       }
//     });

//     var url_ = location.href.split('/');
//     this.f.code.setValue(url_[url_.length - 2]);
//     this.f.email.setValue(url_[url_.length - 1]);
//     console.log(this.resetPasswordForm.value);
//   }

//   get f() { return this.resetPasswordForm.controls; }

//   passwordMatchValidator() {
//     this.passwordNotSame = this.f.password.value !== this.f.confirmPassword.value;
//   }

//   onSubmit() {
//     // console.log(this.resetPasswordForm.value);
//     this.submitted = true;

//     if (this.resetPasswordForm.invalid) {
//       // console.log("stop here if form is invalid");
//       return;
//     }

//     this.loading = true;
//     this.authenticationService.resetPassword(this.resetPasswordForm.value)
//       .pipe(first())
//       .subscribe(
//         data => {
//           this.publicService.alertMsg("Your password successfully changed", 1500);
//           this.router.navigate(['/login']);
//         },
//         error => {
//           // console.log(error);s
//           this.loading = false;
//           this.requestFailed = true;
//         });
//   }
// }