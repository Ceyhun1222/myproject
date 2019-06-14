import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'sort'
})

export class ArraySortPipe  implements PipeTransform {
    transform(array: any[], field: string): any[] {
        if (!Array.isArray(array)) {
            return;
        }
        array.sort(function(a,b){return b[field] - a[field]})
        return array;
    }
}