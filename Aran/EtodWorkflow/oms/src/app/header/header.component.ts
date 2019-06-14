import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService, SettingsService } from '../_services';
import { UserService } from 'projects/omslib/src/lib/_services/user.service';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  user: any = {};
  userNotificationCount: any = 0;
  requestNotificationCount: any = 0;

  constructor(private router: Router,
    private authenticationService: AuthenticationService,
    private settingsService: SettingsService, private userService: UserService,
    public publicService:PublicService) { }

  ngOnInit() {
    console.log('HeaderComponent - ngOnInit');
    let _this = this;
    let user: any = JSON.parse(localStorage.getItem('user')) || JSON.parse(sessionStorage.getItem('user'));
    if (user && user.fullName && user.id) {
      this.user = user;
      this.getUserNotification();
      this.getRequestNotification();
      setInterval(function () {
        _this.getUserNotification();
        _this.getRequestNotification();
      }, 10000)
    }
  }

  getUserNotification() {
    this.settingsService.getNotificationCount().pipe().subscribe((data) => {
      console.log(data);
      this.userNotificationCount = data;
    });
  }

  getRequestNotification() {
    this.userService.getNotificationCount().pipe().subscribe((data) => {
      console.log(data);
      this.requestNotificationCount = data;
    });
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

  nagiateTo(path) {
    this.router.navigate([path+this.publicService.getUtcTime()]);
  }
}