import { GridDataSource, GridRow, GridFieldLink, GridFieldSimple, GridFieldBadge, GridConfig, GridColumn, GridSortingMode, GridAction, GridFieldLabel } from "projects/omslib/src/lib/grid/grid";
import { LangService } from "projects/omslib/src/lib/_services/lang/langService";

export class RequestsGrid {
    public config: GridConfig = null;
    public dataSource: GridDataSource = null;

    constructor(private lang: LangService) {
        this.init();
    }

    private init(): void {
        let gridConfig = new GridConfig();
        gridConfig.columns.push(new GridColumn('designator', this.lang.get('Designator'), true, true, false, '*'));
        gridConfig.columns.push(new GridColumn('airportName', this.lang.get('Airport'), true, true, false, '*'));
        gridConfig.columns.push(new GridColumn('elevation', this.lang.get('Elevation'), true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('height', this.lang.get('Height'), true, true, true, '70px'));
        gridConfig.columns.push(new GridColumn('horizontalAccuracy', this.lang.get('Horizontal accuracy'), true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('verticalAccuracy', this.lang.get('Vertical accuracy'), true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('beginningDate', this.lang.get('Beginning date'), false, true, true, '*'));
        gridConfig.columns.push(new GridColumn('endDate', this.lang.get('End date'), false, true, true, '*'));
        gridConfig.columns.push(new GridColumn('duration', this.lang.get('Duration'), false, true, true, '*'));
        gridConfig.columns.push(new GridColumn('geometryType', this.lang.get('Geometry type'), false, false, true, '*'));
        gridConfig.columns.push(new GridColumn('createdAt', this.lang.get('Created at'), true, true, true, '*'));
        gridConfig.columns.push(new GridColumn('obstructionType', this.lang.get('Obstruction type'), false, false, true, '*'));
        gridConfig.columns.push(new GridColumn('type', this.lang.get('Type'), false, false, true, '*'));
        gridConfig.columns.push(new GridColumn('submitted', this.lang.get('Submitted'), true, false, true, '*'));

        gridConfig.sorting.column = 'designator';
        gridConfig.sorting.mode = GridSortingMode.DESC;

        gridConfig.pagination.page = 1;
        gridConfig.pagination.perPage = 10;

        gridConfig.actions.push(
            new GridAction('view', this.lang.get('View'), 'icon-file-eye'),
            new GridAction('submit', this.lang.get('Submit'), 'icon-checkmark-circle2'),
            new GridAction('delete', this.lang.get('Delete'), 'icon-trash')
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
            switch (operation.submitted) {
                case true:
                    submittedClass = 'badge-success';
                    break;
                case false:
                    submittedClass = 'badge-warning';
                    break;
            }

            let designator = new GridFieldSimple(this.config.getColumn('designator'), operation.designator);
            let airportName = new GridFieldSimple(this.config.getColumn('airportName'), operation.airportName);
            let elevation = new GridFieldSimple(this.config.getColumn('elevation'), operation.elevation);
            let height = new GridFieldSimple(this.config.getColumn('height'), operation.height);
            let horizontalAccuracy = new GridFieldSimple(this.config.getColumn('horizontalAccuracy'), operation.horizontalAccuracy);
            let verticalAccuracy = new GridFieldSimple(this.config.getColumn('verticalAccuracy'), operation.verticalAccuracy);
            let beginningDate = new GridFieldSimple(this.config.getColumn('beginningDate'), operation.createdAt);
            let endDate = new GridFieldSimple(this.config.getColumn('endDate'), operation.endDate);
            let duration = new GridFieldSimple(this.config.getColumn('duration'), this.lang.get(operation.duration));

            
            let geometryType = new GridFieldSimple(this.config.getColumn('geometryType'), this.lang.get(operation.geometryType));
            let createdAt = new GridFieldSimple(this.config.getColumn('createdAt'), operation.createdAt);
            let obstructionType = new GridFieldSimple(this.config.getColumn('obstructionType'), this.lang.get(operation.obstructionType));
            
            let type = new GridFieldLabel(this.config.getColumn('type'), this.lang.get(operation.type), typeClass);
            let submitted = new GridFieldLabel(this.config.getColumn('submitted'), operation.submitted ? 'Yes':'No', submittedClass);
            
            row.push([designator, airportName, elevation, height, horizontalAccuracy, verticalAccuracy, beginningDate, endDate, duration, geometryType, createdAt, obstructionType,type, submitted]);//details

            dataSource.push(row);
        }

        this.dataSource = dataSource;
    }
}