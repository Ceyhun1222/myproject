import { Injectable } from '@angular/core';
declare var proj4: any;
declare var ol: any;
@Injectable({
  providedIn: 'root'
})
export class ProjService {
  constructor() { }
  getUtmZone = function (t) { return Math.floor((t[0] + 180) / 6) % 60 + 1 };
  utm_(coor4326): any {
    return proj4.Proj('+proj=utm +datum=WGS84 +units=m +no_defs +zone=' + this.getUtmZone(coor4326));
  };
  googleToUtm = function (o) { return o = proj4.transform(proj4.WGS84, this.utm_(o), o), o = [o.x, o.y] };
  
  googleToDmsString = function (coorObj) {
    var coor = [coorObj.longitude, coorObj.latitude];
    return ol.coordinate.toStringHDMS(coor, 2);
  }
  
  googleToDms = function (coorObj) {
    var coor = [coorObj.longitude, coorObj.latitude];
    // console.log(coor);
    var dmsStr = ol.coordinate.toStringHDMS(coor, 2);
    // console.log(dmsStr);
    var dmsParsed = dmsStr.match(/[-]{0,1}[\d.]*[\d]|([NSEW])+/g);
    // console.log(dmsParsed);
    if (dmsParsed.length == 8) {
      var dmsParsedObj = {
        latitude: {
          degree: dmsParsed[0],
          minute: dmsParsed[1],
          second: dmsParsed[2],
          direction: dmsParsed[3]
        },
        longitude: {
          degree: dmsParsed[4],
          minute: dmsParsed[5],
          second: dmsParsed[6],
          direction: dmsParsed[7]
        }
      }
      return dmsParsedObj;
    }
    return null;
  };
  dmsToGoogle = function (o: any) {//{degree:"40",minute:"24",second:"47.9",direction:"N"}
    var n = NaN;
    if (o) {
      var t = Number(o.degree),
        d = "undefined" != typeof o.minute ? Number(o.minute) / 60 : 0,
        l = "undefined" != typeof o.second ? Number(parseFloat(o.second.replace(',', '.'))) / 3600 : 0,
        r = o.direction || null;
      null !== r && /[SW]/i.test(r) && (t = -1 * Math.abs(t)), n = 0 > t ? t - d - l : t + d + l
    }
    return n
  }
  isGoogleCoor = function (o) { return isNaN(o[0]) || o[0] < -180 || o[0] > 180 || isNaN(o[1]) || o[1] < -90 || o[1] > 90 ? !1 : !0 }
  
  // utmToGoogle = function (o, r) { return r >= 0 && 60 >= r ? (o = proj4.transform(proj4.Proj("+proj=utm +datum=WGS84 +units=m +no_defs +zone=" + r), proj4.WGS84, o), o = [o.x, o.y]) : (alert("Utm zone error"), null) };
  // googleToDmsOld = function (i) { var n, o = /(N|E|S|W)+/; i = ol.coordinate.toStringHDMS(i, 2); var r = i.match(o); return void 0 != r && r.index > 10 ? (n = [], n.push(i.slice(r.index + 1).trim()), n.push(i.slice(0, r.index + 1).trim()), n) : null };
  // dtgReg = /[-]{0,1}[\d.]*[\d]|([NSEW])+/g;//output["59", "55", "43.95", "E"]
  // dmsToGoogleFull(dmsCoor: any): any {
  //   dmsCoor[0] = this.dmsToGoogleOld(dmsCoor[0]);
  //   dmsCoor[1] = this.dmsToGoogleOld(dmsCoor[1]);
  //   return dmsCoor;
  // }
  // dmsToGoogleOld = function (e) { "lon or lat"; var n = NaN, o = e.match(this.dtgReg); if (o) { var t = Number(o[0]), d = "undefined" != typeof o[1] ? Number(o[1]) / 60 : 0, l = "undefined" != typeof o[2] ? Number(o[2]) / 3600 : 0, r = o[3] || null; null !== r && /[SW]/i.test(r) && (t = -1 * Math.abs(t)), n = 0 > t ? t - d - l : t + d + l } return n }

  // dmsToGoogle2 = function () {//["59", "23", "43.95", "E"]
  //   "lon or lat";
  //   var n = NaN, o = ["40", "24", "47.9", "N"];
  //   if (o) {
  //     var t = Number(o[0]),
  //       d = "undefined" != typeof o[1] ? Number(o[1]) / 60 : 0,
  //       l = "undefined" != typeof o[2] ? Number(o[2]) / 3600 : 0,
  //       r = o[3] || null;
  //     null !== r && /[SW]/i.test(r) && (t = -1 * Math.abs(t)), n = 0 > t ? t - d - l : t + d + l
  //   }
  //   return n
  // }
}