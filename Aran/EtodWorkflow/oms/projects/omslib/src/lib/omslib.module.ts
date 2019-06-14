import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { OmslibComponent } from './omslib.component';
import { UrlNotFoundComponent } from './url-not-found/url-not-found.component';
import { SlotfilterPipe } from './_pipes/slotfilter.pipe';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { RecoveryComponent } from './recovery/recovery.component';
import { CountrycodePipe } from './_pipes/countrycode.pipe';
import { KeysPipe } from './_pipes/keys.pipe';
import { UserFormComponent } from './user-form/user-form.component';
import { NgxMaskModule } from 'ngx-mask';
import { ArraySortPipe } from './_pipes/array-sort.pipe';
import { GridComponent } from './grid/grid.component';
import { SimpleSelectorComponent } from './simple-selector/simple-selector.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    NgxMaskModule.forRoot(),
  ],
  declarations: [
    SlotfilterPipe,
    CountrycodePipe,
    KeysPipe,
    ArraySortPipe,
    OmslibComponent,
    UrlNotFoundComponent,
    ResetPasswordComponent,
    RecoveryComponent,
    UserFormComponent,
    GridComponent,
    SimpleSelectorComponent
  ],
  exports: [
    SlotfilterPipe,
    CountrycodePipe,
    KeysPipe,
    ArraySortPipe,
    OmslibComponent,
    UrlNotFoundComponent,
    ResetPasswordComponent,
    RecoveryComponent,
    UserFormComponent,
    GridComponent,
    SimpleSelectorComponent
  ]
})
export class OmslibModule { }
