import { Injectable } from '@angular/core';
import { StyleFilter } from './style-filter';
declare var ol: any;

@Injectable({
  providedIn: 'root'
})
export class StyleService {

  constructor() {
    var this_ = this;
  }

  airport(feature) {
    var style = new ol.style.Style({
      image: new ol.style.Icon({
        src: '../../../../../../global_assets/images/airplane7.png',
        scale: 1
      }),
      text: new ol.style.Text({
        text: feature.get('Name'),
        font: '12px Calibri,sans-serif',
        fill: new ol.style.Fill({
          color: 'black'

        }),
        stroke: new ol.style.Stroke({
          color: 'red',
          width: 0.2
        }),
        scale: '1',
        textBaseline: 'bottom',
        textAlign: 'center',
        offsetY: -8
      })
    });
    return [style];
  }

  verticalStructure(feature) {
    if (feature.getGeometry().getType() && StyleFilter.filter.includes(feature.getGeometry().getType())) {
      return new ol.style.Style({
        fill: new ol.style.Fill({
          color: 'rgba(128, 128, 128, 0.1)'
        }),
        stroke: new ol.style.Stroke({
          color: 'yellow',
          width: 1
        }),
        image: new ol.style.Circle({
          radius: 3,
          stroke: new ol.style.Stroke({
            color: 'black',
            width: 0.3
          }),
          fill: new ol.style.Fill({
            color: 'yellow'
          })
        })
      })
    }
  }

  verticalStructureTest(feature) {
    if (feature.getGeometry().getType() && StyleFilter.filter.includes(feature.getGeometry().getType())) {
      return new ol.style.Style({
        fill: new ol.style.Fill({
          color: StyleFilter.verticalStructure.fill
        }),
        stroke: new ol.style.Stroke({
          color: StyleFilter.verticalStructure.stroke,
          width: 1
        }),
        image: new ol.style.Circle({
          radius: StyleFilter.verticalStructure.radius,
          stroke: new ol.style.Stroke({
            color: StyleFilter.verticalStructure.stroke,
            width: 0.3
          }),
          fill: new ol.style.Fill({
            color: StyleFilter.verticalStructure.fill
          })
        })
      })
    }
  }

  obstacleArea(feature) {
    var style = [];
    if (feature.getId() && StyleFilter.filter.includes(feature.getId())) {
      style.push(new ol.style.Style({
        fill: new ol.style.Fill({
          color: 'rgba(128, 128, 128, 0.1)'
        }),
        stroke: new ol.style.Stroke({
          color: 'rgb(255, 255, 0)',
          width: 1
        }),
        image: new ol.style.Circle({
          radius: 5,
          stroke: new ol.style.Stroke({
            color: 'white',
            width: 1
          }),
          fill: new ol.style.Fill({
            color: 'blue'
          })
        })
      }))
    }

    if (feature.getId() == StyleFilter.selectedArea) {
      style.push(new ol.style.Style({
        fill: new ol.style.Fill({
          color: 'rgba(255, 0, 0, 0.1)'
        }),
        stroke: new ol.style.Stroke({
          color: 'rgb(255, 0, 0)',
          width: 0.5
        }),
        image: new ol.style.Circle({
          radius: 7,
          stroke: new ol.style.Stroke({
            color: 'white',
            width: 1.3
          }),
          fill: new ol.style.Fill({
            color: 'rgba(255, 0, 0, 0.1)'
          })
        }),
        zIndex: 1000
      }))
    }
      return style;
  }

  requestFeature(feature) {//r-e-q-u-e-s-t-f-e-a-t-u-r-e
    // console.log(feature.getProperties());
    if (StyleFilter.filter.includes('r-e-q-u-e-s-t-f-e-a-t-u-r-e')) {
      return new ol.style.Style({
        fill: new ol.style.Fill({
          color: 'rgba(128, 128, 128, 0.3)'
        }),
        stroke: new ol.style.Stroke({
          color: 'red',
          // lineDash: [4, 4],
          width: 2
        }),
        image: new ol.style.Circle({
          radius: 5,
          stroke: new ol.style.Stroke({
            color: 'white',
            width: 2
          }),
          fill: new ol.style.Fill({
            color: 'blue'
          })
        })
      })
    }
    return null;
  }

  draw(): any {
    return new ol.style.Style({
      fill: new ol.style.Fill({
        color: 'rgba(255, 255, 255, 0.2)'
      }),
      stroke: new ol.style.Stroke({
        color: 'rgba(0, 0, 0, 0.5)',
        lineDash: [4, 4],
        width: 2
      }),
      image: new ol.style.Circle({
        radius: 5,
        stroke: new ol.style.Stroke({
          color: 'rgba(0, 0, 0, 0.7)'
        }),
        fill: new ol.style.Fill({
          color: 'rgba(255, 255, 255, 0.2)'
        })
      })
    })
  }

  // Test //
  all(): any {
    return new ol.style.Style({
      fill: new ol.style.Fill({
        color: 'rgba(255, 255, 255, 0.2)'
      }),
      stroke: new ol.style.Stroke({
        color: 'yellow',
        // lineDash: [4, 4],
        width: 2
      }),
      image: new ol.style.Circle({
        radius: 5,
        stroke: new ol.style.Stroke({
          color: 'blue',
          width: 1
        }),
        fill: new ol.style.Fill({
          color: 'yellow'
        })
      })
    })
  }

  point(feature) {
    var style = new ol.style.Style({
      image: new ol.style.Circle({
        radius: 6,
        stroke: new ol.style.Stroke({
          color: 'white',
          width: 1
        }),
        fill: new ol.style.Fill({
          color: 'yellow'
        })
      }),
      text: new ol.style.Text({
        text: false && feature.get('name'),
        font: '12px Calibri,sans-serif',
        fill: new ol.style.Fill({
          color: 'blue'

        }),
        stroke: new ol.style.Stroke({
          color: 'black',
          width: 0.5
        }),
        scale: '1.5',
        textBaseline: 'bottom',
        textAlign: 'center',
        offsetY: -8
      })
    });
    return [style];
  }
}