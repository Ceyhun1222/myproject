export class TempData {
    static tempList: any = [
        {
          "identifier": "8fff21f6-6888-47e8-a4ad-0bd798dc1de8",
          "name": "TENGIZ",
          "designator": "UATZ"
        },
        {
          "identifier": "96ce7c65-8b2b-4552-830c-fa076f78d267",
          "name": "ALMATY",
          "designator": "UAAA"
        },
        {
          "identifier": "a942bb79-dd24-4dcd-85cb-e9dc342931f8",
          "name": "KOKSHETAU",
          "designator": "UACK"
        },
        {
          "identifier": "ae2dd085-81bc-4192-83a0-b2bba997123d",
          "name": "TARAZ",
          "designator": "UADD"
        },
        {
          "identifier": "b490fa58-0201-48cc-961c-e9d6e63df5e1",
          "name": "BORALDAY",
          "designator": "UAAR"
        },
        {
          "identifier": "b7388eab-40d8-46ac-840d-0cd8d35fd0ee",
          "name": "ASTANA",
          "designator": "UACC"
        },
        {
          "identifier": "68abb479-8c18-4229-9b58-3a7700a33916",
          "name": "ATYRAU",
          "designator": "UATG"
        },
        {
          "identifier": "7003abe2-d61a-46f0-8e48-9bd9eef1a6c4",
          "name": "TALDYKORGAN",
          "designator": "UAAT"
        },
        {
          "identifier": "767f61b4-c4e5-48b9-913d-a54d906d9df9",
          "name": "PAVLODAR",
          "designator": "UASP"
        },
        {
          "identifier": "7c36386c-e321-4ae7-86ae-9e7268d64f05",
          "name": "SHYMKENT",
          "designator": "UAII"
        },
        {
          "identifier": "82c984ff-31d6-42d8-bc7d-55f32799e9d4",
          "name": "SEMEY",
          "designator": "UASS"
        },
        {
          "identifier": "8ff4eaec-5b3b-4f1d-b6f1-951026f3a31f",
          "name": "BALKHASH",
          "designator": "UAAH"
        },
        {
          "identifier": "c0896636-64a0-4273-a8a6-08f465a64e70",
          "name": "UST-KAMENOGORSK",
          "designator": "UASK"
        },
        {
          "identifier": "c3d612cb-fd8c-4433-b230-9047a48b77e9",
          "name": "KOSTANAY",
          "designator": "UAUU"
        },
        {
          "identifier": "e9335fa4-f8eb-42e3-a514-1ad545033571",
          "name": "AKTOBE",
          "designator": "UATT"
        },
        {
          "identifier": "ef3236c7-1cad-4506-828d-71a7f20b3a9d",
          "name": "KYZYLORDA",
          "designator": "UAOO"
        },
        {
          "identifier": "f59040e9-c91b-4ed6-8372-7c25ff173ba8",
          "name": "URDZHAR",
          "designator": "UASU"
        },
        {
          "identifier": "fc0948f8-ddab-4e3b-b825-f9b129543f62",
          "name": "PETROPAVLOVSK",
          "designator": "UACP"
        },
        {
          "identifier": "1400ece1-4b2c-4e68-ae5f-f353c954df90",
          "name": "USHARAL",
          "designator": "UAAL"
        },
        {
          "identifier": "14ded8b2-4d09-4fbf-8290-5914a1a5298e",
          "name": "URALSK",
          "designator": "UARR"
        },
        {
          "identifier": "1d759b58-eedb-4688-bf96-5ab056c61655",
          "name": "ZHEZKAZGAN",
          "designator": "UAKD"
        },
        {
          "identifier": "27b53447-a161-496b-93a5-e6be9547f36e",
          "name": "KARAGANDA",
          "designator": "UAKK"
        },
        {
          "identifier": "417f895a-1cd0-4975-ae63-47ed6fd275a1",
          "name": "ZAISAN",
          "designator": "UASZ"
        },
        {
          "identifier": "50a9ba1e-a434-44ae-b1be-d22e6dd5c142",
          "name": "AKTAU",
          "designator": "UATE"
        }
      ];
}
export class GridConfig {
    columns: GridColumn[] = [];
    pagination: GridPagination = new GridPagination();
    sorting: GridSorting = new GridSorting();
    actions: GridAction[] = [];
    bordered: boolean = true;

