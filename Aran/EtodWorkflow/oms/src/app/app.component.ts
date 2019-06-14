import { Component } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  hideLayout = true;
  constructor(private router: Router) {
    router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.hideLayout = event.url.startsWith('/login') || event.url.startsWith('/signup') || event.url.startsWith('/recovery') || event.url.startsWith('/reset-password');
      }
    })
  }

  ngOnInit() {
  }
}