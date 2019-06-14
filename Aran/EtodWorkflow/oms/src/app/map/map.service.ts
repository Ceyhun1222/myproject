import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LayersService } from '../../../projects/omslib/src/lib/_services/map/layers.service';
import { ProjService } from '../../../projects/omslib/src/lib/_services/map/proj.service';
import { MeasureService } from '../../../projects/omslib/src/lib/_services/map/measure.service';

declare var ol: any;

@Injectable({
  providedIn: 'root'
})
export class MapService {
  map: any;
  view: any;
  mapTarget;// = document.getElementById('map');
  utmZone: any = 39;
  projection: string;//UTM, WGS84_DMS, WGS84_DD
  selectedBaseName: string;

  testLayer: any;
  airport: any;
  vs: any;
  obstacleArea: any;
  requestFeature: any;

  infoPopup: any;
  infoPopupPropertiest:any;


  constructor(
    private layersService: LayersService,
    private projService: ProjService,
    private measureService: MeasureService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    if (localStorage.getItem('projection') == undefined) {
      localStorage.setItem('projection', 'WGS84_DD');
    }
    this.projection = localStorage.getItem('projection');

    if (localStorage.getItem('BASE') == undefined) {
      localStorage.setItem('BASE', 'sat');
    }
    this.selectedBaseName = localStorage.getItem('BASE');
  }


  load() {
    this.map = new ol.Map({
      view: new ol.View({
        // center: ol.proj.transform([50.5, 70.1], 'EPSG:4326', 'EPSG:3857'),
        center: [7651174.709608254, 6181198.957271606],
        zoom: 6,
        minZoom: 3,
        maxZoom: 19
      }),
      interactions: ol.interaction.defaults({
        pinchRotate: false,
      }).extend([
        new ol.interaction.DoubleClickZoom({ duration: 500 }),
        new ol.interaction.MouseWheelZoom({ duration: 500 }),
        new ol.interaction.DragRotate({
          condition: ol.events.condition.altKeyOnly
        })
      ]),
      controls: ol.control.defaults({
        attribution: false
      }).extend([
        new ol.control.FullScreen(),
        new ol.control.ScaleLine(),
        this.mousePositionControl()
      ])
    });
    this.view = this.map.getView();
    this.loadLayer();

    this.mapTarget = document.getElementById('map');
    this.map.setTarget(this.mapTarget);
    this.events();
    this.loadPopup();
  }

  projectionChange() {
    this.projection = this.projection == 'UTM' ? 'WGS84_DMS' : this.projection == 'WGS84_DMS' ? 'WGS84_DD' : 'UTM';
    localStorage.setItem('projection', this.projection);
  };

  mousePositionControl = function () {
    let self = this, _3857 = "EPSG:3857", _4326 = "EPSG:4326";
    return new ol.control.MousePosition({
      undefinedHTML: undefined,
      coordinateFormat: function (coor) {
        coor = ol.proj.transform(coor, _3857, _4326);
        if (self.projection == 'UTM') {
          self.utmZone = self.projService.getUtmZone(coor);
          coor = self.projService.googleToUtm(coor);
          return (ol.coordinate.format(coor, '{y}, {x}', 3));
        } else if (self.projection == 'WGS84_DMS') {
          return ol.coordinate.toStringHDMS(coor, 2);
        }
        return ol.coordinate.format(coor, '{y}, {x}', 5);
      }
    })
  };

  loadPopup() {
    this.map.removeOverlay(this.infoPopup);
    this.infoPopup = new ol.Overlay({
      element: document.getElementById('popup'),
      positioning: 'center-center',
      autoPan: true,
      autoPanAnimation: {
        duration: 500
      }
    });
    this.map.addOverlay(this.infoPopup);
    this.infoPopup.setPosition(null);
  }