    getColumn(alias: string): GridColumn {
        for (let column of this.columns) {
            if (column.alias == alias) return column;
        }

        return null;
    }
}

export class GridAction {
    constructor(public alias: string,
        public title: string,
        public iconClass:string
    ) { }
}

export class GridSorting {
    mode: GridSortingMode = GridSortingMode.ASC;
    column: string = null;
    enabled: boolean = true;
}

export enum GridSortingMode {
    ASC,
    DESC
}

export class GridColumn {
    public sortingClass = null;

    constructor(public alias: string,
        public title: string,
        public visibility: boolean,
        public sortable: boolean,
        public selectable: boolean,
        public width: string,
        public iconClass: string = null
    ) { }
}

export class GridDataSource {
    rows: GridRow[] = [];
    totalCount: number = 0;

    push(row: GridRow) {
        this.rows.push(row);
    }
}

export class GridRow {
    fields: GridField[] = [];
    pureObject = null;

    push(fields: GridField[]) {
        for (let field of fields) {
            this.fields.push(field);
        }
    }
}

export abstract class GridField {
    type: string = GridFieldType.SIMPLE;
    column: GridColumn = null;

    constructor(column: GridColumn, type: string) {
        this.column = column;
        this.type = type;
    }
}

export class GridFieldType {
    static SIMPLE: string = 'SIMPLE';
    static LINK: string = 'LINK';
    static LABEL: string = 'LABEL';
    static BADGE: string = 'BADGE';
}

export class GridFieldSimple extends GridField {
    value: string = null;
    constructor(column: GridColumn, value: string) {
        super(column, GridFieldType.SIMPLE);
        this.value = value;
    }
}

export class GridFieldLabel extends GridField {
    value: string = null;
    className: string = null;
    constructor(column: GridColumn, value: string, className: string) {
        super(column, GridFieldType.LABEL);
        this.value = value;
        this.className = 'badge ' + className;
    }
}

export class GridFieldBadge extends GridField {
    value: string = null;
    className: string = null;
    constructor(column: GridColumn, value: string, className: string) {
        super(column, GridFieldType.BADGE);
        this.value = value;
        this.className = className;
    }
}

export class GridFieldLink extends GridField {
    value: string = null;
    url: string = null;
    isRouterLink: boolean = true;
    isBold: boolean = true;

    constructor(column: GridColumn, value: string, url: string, isRouterLink: boolean, isBold: boolean) {
        super(column, GridFieldType.LINK);
        this.value = value;
        this.url = url;
        this.isRouterLink = isRouterLink;
        this.isBold = isBold;
    }
}

export class GridPagination {
    page: number = 1;
    perPage: number = 10;
    totalCount: number = 0;
    totalPage = 0;
    start = 0;
    end = 0;
    pages: string[] = [];
    enabled: boolean = true;
}

export class RequestFilter {
    page: number = 1;
    perPage: number = 10;
    sortMode:string = null;
    sortField:string = null;
    filter:string = null;
    aerodromeId:string = null;
    userId:number = null;
    query:string = null;
    constructor(page: number = 1, perPage: number = 10, sortMode = null, sortField:string = null, filter:string = null, aerodromeId:string = null, userId:number = null, query:string = null) {
        this.page = page;
        this.perPage = perPage;
        this.sortMode = sortMode;
        this.sortField = sortField;
        this.filter = filter;
        this.aerodromeId = aerodromeId;
        this.userId = userId;
        this.query = query;

    }
}