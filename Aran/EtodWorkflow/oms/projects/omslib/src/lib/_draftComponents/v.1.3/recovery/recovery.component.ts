// import { Component, OnInit } from '@angular/core';
// import { Router, ActivatedRoute } from '@angular/router';
// import { FormBuilder, FormGroup, Validators } from '@angular/forms';
// import { first } from 'rxjs/operators';

// import { AuthenticationService } from '../_services';

// @Component({
//   selector: 'app-recovery',
//   templateUrl: './recovery.component.html',
//   styleUrls: ['./recovery.component.css']
// })

// export class RecoveryComponent implements OnInit {
//   recoveryForm: FormGroup;
//   loading = false;
//   submitted = false;
//   returnUrl: string;
//   showAlert: boolean = false;
//   noExistEmail: string = "";

//   constructor(
//     private formBuilder: FormBuilder,
//     private route: ActivatedRoute,
//     private router: Router,
//     private authenticationService: AuthenticationService) { }

//   ngOnInit() {
//     this.recoveryForm = this.formBuilder.group({
//       email: ['', [Validators.required, Validators.email]]
//     });

//     // get return url from route parameters or default to '/'
//     this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
//   }

//   // convenience getter for easy access to form fields
//   get f() { return this.recoveryForm.controls; }

//   onSubmit() {
//     this.submitted = true;

//     // stop here if form is invalid
//     if (this.recoveryForm.invalid || this.noExistEmail === this.f.email.value) {
//       console.log("stop here if form is invalid")
//       return;
//     }

//     this.loading = true;
//     this.authenticationService.forgotPassword(this.f.email.value)
//       .pipe(first())
//       .subscribe(
//         data => {
//           this.showAlert = true;
//         },
//         error => {
//           console.log(error);
//           this.loading = false;
//           this.showAlert = false;
          
//           this.noExistEmail = this.f.email.value;
//         });
//   }
// }