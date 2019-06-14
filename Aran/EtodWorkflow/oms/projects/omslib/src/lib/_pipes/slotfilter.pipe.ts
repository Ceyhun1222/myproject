import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'slotfilter'
})
export class SlotfilterPipe implements PipeTransform {
    transform(items: any[], filter: string): any[] {
        if (!items) return [];
        if (!filter) return items;
        filter = filter.toLowerCase();
        return items.filter(it => {
            return it.name.toLowerCase().includes(filter) | it.status.toLowerCase().includes(filter);
        });
    }
}