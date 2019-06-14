import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../_services';
import { first } from 'rxjs/operators';
import { ProjService } from 'projects/omslib/src/lib/_services/map/proj.service';
import { MapService } from '../map/map.service';
import { PublicService } from 'projects/omslib/src/lib/_services/public.service';
import { RequestService } from 'projects/omslib/src/lib/_services/request.service';
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview';
import { StyleFilter } from 'projects/omslib/src/lib/_services/map/style-filter';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.css']
})
export class RequestComponent implements OnInit {
  collapseIconRotate: boolean = false;
  collapseIconRotateLayers: boolean = true;
  collapseIconRotateStyle: boolean = true;
  collapseIconRotateResult: boolean = false;
  requestData: any;
  resultList: any = [];
  resultShow: boolean = false;

  checkLoading: boolean = false;
  attachmentLoading: boolean = false;
  requestLoading: boolean = false;

  attachmentStr: string = null;

  reportTreeItems: TreeviewItem[];
  dropdownEnabled = true;
  values: number[];
  config = TreeviewConfig.create({
    hasAllCheckBox: false,
    hasFilter: false,
    hasCollapseExpand: false,
    decoupleChildFromParent: false,
    maxHeight: 1600,
  });

  submit2AixmLoading: boolean = false;

  constructor(private activatedRoute: ActivatedRoute, private requestService: RequestService, public projService: ProjService, private mapService: MapService, private router: Router, public publicService: PublicService, private settingsService: SettingsService) { }

  ngOnInit() {
    this.reset();

    this.activatedRoute.params.subscribe(params => {
      console.log(params);
      if (params != undefined && params.id > 0) {
        this.requestLoading = true;
        this.requestService.getRequest(params.id).pipe(first()).subscribe((response) => {
          console.log(response);
          if (response) {
            this.requestLoading = false;
            this.requestData = response;
            if (response['attachment'] && response['attachment']['length'] > 0) {
              this.attachmentStr = response['attachment'].toString();
            }
            this.getAttachment(response['id']);

            if (response['id'] > 0 && response['checked']) {
              this.getReportTree(response['id']);
              this.mapService.loadOmstacleAreaLayer(response['id']);
              this.mapService.loadVsLayer(response['id']);
              this.mapService.loadRequestLayer(response['id']);
              this.getReportData(response['id']);
            }
          }
        });
      }
    });
  }

  reset() {
    this.requestData = {
      "geometryType": "Point",//LineString, Polygon
      "coordinates": [],
      "type": "NewConstruction",
      "duration": "Permanent",
      "obstructionType": "Building",
      "beginningDate": "",
      "endDate": "",
      "elevation": null,
      "height": null,
      "verticalAccuracy": null,
      "horizontalAccuracy": null
    }
  }

  getReportTree(id) {
    this.requestService.getReportTree(id).pipe(first()).subscribe((response) => {
      if (response) {
        this.setReportTreeItems(response);
      }
    });
  }
  getReportData(id) {
    this.requestService.getReportData(id).pipe(first()).subscribe((response) => {
      if (response) {
        this.resultShow = true;
        this.resultList = response;
      }
    });
  }
  check() {
    if (this.requestData.id > 0) {
      this.checkLoading = true;
      this.requestService.checkRequest(this.requestData.id).pipe(first()).subscribe(response => {
        console.log(response);
        this.resultShow = true;
        if (response) {
          this.checkLoading = false;
          this.resultList = response;
          this.getReportTree(this.requestData['id']);
          this.mapService.loadOmstacleAreaLayer(this.requestData['id']);
          this.mapService.loadRequestLayer(this.requestData['id']);
          this.mapService.loadVsLayer(this.requestData['id']);

          this.requestData.checked = true;
        }
      },
        error => {
          console.log(error);
          this.checkLoading = false;
        });
    }
  }

  submit2Aixm() {
    this.submit2AixmLoading = true;
    this.settingsService.getSelectedSlot().pipe().subscribe(slot => {
      if (slot && slot['name'] && slot['private'] && slot['private']['name']) {
        if (confirm('Are you sure you want to submit?\nPublic slot:  ' + slot['name'] + '\nPrivate slot:  '+ slot['private']['name'])) {
          this.requestService.submit2Aixm(this.requestData.id).pipe(first()).subscribe((response) => {
            this.submit2AixmLoading = false;
            console.log(response);
            this.requestData.submitted2Aixm = true;
            this.publicService.alertMsg("Successfully submitted to " +slot['name'] +" slot", 3000);
            this.requestData.submitted2AixmAt = "Today"
            this.requestData.submitted2AixmPrivateSlotName = slot['private']['name'];
            this.requestData.submitted2AixmPublicSlotName = slot['name'];
          });
        } else {
          this.submit2AixmLoading = false;
        }
      }
    });
  }

