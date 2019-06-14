import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AuthenticationService } from '../_services/authentication.service';
import { LangService } from 'projects/omslib/src/lib/_services/lang/langService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})

export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;lo
  submitted = false;
  returnUrl: string;
  passwordShow: boolean = false;
  badRequest: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    public lang:LangService) { }

  // dadalov, 123456Aa&
  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      remember: [true],
      role: ['Client']
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService.login(this.loginForm.value)
      .pipe(first())
      .subscribe(
        data => {
          console.log(data)
          if (this.returnUrl && this.returnUrl.length > 2) {
            this.router.navigate([this.returnUrl]);
          } else {
            this.router.navigate(['/requests']);
          }
        },
        error => {
          this.loading = false;
          this.badRequest = true;
        });
  }
}
