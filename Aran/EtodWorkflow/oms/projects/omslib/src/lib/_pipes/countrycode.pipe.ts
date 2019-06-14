import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'countrycode'
})
export class CountrycodePipe implements PipeTransform {
    transform(items: any[], codeFilter: string): any[] {
        if (!items) return [];
        if (!codeFilter) return items;
        codeFilter = codeFilter.toLowerCase();
        return items.filter(it => {
            return it.name.toLowerCase().includes(codeFilter) | it.code.includes(codeFilter);
        });
    }
}