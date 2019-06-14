import { Component, OnInit } from '@angular/core';
import { LayersService } from '../../../projects/omslib/src/lib/_services/map/layers.service';
import { ProjService } from '../../../projects/omslib/src/lib/_services/map/proj.service';
import { MeasureService } from '../../../projects/omslib/src/lib/_services/map/measure.service';
import { MapService } from './map.service';
declare var $: any;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  objectKeys = Object.keys;
  
  constructor( public mapService: MapService, public layerService: LayersService, public projService: ProjService, public measureService: MeasureService) { }
  ngOnInit() {
    console.log('MapComponent init');
    this.mapService.load();
  }

  ngAfterContentInit() {
    let this_ = this;
    $(document).on('click', '.es-stop-propagation', function (e) {
      e.stopPropagation();
    });
    $(document).on('click', '.sidebar-main-toggle', function (e) {
      console.log('updateSize')
      this_.mapService.map.updateSize();
    });
  }
}