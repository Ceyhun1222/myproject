import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-map-info',
  templateUrl: './map-info.component.html',
  styleUrls: ['./map-info.component.css']
})
export class MapInfoComponent implements OnInit {

  constructor(private activatedRoute:ActivatedRoute, private router: Router) { }
  collapseIconRotate:boolean = false;

  data:any;

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      console.log(params);
      this.data = params;
    })

    this.activatedRoute.queryParams.subscribe(params => {
      console.log(params);
    })
  }
}