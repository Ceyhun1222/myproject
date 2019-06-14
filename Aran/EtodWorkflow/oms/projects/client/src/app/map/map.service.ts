import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LayersService } from 'projects/omslib/src/lib/_services/map/layers.service';
import { ProjService } from 'projects/omslib/src/lib/_services/map/proj.service';
import { MeasureService } from 'projects/omslib/src/lib/_services/map/measure.service';

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

  REQUEST: any;
  airport: any;


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
        // center: ol.proj.transform([50, 72], 'EPSG:4326', 'EPSG:3857'),
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
      _this.map.forEachFeatureAtPixel(event.pixel, function (feature, layer) {
        if (feature.get('Name') != undefined) {
          var Name = feature.get('Name');
          var Designator = feature.get('Designator');
          _this.router.navigate(['map/info/' + Name + '/' + Designator], { relativeTo: _this.activatedRoute });
        }
        // console.log(event.coordinate);
        // console.log(feature.getGeometry().getCoordinates());
      });
    });
  }

  loadLayer() {
    this.baseChange(localStorage.getItem('BASE'));

    this.map.addLayer(this.layersService.draw_());

    this.REQUEST = this.layersService.REQUEST();
    this.map.addLayer(this.REQUEST);
  }

  makeFeature(coordinates, type, extraPoint) {
    console.log(type);
    console.log(coordinates);
    console.log(extraPoint);

    var geom = new ol.geom.LineString(coordinates).transform('EPSG:4326', 'EPSG:3857');
    if (type == 'Point') {
      geom = new ol.geom.Point(coordinates[0]).transform('EPSG:4326', 'EPSG:3857');
    } else if (type == 'Polygon') {
      console.log(coordinates);
      geom = new ol.geom.Polygon([coordinates]).transform('EPSG:4326', 'EPSG:3857');
    }
    console.log(geom.getType());
    console.log(geom.getCoordinates());
    var features = [new ol.Feature({
      geometry: geom,
    })];
    if (extraPoint && extraPoint.length == 2) {
      features.push(new ol.Feature({
        geometry: new ol.geom.Point(extraPoint).transform('EPSG:4326', 'EPSG:3857')
      }));
    }
    this.REQUEST.getSource().clear();
    this.REQUEST.getSource().addFeatures(features);
    this.map.getView().fit(this.REQUEST.getSource().getExtent(), { size: this.map.getSize(), duration: 1000, maxZoom: this.map.getView().getZoom() });
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