import { Component, Input, Output, EventEmitter, OnInit, OnChanges, AfterContentInit, AfterContentChecked, Renderer, ElementRef } from '@angular/core';
import { SimpleSelectItem } from "./simple-select-item";

declare let $: any;

@Component({
    selector: 'simple-selector',
    template: `<select [disabled]="disabled" class="select">
                    <option *ngFor="let item of items" [selected]="item.selected ? true : null" value="{{item.alias}}">{{item.name}}</option>
              </select>`
})
export class SimpleSelectorComponent implements OnInit {
    // Declarations
    component: any = null;
    @Input() disabled: boolean;
    @Input() placeholder: string = '';
    @Input() isMultiple: boolean = false;
    @Output() callbackChange = new EventEmitter();

    items: SimpleSelectItem[];
    @Input('items')
    set setItem(items) {
        this.items = items;
        let proto = this;
        setTimeout(function () {
            proto.initComponent();
        }, 100)
        
    }

    // Constructor
    constructor(private element: ElementRef,
        private renderer: Renderer) { }

    // Implementations
    ngOnInit() {
        this.initComponent();
    }
    

    // Methods
    initComponent() {
        let proto = this;
        let elem = this.getElem();

        if (elem.select2() != null) {
            elem.select2("destroy");
            elem.unbind("change");
        }

        this.component = elem.select2({
            minimumResultsForSearch: Infinity,
            multiple: this.isMultiple,
            placeholder: this.placeholder
        }).on("change", function (e) {
            if(proto.isMultiple){
                let aliases = $(this).val();
                let selectedItems = proto.getSelectedItemsForMultiple(aliases);

                if (selectedItems != null) {
                    proto.callbackChange.emit(selectedItems);
                }
                
            }else{
                let alias = $(this).val();
                let item = proto.getAndSelectItem(alias);
    
                if (item != null) {
                    proto.callbackChange.emit(item);
                }
            }
        });
        

    }

    getElem(): any {
        var elem = $(this.element.nativeElement);
        return elem.find('select');
    }

    getAndSelectItem(alias: string) {
        let selectedItem = null;
        for (let item of this.items) {
            if (item.alias == alias) {
                item.selected = true;
                selectedItem = item;
            } else {
                if(!this.isMultiple) item.selected = false;
            }
        }

        return selectedItem;
    }

    getSelectedItemsForMultiple(aliases: any){
        let selectedItems = [];
        if (aliases != null) {
            for (let alias of aliases) {
                for (let item of this.items) {
                    if (item.alias == alias) {
                        selectedItems.push(item);
                    }
                }
            }
        }

        return selectedItems;
    }
    
}