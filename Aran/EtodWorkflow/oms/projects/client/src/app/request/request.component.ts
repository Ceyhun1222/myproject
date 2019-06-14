import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
// import { UserService } from '../_services';
import { first } from 'rxjs/operators';
import { ProjService } from 'projects/omslib/src/lib/_services/map/proj.service';
import { MapService } from '../map/map.service';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';
import { RequestService } from 'projects/omslib/src/lib/_services/request.service';
import { _config } from 'projects/omslib/src/lib/_config/_config';
import { UserService } from 'projects/omslib/src/lib/_services/user.service';
import { LangService } from 'projects/omslib/src/lib/_services/lang/langService';


@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.css']
})
export class RequestComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private requestService: RequestService, private projService: ProjService, private mapService: MapService, private router: Router, private publicService: PublicService, private userService: UserService, public lang: LangService) { }

  requestData: any;

  currentDmsData: any;
  selectedDmsIndex = null;

  coordinateParseError = false;
  coordinateValidationError = false;
  coorDmsList = [];
  airportList: any;

  loading = false;
  submitted = false;
  attachmentFailed:boolean = false;

  todayDate: string;

  // verticalStructures
  verticalStructuresDtoList: any = [];
  verticalStructuresDtoSelected: any = 'verticalStructuresDtoSelected';
  onChange() {
    console.log(this.verticalStructuresDtoSelected);
    if (this.verticalStructuresDtoSelected && this.verticalStructuresDtoSelected.geo && this.verticalStructuresDtoSelected.geo.coordinates) {
      var temp = {
        longitude: this.verticalStructuresDtoSelected.geo.coordinates[0],
        latitude: this.verticalStructuresDtoSelected.geo.coordinates[1]
      }
      this.requestData.elevation = this.verticalStructuresDtoSelected.elevation;
      this.requestData.horizontalAccuracy = this.verticalStructuresDtoSelected.horizontalAccuracy;
      this.requestData.verticalAccuracy = this.verticalStructuresDtoSelected.verticalAccuracy;
      this.requestData.height = 0;


      this.currentDmsData = this.projService.googleToDms(temp);
      this.coorDmsList = [];
      this.requestData.coordinates = [];
      this.saveCoordinate();
      this.showCoordinate(null);
    }
  }
  verticalStructuresDto() {
    this.verticalStructuresDtoList = _config.verticalStructuresDtoList;
    // console.log(this.verticalStructuresDtoList);
    // localStorage.removeItem('verticalStructuresDtoList1');
    // if (localStorage.getItem('verticalStructuresDtoList')) {
    //   // console.log('verticalStructuresDtoList 1')
    //   this.verticalStructuresDtoList = JSON.parse(localStorage.getItem('verticalStructuresDtoList'));
    // } else {
    //   // console.log('verticalStructuresDtoList 2 ')
    //   this.requestService.verticalStructuresDto().pipe(first()).subscribe((response: any) => {
    //     response.forEach((elem) => {
    //       if (elem.geo && elem.geo.type === "Point") {
    //         this.verticalStructuresDtoList.push(elem);
    //       }
    //     })
    //     this.publicService.sortByKey(this.verticalStructuresDtoList, 'name');
    //     localStorage.setItem('verticalStructuresDtoList', JSON.stringify(this.verticalStructuresDtoList));
    //   });
    // }
    // console.log(this.verticalStructuresDtoList)
  }
  // verticalStructures

  ngOnInit() {
    this.reset();
    this.verticalStructuresDto();
    this.getAirportList()


    this.todayDate = this.publicService.getDayStrFromToday();
    console.log(this.todayDate);

    this.activatedRoute.params.subscribe(params => {
      console.log(params);
      if (params != undefined && params.id > 0) {
        this.requestService.getRequest(params.id, true).pipe(first()).subscribe((response) => {
          console.log(response);
          if (response) {
            this.requestData = response;
            if (this.requestData.geometryType === 'Polygon') {
              this.requestData.coordinates.pop();
            }
            this.coordinatesApply(this.requestData.coordinates);
          }
        });
      }
    });
  }

  coordinatesApply(coordinates) {
    this.coorDmsList = [];
    for (let i = 0; i < coordinates.length; i++) {
      if (coordinates[i] && coordinates[i].longitude && coordinates[i].latitude) {
        this.coorDmsList.push(this.projService.googleToDms(coordinates[i]))
        // console.log(this.coorDmsList);
      }
    }
  }

  newCoordinate() {
    this.currentDmsData = {
      latitude: {
        degree: "43",
        minute: "23",
        second: "24",
        direction: 'N'
      },
      longitude: {
        degree: "77",
        minute: "6",
        second: "53",
        direction: 'E'
      }
    }
  }
  cancelCoordinate() {
    this.currentDmsData = null;
    this.selectedDmsIndex = null;
  }

  selectCoordinate(index) {
    this.selectedDmsIndex = index;
    this.currentDmsData = JSON.parse(JSON.stringify(this.coorDmsList[index]));
  }

  saveCoordinate() {
    this.coordinateValidationError = false;
    // console.log(this.currentDmsData);
    var decimalCoordinate = [this.projService.dmsToGoogle(this.currentDmsData.longitude), this.projService.dmsToGoogle(this.currentDmsData.latitude)];
    // console.log(decimalCoordinate);
    if (this.projService.isGoogleCoor(decimalCoordinate)) {
      // console.log("isGoogleCoor True");
      var dataCoordinate = {
        longitude: decimalCoordinate[0],
        latitude: decimalCoordinate[1],
        z: 0
      }
      // console.log(dataCoordinate);
      if (this.selectedDmsIndex != null && this.selectedDmsIndex >= 0) {
        this.coorDmsList[this.selectedDmsIndex] = JSON.parse(JSON.stringify(this.currentDmsData));
        this.requestData.coordinates[this.selectedDmsIndex] = dataCoordinate;
      } else {
        this.coorDmsList.push(this.currentDmsData);
        this.requestData.coordinates.push(dataCoordinate)
      }
      this.selectedDmsIndex = null;
      this.currentDmsData = null;
    } else {
      this.coordinateParseError = true;
      console.log('Coordinate error');
    }
    // console.log(this.coorDmsList);
    // console.log(this.requestData.coordinates);
  }

  showCoordinate(index) {
    // console.log(index);
    // console.log(this.coorDmsList);
    // console.log(this.requestData.coordinates);
    var mapFeatureCoordinates = [];
    var extraPoint = null;
    for (let i = 0; i < this.requestData.coordinates.length; i++) {
      // console.log(this.requestData.coordinates[i]);
      mapFeatureCoordinates.push([this.requestData.coordinates[i].longitude, this.requestData.coordinates[i].latitude]);
      if (index == i) {
        extraPoint = [this.requestData.coordinates[i].longitude, this.requestData.coordinates[i].latitude];
      }
    }
    this.mapService.makeFeature(mapFeatureCoordinates, this.requestData.geometryType, extraPoint);
  }

  removeCoordinate(index) {
    this.coorDmsList.splice(index, 1);
    this.requestData.coordinates.splice(index, 1);
  }

  save(form, form2) {
    console.log(this.requestData);
    this.coordinateValidationError = false;
    console.log(form.invalid, form2.invalid);
    this.submitted = true;

    if (form.invalid || form2.invalid) {
      return;
    }

    if (!((this.requestData.geometryType === 'Point' && this.requestData.coordinates.length === 1) || (this.requestData.geometryType === 'LineString' && this.requestData.coordinates.length > 1) || (this.requestData.geometryType === 'Polygon' && this.requestData.coordinates.length > 2))) {
      this.coordinateValidationError = true;
      return;
    }

    if(this.requestData.attachment===null){
      this.attachmentFailed = true;
    }

    if (this.requestData.geometryType === 'Polygon') {
      this.requestData.coordinates.push(this.requestData.coordinates[0])
    }

    this.loading = true;
    this.requestService.saveRequest(this.requestData)
      .pipe(first())
      .subscribe(
        data => {
          this.loading = false;
          console.log(data);
          this.publicService.alertMsg("Request sent", 1500);
          this.router.navigate(['requests']);
        },
        error => {
          console.log(error);
        })
  }

  reset() {
    this.requestData = {
      "geometryType": "Point",//LineString, Polygon
      "coordinates": [],
      "type": "NewConstruction",
      "duration": "Permanent",
      "obstructionType": "Building",
      "beginningDate": this.publicService.getDayStrFromToday(5),
      "endDate": this.publicService.getDayStrFromToday(15),
      "elevation": 600,
      "height": null,
      "verticalAccuracy": null,//3
      "horizontalAccuracy": null,//5
      "designator": null,
      "airportName": "",
      "airportId": "",
      "attachment":null
    }
  }

  airportOnChange(val) {
    let airport = this.airportList.find(x => x.identifier === val);
    if (airport) {
      this.requestData.airportName = airport.name;
    }
  }
  getAirportList() {
    this.requestService.getAirportData()
      .subscribe(
        data => {
          this.airportList = data;
          // name
          this.publicService.sortByKey(this.airportList, 'name');
        },
        error => {
          console.log(error);
        });
  }

  previewFile(e) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    var reader = new FileReader();
    if (file) {
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.requestData.attachment = reader.result;
        // this.requestData.attachment = this.requestData.attachment.split(',')[1];
        // console.log(this.requestData.attachment);
      };
    }
  }

}