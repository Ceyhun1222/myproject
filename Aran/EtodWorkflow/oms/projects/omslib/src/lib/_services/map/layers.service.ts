import { Injectable } from '@angular/core';
import { Geojson } from './Geojson';
import { StyleService } from './style.service';
import { _config } from '../../_config/_config';

declare var ol: any;
declare var proj4: any;

@Injectable({
  providedIn: 'root'
})
export class LayersService {
  // Layer parent => TEST, VECTOR, OMS, BASE

  constructor(private stylesService: StyleService) { }

  getSource(url): any { 
    let user: any = JSON.parse(localStorage.getItem('user')) || JSON.parse(sessionStorage.getItem('user'));
    if(user && user.access){
      var vectorSource = new ol.source.Vector({
        format: new ol.format.GeoJSON(),
        loader: function(extent, resolution, projection) {
          // var proj = projection.getCode();
          var xhr = new XMLHttpRequest();
          xhr.open('GET', url);
          xhr.setRequestHeader('Authorization', 'Bearer '+user.access);
          var onError = function() {
            vectorSource.removeLoadedExtent(extent);
          }
          xhr.onerror = onError;
          xhr.onload = function() {
            if (xhr.status == 200) {
              vectorSource.addFeatures(
                  vectorSource.getFormat().readFeatures(xhr.responseText, {
                    dataProjection: 'EPSG:4326',
                    featureProjection: 'EPSG:3857',
                  }));
            } else {
              onError();
            }
          }
          xhr.send();
        },
      })
      return vectorSource;
    }
    return new ol.source.Vector();
  }

  draw:any;

  getBase(name: string = 'sat'): any {//map, sat , skl, osm, gomap
    var source;
    if (name === 'gomap') {
      source = new ol.source.OSM({
        crossOrigin: 'anonymous',
        url: "http://gomap.az/info/xyz.do?lng=az&x={x}&y={y}&z={z}&f=jpg",
      })
    } else if (name === 'osm') {
      source = new ol.source.OSM({ crossOrigin: 'anonymous' })
    } else {
      var url = name == 'sat' ? "https://sat0{1-4}.maps.yandex.net/tiles?l=sat&v=18.09.29-0&x={x}&y={y}&z={z}&scale=1" : name == 'map' ? "https://vec0{1-4}.maps.yandex.net/tiles?l=map&v=18.09.29-0&x={x}&y={y}&z={z}&scale=1" : "https://vec0{1-4}.maps.yandex.net/tiles?l=skl&v=18.09.29-0&x={x}&y={y}&z={z}&scale=1";
      proj4.defs('EPSG:3395', '+proj=merc +lon_0=0 +k=1 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs');
      source = new ol.source.XYZ({
        crossOrigin: 'anonymous',
        url: url,
        projection: 'EPSG:3395',
        tileGrid: ol.tilegrid.createXYZ({
          extent: [-20037508.342789244, -20037508.342789244, 20037508.342789244, 20037508.342789244]
        })
      })
    }
    return new ol.layer.Tile({
      source: source,
      parent: 'BASE',
      layername: name,
      zIndex: 0
    })
  };

  airports(): any {
    var this_=this
    return new ol.layer.Vector({
      source: this_.getSource(_config.links.airportsGeo),
      style: this.stylesService.airport,
      parent:'OMS',
      layername:'airport',
      zIndex: 10
    })
  }

  draw_(): any {
    if (this.draw != undefined) {
      return this.draw;
    }
    return this.draw = new ol.layer.Vector({
      source: new ol.source.Vector(),
      style: this.stylesService.draw,
      parent: 'VECTOR',
      layername: 'draw',
      zIndex: 110
    });
  }

  REQUEST(): any {
    return new ol.layer.Vector({
      source: new ol.source.Vector(),
      style: this.stylesService.all,
      parent: 'REQUEST',
      layername: 'REQUEST',
      zIndex: 1000
    });
  }

  verticalStructure(requestId): any {
    var url= _config.links.verticalStructuresGeo+requestId+'/5000';
    var this_=this
    return new ol.layer.Vector({
      source: this_.getSource(url),
      style: this.stylesService.verticalStructure,
      parent:'REQUEST',
      layername:'verticalStructure',
      zIndex: 10
    })
  }

  obstacleArea(requestId): any {
    var url= _config.links.obstacleAreasGeo+requestId;
    var this_=this
    return new ol.layer.Vector({
      source: this_.getSource(url),
      style: this.stylesService.obstacleArea,
      parent:'REQUEST',
      layername:'obstacleArea',
      zIndex: 10
    })
  }

  requestFeature(requestId): any {
    var url= _config.links.requestGeo+requestId;
    var this_=this;
    return new ol.layer.Vector({
      source: this_.getSource(url),
      style: this.stylesService.requestFeature,
      parent:'REQUEST',
      layername:'requestFeature',
      zIndex: 10
    })
  }

  // Tests //
  testLayer(): any {
    console.log(Geojson)
    return new ol.layer.Vector({
      source: new ol.source.Vector({
        features: (new ol.format.GeoJSON()).readFeatures(Geojson, {
          dataProjection: 'EPSG:4326',
          featureProjection: 'EPSG:3857',
        })
      }),
      style: this.stylesService.all,
      parent: 'TEST',
      layername: 'draw',
      zIndex: 1000
    });
  }
  geoJsonLayer(): any {
    return new ol.layer.Vector({
      source: new ol.source.Vector({
        url: "anyUrl",
        format: new ol.format.GeoJSON()
      }),
      style: this.stylesService.point,
      zIndex: 10
    })
  }
}