  events() {
    let _this = this;
    this.map.on('pointermove', function (event) {
      var hit = _this.map.hasFeatureAtPixel(event.pixel);
      _this.mapTarget.style.cursor = hit ? 'pointer' : null;
      _this.map.forEachFeatureAtPixel(event.pixel, function (feature, layer) {
        if (feature) {
          // console.log(feature.get('name'));
        }
      });
    });

    _this.map.on('singleclick', function (event) {
      _this.infoPopup.setPosition();
      _this.map.forEachFeatureAtPixel(event.pixel, function (feature, layer) {
        // if (feature.get('Name') != undefined) {
        //   var Name = feature.get('Name');
        //   var Designator = feature.get('Designator');
        //   _this.router.navigate(['map/info/'+Name+'/'+Designator], { relativeTo: _this.activatedRoute });
        // }
        var prop = feature.getProperties();
        delete prop.geometry
        console.log(prop);
        prop = JSON.parse(JSON.stringify(prop));
        console.log(prop);
        if(Object.entries(prop).length > 0){
          _this.infoPopupPropertiest = prop;
          var coor = feature.getGeometry().getType() == "Point" ? feature.getGeometry().getCoordinates(): event.coordinate;
          _this.infoPopup.setPosition(coor);
        }
        // if (prop && prop.Name && prop.Designator) {
        //   // document.getElementById('popup').innerHTML = '<div class="font-weight-bold">' + feature.get('Name') + '</div>' + '<div>' + feature.get('Designator') + '</div>';
        // }
        // console.log(event.coordinate);
        // console.log(feature.getGeometry().getCoordinates());
      },{
          hitTolerance: 15
        });
    });
  }

  loadLayer() {
    this.baseChange(localStorage.getItem('BASE'));

    this.map.removeLayer(this.layersService.draw_());
    this.map.addLayer(this.layersService.draw_());

    this.airport = this.layersService.airports();
    this.map.addLayer(this.airport);
    let this_ = this;
    // var listenerKey = this.airport.getSource().on('change', function(e) {
    //   if (this_.airport.getSource().getState() == 'ready') {
    //     this_.map.getView().fit(this_.airport.getSource().getExtent(), this_.map.getSize());
    //     ol.Observable.unByKey(listenerKey);
    //   }
    // });
  }

  loadVsLayer(requestId) {
    this.map.removeLayer(this.vs);
    this.vs = this.layersService.verticalStructure(requestId);
    this.map.addLayer(this.vs);
    // let this_ = this;
    // var listenerKey = this.vs.getSource().on('change', function(e) {
    //   if (this_.vs.getSource().getState() == 'ready') {
    //     this_.map.getView().fit(this_.vs.getSource().getExtent(),{size:this_.map.getSize(), duration:2000});
    //     ol.Observable.unByKey(listenerKey);
    //   }
    // });
  }

  loadOmstacleAreaLayer(requestId) {
    this.map.removeLayer(this.obstacleArea);
    this.obstacleArea = this.layersService.obstacleArea(requestId);
    this.map.addLayer(this.obstacleArea);
    let this_ = this;
    // var listenerKey = this.obstacleArea.getSource().on('change', function(e) {
    //   if (this_.obstacleArea.getSource().getState() == 'ready') {
    //     this_.map.getView().fit(this_.obstacleArea.getSource().getExtent(), this_.map.getSize());
    //     ol.Observable.unByKey(listenerKey);
    //   }
    // });
  }

  loadRequestLayer(requestId) {
    this.map.removeLayer(this.requestFeature);
    this.requestFeature = this.layersService.requestFeature(requestId);
    this.map.addLayer(this.requestFeature);
    let this_ = this;
    var listenerKey = this.requestFeature.getSource().on('change', function (e) {
      if (this_.requestFeature.getSource().getState() == 'ready') {
        this_.map.getView().fit(this_.requestFeature.getSource().getExtent(), { size: this_.map.getSize(), duration: 2000, maxZoom: 13 });
        ol.Observable.unByKey(listenerKey);
      }
    });
  }
  redrawOmstacleLayers() {
    this.obstacleArea && this.obstacleArea.changed();
    this.vs && this.vs.changed();
    this.requestFeature && this.requestFeature.changed();
  }

  get layers() { return this.map.getLayers(); }

  baseChange(name: string) {
    localStorage.setItem('BASE', name);
    this.selectedBaseName = name;
    let this_ = this;
    this.map.getLayers().forEach(function (layer) {
      if (layer && layer.get('parent') === 'BASE') {
        this_.map.removeLayer(layer);
      }
    });
    name === 'skl' && this.map.addLayer(this.layersService.getBase());
    this.map.addLayer(this.layersService.getBase(name));
  }

  measureInit(type: string) {
    this.measureService.init(this.map, type);
  }
}