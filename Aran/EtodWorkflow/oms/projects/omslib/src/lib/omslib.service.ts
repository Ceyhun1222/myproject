import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OmslibService {

  constructor() { }

  sayHello(str){
    return str;
  }
}
