import { Component } from '@angular/core';
import { OmslibService } from '../../../omslib/src/public_api';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // title = '';

  hideLayout = true;
  constructor(private router: Router,private omslibService:OmslibService) {
    // this.title = this.omslibService.sayHello("Angular 6 Workspace lib works")
    router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.hideLayout = event.url.startsWith('/login') || event.url.startsWith('/signup') || event.url.startsWith('/recovery') || event.url.startsWith('/reset-password');
      }
    })

    if(!localStorage.getItem('lang')){
      localStorage.setItem('lang','en');
    } 
    // console.log(localStorage.getItem('lang'))
  }
}