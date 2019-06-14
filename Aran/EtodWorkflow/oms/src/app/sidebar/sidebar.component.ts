import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';
declare var $: any;
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  // styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {

  }

  ngAfterContentInit() {
    document.body.classList.toggle('sidebar-xs');
    var elem = document.getElementsByClassName('sidebar-main-toggle');
    Array.from(elem).forEach(function (element) {
      element.addEventListener('click', function (e) {
        e.preventDefault();
        document.body.classList.toggle('sidebar-xs');
      });
    });

    // Toggle main sidebar on mobile
    $('.sidebar-mobile-main-toggle').on('click', function (e) {
      e.preventDefault();
      $('body').toggleClass('sidebar-mobile-main').removeClass('sidebar-mobile-secondary sidebar-mobile-right');

      if ($('.sidebar-main').hasClass('sidebar-fullscreen')) {
        $('.sidebar-main').removeClass('sidebar-fullscreen');
      }
    });

    // Expand sidebar to full screen on mobile
    $('.sidebar-mobile-expand').on('click', function (e) {
      e.preventDefault();
      var $sidebar = $(this).parents('.sidebar'),
        sidebarFullscreenClass = 'sidebar-fullscreen'

      if (!$sidebar.hasClass(sidebarFullscreenClass)) {
        $sidebar.addClass(sidebarFullscreenClass);
      }
      else {
        $sidebar.removeClass(sidebarFullscreenClass);
      }
    });
  }
}