  setReportTreeItems(data) {
    console.log(data);
    var objParent: any = { text: data.airportName, value: 'AirportName', children: [] };
    if (data.runways && data.runways.length > 0) {
      data.runways.forEach((runway, index) => {
        var objRunway: any = { text: runway.name, value: "runway_" + index, children: [] };
        if (runway && runway.runwayDirections && runway.runwayDirections.length > 0) {
          runway.runwayDirections.forEach((dir, index) => {
            var objDir = { text: dir.name, value: "runwayDirection_" + (index + 1), collapsed: true, children: [] }
            if (dir && dir.obstacleAreas && dir.obstacleAreas.length > 0) {
              dir.obstacleAreas.forEach((area) => {
                var objArea = { text: area.name.split("OTHER_").pop(), value: area.id, checked: area.isChecked }
                objDir.children.push(objArea);
              });
            }
            objRunway.children.push(objDir);
          });
        }
        objParent.children.push(objRunway);
      });
    }

    if (data.obstacleAreas && data.obstacleAreas.length > 0) {
      data.obstacleAreas.forEach((obstacleArea, index) => {
        objParent.children.push({ text: obstacleArea.name.split("OTHER_").pop(), value: obstacleArea.id, checked: obstacleArea.isChecked });
      })
    }

    var objVS: any = {
      text: 'Vertical structure', value: 'verticalStructure', children: [
        { text: 'Point', value: 'Point', checked: false },
        // { text: 'LineString', value: 'LineString', checked: false },
        { text: 'Line', value: 'MultiLineString', checked: false },
        // { text: 'Polygon', value: 'Polygon', checked: false },
        { text: 'Polygon', value: 'MultiPolygon', checked: false }
      ]
    }
    var objRequest: any = {
      text: 'Request feature', value: 'r-e-q-u-e-s-t-f-e-a-t-u-r-e', checked: true
    }
    objParent.children.push(objVS);
    objParent.children.push(objRequest);
    this.reportTreeItems = [new TreeviewItem(objParent)];
  }

  onFilterChange(value: string) {
    console.log('filter:', value);
    StyleFilter.filter = value;
    // StyleFilter.filter.push("6b282c7d-67a7-410f-913d-d7aa2236077e");
    this.mapService.redrawOmstacleLayers();
  }

  onFilterChange2(value: string) {
    console.log('filter2:', value);
    if (StyleFilter.selectedArea != value) {
      StyleFilter.selectedArea = value;
    } else {
      StyleFilter.selectedArea = "";
    }
    this.mapService.redrawOmstacleLayers();
  }

  getAttachment(id) {
    this.attachmentLoading = true;
    this.requestService.getAttachment(id).pipe(first()).subscribe(base64Str => {
      if (base64Str && base64Str['length'] > 0) {
        this.attachmentLoading = false;
        this.attachmentStr = base64Str.toString();
      }
    });
  }

  picDownload(id, filename = "request-image") {
    if (this.attachmentStr && this.attachmentStr.length > 0) {
      let byteCharacters = atob(this.attachmentStr.toString().split(',')[1]);

      let byteNumbers = new Array(byteCharacters.length);
      for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      let byteArray = new Uint8Array(byteNumbers);

      let blob = new Blob([byteArray], { "type": "image/jpeg" });

      if (navigator.msSaveBlob) {
        navigator.msSaveBlob(blob, filename);
      } else {
        let link = document.createElement("a");

        link.href = URL.createObjectURL(blob);

        link.setAttribute('visibility', 'hidden');
        link.download = 'picture';

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      }
    }
  }

  downloadReport(id, fileName) {
    var a = document.createElement("a");
    document.body.appendChild(a);
    // a.style = "display: none";

    this.requestService.downloadReport(id).subscribe(data => {
      console.log(data);
      var file = new Blob([data], { type: 'application/pdf' });
      var fileURL = URL.createObjectURL(file);
      a.href = fileURL;
      a.download = fileName;
      a.click();
      window.URL.revokeObjectURL(fileURL);
    });
  }
}