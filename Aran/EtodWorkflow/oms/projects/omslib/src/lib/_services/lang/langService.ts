import { Injectable } from '@angular/core';
import { en } from './en';
import { ru } from './ru';


@Injectable({
    providedIn: 'root'
})
export class LangService {
    currentLang: string = "";//ru, en

    constructor() {
        if (!localStorage.getItem('currentLang')) {
            localStorage.setItem('currentLang', 'en');
        }
        this.currentLang = localStorage.getItem('currentLang');
        console.log(this.currentLang);
    }

    change(lang: string) {
        localStorage.setItem('currentLang', lang);
        this.currentLang = localStorage.getItem('currentLang');
        location.reload(true);
    }

    getLang() {
        switch (this.currentLang) {
            case 'en':
                return en.obj;
            case 'ru':
                return ru.obj;
        }
    }

    get(value:string):string{
        if(this.currentLang === 'en' && en.obj[value] && en.obj[value].length > 0){
            return en.obj[value];
        }
        if(this.currentLang === 'ru' && ru.obj[value] && ru.obj[value].length > 0){
            return ru.obj[value];
        }
        return value;
    }
}