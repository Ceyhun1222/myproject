import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { PublicService } from '../_services/public.service';
import { LangService } from '../_services/lang/langService';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-recovery',
  templateUrl: './recovery.component.html'
})

export class RecoveryComponent implements OnInit {
  recoveryForm: any = {
    email: ""
  };
  loading = false;
  submitted = false;
  showAlert: boolean = false;
  notFoundEmail: string = "";
  invalidEmail: string = "";

  constructor(
    private router: Router,
    private userService: UserService,
    private publicService: PublicService,
    public lang:LangService) { }

  ngOnInit() {
  }

  passwordMatchValidator() {
    this.invalidEmail = "";
    if (!this.publicService.validateEmail(this.recoveryForm.email)) {
      this.invalidEmail = this.recoveryForm.email;
    }
  }

  returnToLogin() {
    this.router.navigate(['/login']);
    // location.reload(true);
  }

  onSubmit() {
    this.submitted = true;
    if (this.publicService.validateEmail(this.recoveryForm.email) && this.notFoundEmail !== this.recoveryForm.email) {
      this.loading = true;
      this.userService.forgotPassword(this.recoveryForm.email)
        .pipe(first())
        .subscribe(
          data => {
            this.showAlert = true;
          },
          error => {
            console.log(error);
            this.loading = false;
            this.showAlert = false;

            this.notFoundEmail = this.recoveryForm.email;
          });
    }
  }
}