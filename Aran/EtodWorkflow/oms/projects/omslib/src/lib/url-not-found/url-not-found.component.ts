import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'lib-url-not-found',
  template: `<div class="flex-fill mt-5">
    <div class="text-center mb-3">
      <h1 class="error-title">404</h1>
      <h5>Oops, an error has occurred. Page not found!</h5>
    </div>
  </div>`,
  styles: [`.error-title {
    color: #fff;
    font-size: 12rem;
    line-height: 1;
    margin-bottom: 2.5rem;
    font-weight: 300;
    display: block;
    text-shadow: 0 1px 0 #ccc, 0 2px 0 #c9c9c9, 0 3px 0 #bbb, 0 4px 0 #b9b9b9, 0 5px 0 #aaa, 0 6px 1px rgba(0,0,0,.1), 0 0 5px rgba(0,0,0,.1), 0 1px 3px rgba(0,0,0,.3), 0 3px 5px rgba(0,0,0,.2), 0 5px 10px rgba(0,0,0,.25), 0 10px 10px rgba(0,0,0,.2), 0 20px 20px rgba(0,0,0,.15);
  }`]
})
export class UrlNotFoundComponent implements OnInit {
  constructor() { }
  ngOnInit() {
  }
}