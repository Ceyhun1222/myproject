import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';

import { PublicService } from '../_services/public.service';
import { LangService } from '../_services/lang/langService';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html'
})

export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: any;
  loading = false;
  submitted = false;
  passwordsMatched;
  emailExist: string = "";
  passwordShow: boolean = false;
  confirmPasswordShow: boolean = false;
  requestFailed = false;
  dataFailed = false;


  constructor(
    private router: Router,
    private publicService: PublicService,
    private userService:UserService,
    public lang:LangService) { }

  ngOnInit() {
    this.resetPasswordForm = {
      "email": "",
      "password": "",
      "confirmPassword": "",
      "code": ""
    }
    var url_ = location.href.split('/');
    this.resetPasswordForm.code = url_[url_.length - 2];
    this.resetPasswordForm.email = url_[url_.length - 1];
    if (!(this.publicService.validateEmail(this.resetPasswordForm.email) && (this.resetPasswordForm.code && this.resetPasswordForm.code.length > 0))) {
      this.dataFailed = true;
    }
    console.log(this.resetPasswordForm);
  }

  get f() { return this.resetPasswordForm; }

  passwordMatchValidator() {
    return this.passwordsMatched = this.f.password === this.f.confirmPassword;
  }

  onSubmit() {
    console.log(this.resetPasswordForm);
    this.submitted = true;

    if (this.f.password && this.f.password.length > 0 && this.f.confirmPassword && this.f.confirmPassword.length > 0 && this.passwordMatchValidator()) {
      this.loading = true;
      this.userService.resetPassword(this.resetPasswordForm)
        .pipe(first())
        .subscribe(
          data => {
            this.publicService.alertMsg("Your password successfully changed", 1500);
            this.router.navigate(['/login']);
            // location.reload(true);
          },
          error => {
            console.log(error);
            this.loading = false;
            this.requestFailed = true;
          });
    } else {
      console.log("stop here if form is invalid");
    }
  }
}