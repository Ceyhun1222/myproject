import { GridDataSource, GridRow, GridFieldLink, GridFieldSimple, GridFieldBadge, GridConfig, GridColumn, GridSortingMode, GridAction, GridFieldLabel } from "projects/omslib/src/lib/grid/grid";

export class RequestsGrid {
    public config: GridConfig = null;
    public dataSource: GridDataSource = null;

    constructor() {
        this.init();
    }

    private init(): void {
        let gridConfig = new GridConfig();
        gridConfig.columns.push(new GridColumn('userFullname', 'User name', true, false, false, '*'));
        gridConfig.columns.push(new GridColumn('designator', 'Designator', true, true, false, '*'));
        gridConfig.columns.push(new GridColumn('airportName', 'Airport', true, true, false, '*'));
        gridConfig.columns.push(new GridColumn('elevation', 'Elevation', true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('height', 'Height', true, true, true, '70px'));
        gridConfig.columns.push(new GridColumn('horizontalAccuracy', 'Horizontal accuracy', true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('verticalAccuracy', 'Vertical accuracy', true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('beginningDate', 'Beginning date', false, true, true, '*'));
        gridConfig.columns.push(new GridColumn('endDate', 'End Date', false, true, true, '*'));
        gridConfig.columns.push(new GridColumn('duration', 'Duration', false, true, true, '*'));
        gridConfig.columns.push(new GridColumn('type', 'Type', false, false, true, '*'));
        gridConfig.columns.push(new GridColumn('geometryType', 'Geometry type', false, false, true, '*'));
        gridConfig.columns.push(new GridColumn('createdAt', 'Created at', true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('obstructionType', 'Obstruction type', false, false, true, '*'));
        gridConfig.columns.push(new GridColumn('submitted2Aixm', 'Submitted to Aixm', true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('checked', 'Checked', true, true, true, '*'));

        gridConfig.sorting.column = 'designator';
        gridConfig.sorting.mode = GridSortingMode.DESC;

        gridConfig.pagination.page = 1;
        gridConfig.pagination.perPage = 10;

        gridConfig.actions.push(
            new GridAction('view', 'View request', 'icon-file-eye'),
            // new GridAction('submit2Aixm', 'Submit to Aixm', 'icon-file-upload'),
            new GridAction('delete', 'Delete request', 'icon-trash')
        );

        this.config = gridConfig;
    }

    public convert(resellerRequests: any[], totalCount: number): void {
        let dataSource = new GridDataSource();
        dataSource.totalCount = totalCount;

        for (let operation of resellerRequests) {
            let row = new GridRow();
            row.pureObject = operation;


            let typeClass: string = null;
            switch (operation.type) {
                case 'NewConstruction':
                    typeClass = 'badge-success';
                    break;
                case 'Alteration':
                    typeClass = 'badge-warning';
                    break;
                case 'Existing':
                    typeClass = 'badge-info';
                    break;
            }

            let submittedClass: string = null;
            switch (operation.submitted2Aixm) {
                case true:
                    submittedClass = 'badge-success';
                    break;
                case false:
                    submittedClass = 'badge-warning';
                    break;
            }

            let checkedClass: string = null;
            switch (operation.checked) {
                case true:
                    checkedClass = 'badge-success';
                    break;
                case false:
                    checkedClass = 'badge-warning';
                    break;
            }

            let userFullname = new GridFieldLink(this.config.getColumn('userFullname'), operation.userFullname, `/user/${operation.userId}`, true, true);
            let designator = new GridFieldSimple(this.config.getColumn('designator'), operation.designator);
            let airportName = new GridFieldSimple(this.config.getColumn('airportName'), operation.airportName);
            let elevation = new GridFieldSimple(this.config.getColumn('elevation'), operation.elevation);
            let height = new GridFieldSimple(this.config.getColumn('height'), operation.height);
            let horizontalAccuracy = new GridFieldSimple(this.config.getColumn('horizontalAccuracy'), operation.horizontalAccuracy);
            let verticalAccuracy = new GridFieldSimple(this.config.getColumn('verticalAccuracy'), operation.verticalAccuracy);
            let beginningDate = new GridFieldSimple(this.config.getColumn('beginningDate'), operation.createdAt);
            let endDate = new GridFieldSimple(this.config.getColumn('endDate'), operation.endDate);
            let duration = new GridFieldSimple(this.config.getColumn('duration'), operation.duration);
            let type = new GridFieldLabel(this.config.getColumn('type'), operation.type, typeClass);
            let geometryType = new GridFieldSimple(this.config.getColumn('geometryType'), operation.geometryType);
            let createdAt = new GridFieldSimple(this.config.getColumn('createdAt'), operation.createdAt);
            let obstructionType = new GridFieldSimple(this.config.getColumn('obstructionType'), operation.obstructionType);
            let submitted2Aixm = new GridFieldLabel(this.config.getColumn('submitted2Aixm'), operation.submitted2Aixm ? 'Yes':'No', submittedClass);
            let checked = new GridFieldLabel(this.config.getColumn('checked'), operation.checked ? 'Yes':'No', checkedClass);
            
            row.push([userFullname, designator, airportName, elevation, height, horizontalAccuracy, verticalAccuracy, beginningDate, endDate, duration, type, geometryType, createdAt, obstructionType, submitted2Aixm, checked]);//details
            dataSource.push(row);
        }

        this.dataSource = dataSource;
    }

    temp = [
        {
            "checked": true,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": true,
            "submitted2AixmAt": "2018-12-27",
            "submitted2AixmPrivateSlotName": "kaz",
            "submitted2AixmPublicSlotName": "etod",
            "submitted": true,
            "createdAt": "2018-12-26",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.34861666666667,
                    "longitude": 77.03154444444445
                }
            ],
            "attachment": null,
            "id": 77,
            "type": "NewConstruction",
            "designator": "TRANS",
            "duration": "Permanent",
            "obstructionType": "Other",
            "beginningDate": "2018-12-31",
            "endDate": "2019-01-10",
            "elevation": 676.958,
            "height": 0,
            "verticalAccuracy": 3,
            "horizontalAccuracy": 5,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        },
        {
            "checked": true,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2018-12-26",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.36816666666667,
                    "longitude": 77.0718
                }
            ],
            "attachment": null,
            "id": 78,
            "type": "NewConstruction",
            "designator": "designator",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2018-12-31",
            "endDate": "2019-01-10",
            "elevation": 679.656,
            "height": 0,
            "verticalAccuracy": 3,
            "horizontalAccuracy": 5,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        },
        {
            "checked": true,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2018-12-27",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.34375277777778,
                    "longitude": 77.01793611111111
                }
            ],
            "attachment": null,
            "id": 79,
            "type": "NewConstruction",
            "designator": "designator",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2019-01-01",
            "endDate": "2019-01-11",
            "elevation": 682.197,
            "height": 0,
            "verticalAccuracy": 0.037,
            "horizontalAccuracy": 0.039,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        },
        {
            "checked": true,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2019-01-03",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.39,
                    "longitude": 77.11472222222221
                }
            ],
            "attachment": null,
            "id": 80,
            "type": "NewConstruction",
            "designator": "designator",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2019-01-08",
            "endDate": "2019-01-18",
            "elevation": 600,
            "height": 190,
            "verticalAccuracy": 2,
            "horizontalAccuracy": 3,
            "airportName": "ASTANA",
            "airportId": "b7388eab-40d8-46ac-840d-0cd8d35fd0ee"
        },
        {
            "checked": false,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2019-01-07",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.36816666666667,
                    "longitude": 77.0718
                }
            ],
            "attachment": null,
            "id": 81,
            "type": "NewConstruction",
            "designator": "designator",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2019-01-12",
            "endDate": "2019-01-22",
            "elevation": 679.656,
            "height": 0,
            "verticalAccuracy": 0.037,
            "horizontalAccuracy": 0.039,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        },
        {
            "checked": false,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2019-01-07",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.36816666666667,
                    "longitude": 77.0718
                }
            ],
            "attachment": null,
            "id": 82,
            "type": "NewConstruction",
            "designator": "designator",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2019-01-12",
            "endDate": "2019-01-22",
            "elevation": 679.656,
            "height": 0,
            "verticalAccuracy": 0.037,
            "horizontalAccuracy": 0.039,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        },
        {
            "checked": false,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2019-01-07",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.25801666666667,
                    "longitude": 76.93891111111111
                }
            ],
            "attachment": null,
            "id": 83,
            "type": "NewConstruction",
            "designator": "designator",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2019-01-12",
            "endDate": "2019-01-22",
            "elevation": 829.48,
            "height": 0,
            "verticalAccuracy": 0.101,
            "horizontalAccuracy": 0.188,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        },
        {
            "checked": false,
            "userFullname": "Ilyas Gojayev",
            "userId": 15,
            "submitted2Aixm": false,
            "submitted2AixmAt": "0001-01-01",
            "submitted2AixmPrivateSlotName": null,
            "submitted2AixmPublicSlotName": null,
            "submitted": true,
            "createdAt": "2019-01-07",
            "geometryType": "Point",
            "coordinates": [
                {
                    "latitude": 43.338433333333334,
                    "longitude": 77.010125
                }
            ],
            "attachment": null,
            "id": 84,
            "type": "NewConstruction",
            "designator": "veer",
            "duration": "Permanent",
            "obstructionType": "Building",
            "beginningDate": "2019-01-12",
            "endDate": "2019-01-22",
            "elevation": 686.496,
            "height": 0,
            "verticalAccuracy": 0.037,
            "horizontalAccuracy": 0.039,
            "airportName": "ALMATY",
            "airportId": "96ce7c65-8b2b-4552-830c-fa076f78d267"
        }
    ]
}