import { Injectable } from '@angular/core';
import { LayersService } from './layers.service';
import { StyleService } from './style.service';
declare var ol: any;

@Injectable({
  providedIn: 'root'
})
export class MeasureService {
  sketch;
  measureTooltipElement;
  measureTooltip;
  selectedType: string = '';

  constructor(private styleService: StyleService, private layerService: LayersService) { }

  drawInterAction; // global so we can remove it later

  formatLength = function (line) {
    var length = ol.Sphere.getLength(line);
    var output;
    if (length > 1000) {
      output = (Math.round(length / 1000 * 100) / 100) + ' ' + 'km';
    } else {
      // output = (Math.round(length * 100) / 100) + ' ' + 'm';
      output = length.toFixed(2) + ' ' + 'm';
    }
    return output;
  };
  formatLengthLast = function (line) {
    var coordinates = line.getCoordinates();
    var c1 = coordinates[coordinates.length - 2];
    var c2 = coordinates[coordinates.length - 1];
    line = new ol.geom.LineString([c1, c2])
    var length = ol.Sphere.getLength(line);
    var output;
    if (length > 1000) {
      output = (Math.round(length / 1000 * 100) / 100) + ' ' + 'km';
    } else {
      // output = (Math.round(length * 100) / 100) + ' ' + 'm';
      output = length.toFixed(2) + ' ' + 'm';
    }
    return output;
  };
  formatArea = function (polygon) {
    var area = ol.Sphere.getArea(polygon);
    var output;
    if (area > 10000) {
      output = (Math.round(area / 1000000 * 100) / 100) +
        ' ' + 'km<sup>2</sup>';
    } else {
      output = (Math.round(area * 100) / 100) +
        ' ' + 'm<sup>2</sup>';
    }
    return output;
  };

  addInteraction(map: any, type: string) {
    let self = this;
    this.drawInterAction = new ol.interaction.Draw({
      source: this.layerService.draw.getSource(),
      type: type == 'LineString' ? 'LineString' : 'Polygon',
      style: this.styleService.draw,
      parent: "interaction",
      layername: "draw"
    });
    map.addInteraction(this.drawInterAction);

    self.createMeasureTooltip(map);

    var listener;
    this.drawInterAction.on('drawstart',
      function (evt) {
        self.sketch = evt.feature;
        var tooltipCoord = evt.coordinate;

        listener = self.sketch.getGeometry().on('change', function (evt) {
          var geom = evt.target;
          var output;
          if (geom instanceof ol.geom.Polygon) {
            output = self.formatArea(geom);
            tooltipCoord = geom.getInteriorPoint().getCoordinates();
          } else if (geom instanceof ol.geom.LineString) {
            output = self.formatLength(geom);
            output += ' (' + self.formatLengthLast(geom) + ')';
            tooltipCoord = geom.getLastCoordinate();
          }
          self.measureTooltipElement.innerHTML = output;
          self.measureTooltip.setPosition(tooltipCoord);
        });
      }, this);

    this.drawInterAction.on('drawend', function (evt) {
      this.measureTooltipElement.className = 'tooltip tooltip-static';
      this.measureTooltip.setOffset([0, -7]);
      this.sketch = null;
      this.measureTooltipElement = null;
      this.createMeasureTooltip(map);
      ol.Observable.unByKey(listener);
    }, this);
  }

  createMeasureTooltip(map: any) {
    console.log('createMeasureTooltip');
    if (this.measureTooltipElement) {
      this.measureTooltipElement.parentNode.removeChild(this.measureTooltipElement);
    }
    this.measureTooltipElement = document.createElement('div');
    this.measureTooltipElement.className = 'tooltip tooltip-measure';
    this.measureTooltip = new ol.Overlay({
      element: this.measureTooltipElement,
      offset: [0, -15],
      positioning: 'bottom-center',
      autoPan: true,
      autoPanAnimation: 'easing',
      autoPanMargin: 2
    });
    map.addOverlay(this.measureTooltip);
  }

  removeAll(map: any) {
    this.layerService.draw.getSource().clear();
    this.selectedType = '';
    this.measureTooltipElement = null;
    map.getOverlays().getArray().slice(0).forEach(function (overlay) {
      map.removeOverlay(overlay);
    });
  }

  init(map: any, type: string) {
    map.removeInteraction(this.drawInterAction);
    if (type === this.selectedType) {
      this.removeAll(map);
      return;
    }
    this.selectedType = type;
    this.addInteraction(map, type);
  }
}