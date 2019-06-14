import { Component, OnInit } from '@angular/core';
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview';

@Component({
  selector: 'app-map-filter',
  templateUrl: './map-filter.component.html',
  styleUrls: ['./map-filter.component.css']
})
export class MapFilterComponent implements OnInit {
  collapseIconRotate= false;
  dropdownEnabled = true;
  items: TreeviewItem[];
  values: number[];
  config = TreeviewConfig.create({
    hasAllCheckBox: false,
    hasFilter: true,
    hasCollapseExpand: false,
    decoupleChildFromParent: false,
    maxHeight: 400,
  });

  constructor() { }

  ngOnInit() {
    this.items = this.getBooks();
  }

  onFilterChange(value: string) {
    console.log('filter:', value);
  }

  getBooks(): TreeviewItem[] {
    const vsCategory = new TreeviewItem({
      text: 'Airport', value: '1', children: [
        {
          text: 'Vertical Structure', value: '1.1', children: [
            { text: 'Point', value: '1.1.1' },
            { text: 'Line', value: '1.1.2' },
            { text: 'Polygon', value: '1.1.3' }
          ]
        },
        {
          text: 'RunWay', value: '1.2', children: [
            {
              text: 'RunWay Direction 1', collapsed: true, value: '1.2.1', children: [{
                text: 'Area 1.1', value: '1.2.1.1'
              }, {
                text: 'Area 1.2', value: '1.2.1.2'
              }]
            },
            {
              text: 'RunWay Direction 2', collapsed: true, value: '1.2.2', children: [{
                text: 'Area 2.1', value: '1.2.2.1'
              }, {
                text: 'Area 2.2', value: '1.2.2.2'
              }]
            }
          ]
        }
      ]
    });
    return [vsCategory];
  }
}
