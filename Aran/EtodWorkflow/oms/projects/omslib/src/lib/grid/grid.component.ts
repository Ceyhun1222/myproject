import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { TempData, GridConfig, GridSortingMode, GridColumn, GridRow, GridAction, GridDataSource } from './grid';
import { SimpleSelectItem } from '../simple-selector/simple-select-item';
import { Router } from '@angular/router';

@Component({
    selector: 'grid',
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

    tempData = TempData.tempList;
    config: GridConfig;
    dataSource: GridDataSource;
    langs: any = null;

    @Input('config')
    set setConfig(config) {
        console.log(config);
        this.config = config;
        this.refresh();
    }

    @Input('dataSource')
    set setDataSource(dataSource) {
        console.log(dataSource);
        this.dataSource = dataSource;
        this.refresh();
    }

    @Output() callbackSort = new EventEmitter();
    @Output() callbackPage = new EventEmitter();
    @Output() callbackPerPage = new EventEmitter();
    @Output() callbackAction = new EventEmitter();

    perPageItems: SimpleSelectItem[] = [];
    resultMessage = null;

    // Constructor
    constructor(private router:Router) { }

    // Implementations    
    ngOnInit() {
        this.refresh();
    }

    navigateTo(path){
          this.router.navigate([path]);

    }

    // Methods
    refresh() {
        // Set columns sorting classes
        this.refreshSortingClasses();

        // Refresh pagination
        this.refreshPagination();

        // Refresh per page items
        this.refreshPerPageItems();

        // Refresh result message
        this.refreshResultMessage();
    }

    refreshPerPageItems() {
        this.perPageItems = [
            new SimpleSelectItem('10', '10', this.config.pagination.perPage == 10),
            new SimpleSelectItem('25', '25', this.config.pagination.perPage == 25),
            new SimpleSelectItem('50', '50', this.config.pagination.perPage == 50),
            new SimpleSelectItem('75', '75', this.config.pagination.perPage == 75),
            new SimpleSelectItem('100', '100', this.config.pagination.perPage == 100)
        ];
    }

    refreshPagination() {
        this.config.pagination.totalCount = (this.dataSource) ? this.dataSource.totalCount : 0;

        if (this.config.pagination.totalCount == 0) return;

        let pages: string[] = [];
        let page: number = this.config.pagination.page;
        let perPage: number = this.config.pagination.perPage;
        let totalCount = this.config.pagination.totalCount;
        let totalPage: number = Math.ceil(totalCount / perPage);
        let start: number = (page - 1) * perPage + 1;
        let end: number = (page - 1) * perPage + perPage;

        if (end > totalCount) {
            end = totalCount;
        }

        var left = page - 1;
        var right = totalPage - page;

        if (left >= 3 && right >= 3) {
            pages.push('1');

            if (page - 3 != 1) {
                pages.push('...');
            }

            for (var i = page - 2; i <= page + 2; i++) {
                pages.push(i.toString());
            }

            if (page + 3 != totalPage) {
                pages.push('...');
            }

            pages.push(totalPage.toString());
        }
        else if (left <= 3 && right > 3) {
            for (var i = 1; i <= page + 2; i++) {
                pages.push(i.toString());
            }

            pages.push('...');
            pages.push(totalPage.toString());
        }
        else if (left > 3 && right <= 3) {
            pages.push('1');
            pages.push('...');

            for (var i = page - 2; i <= totalPage; i++) {
                pages.push(i.toString());
            }
        }
        else if (left <= 3 && right <= 3) {
            for (var i = 1; i <= totalPage; i++) {
                pages.push(i.toString());
            }
        }

        this.config.pagination.totalPage = totalPage;
        this.config.pagination.pages = pages;
        this.config.pagination.start = start;
        this.config.pagination.end = end;

    }

    refreshSortingClasses() {
        for (let column of this.config.columns) {
            if (!column.sortable) {
                column.sortingClass = null;
                continue;
            }

            if (column.alias == this.config.sorting.column) {
                column.sortingClass = (this.config.sorting.mode == GridSortingMode.ASC) ? 'sorting_asc' : 'sorting_desc';
                continue;
            }

            column.sortingClass = 'sorting';
        }
    }

    refreshResultMessage() {
        let resultMessage = "resultMessage";// this.langService.get('common', 'message_grid_showing_entries');
        resultMessage = resultMessage.replace("{0}", this.config.pagination.start.toString());
        resultMessage = resultMessage.replace("{1}", this.config.pagination.end.toString());
        resultMessage = resultMessage.replace("{2}", this.config.pagination.totalCount.toString());
        this.resultMessage = resultMessage;
    }

    changeColumnVisibility(column: GridColumn) {
        column.visibility = !column.visibility;
    }

    changeSorting(column: GridColumn) {
        if (!this.config.sorting.enabled || !column.sortable) return;

        if (column.alias == this.config.sorting.column) {
            this.config.sorting.mode = (this.config.sorting.mode == GridSortingMode.ASC) ? GridSortingMode.DESC : GridSortingMode.ASC;
        } else {
            this.config.sorting.column = column.alias;
            this.config.sorting.mode = GridSortingMode.ASC;
        }

        this.refreshSortingClasses();
        this.callbackSort.emit(this.config.sorting);
    }

    changePage(page: string) {
        if (page == '...') return;
        this.config.pagination.page = parseInt(page);
        this.refreshPagination();
        this.callbackPage.emit(this.config.pagination.page);
    }

    changePagePrevious() {
        if (this.config.pagination.page > 1) {
            this.changePage((this.config.pagination.page - 1).toString());
        }
    }

    changePageNext() {
        if (this.config.pagination.page < this.config.pagination.totalPage) {
            this.changePage((this.config.pagination.page + 1).toString());
        }
    }

    changeAction(action: GridAction, row: GridRow) {
        this.callbackAction.emit({
            alias: action.alias,
            row: row
        });
    }

    callbackChangePerPage(item: any) {//SimpleSelectItem
        for (let it of this.perPageItems) {
            it.selected = it.alias == item.alias
        }

        this.config.pagination.perPage = parseInt(item.alias);
        this.refreshPagination();
        this.callbackPerPage.emit(this.config.pagination.perPage);
    }

}
