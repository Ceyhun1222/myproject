import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services';
import { Router } from '@angular/router';
import { LangService } from 'projects/omslib/src/lib/_services/lang/langService';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  user: any = {};

  constructor(private authenticationService: AuthenticationService, private router: Router, public lang: LangService) {}

  ngOnInit() {
    let user: any = JSON.parse(localStorage.getItem('user')) || JSON.parse(sessionStorage.getItem('user'));
    if (user && user.fullName && user.id) {
      this.user = user;
    }
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
