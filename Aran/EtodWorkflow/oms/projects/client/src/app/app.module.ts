import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { NgxMaskModule } from 'ngx-mask';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { OmslibModule } from 'projects/omslib/src/public_api';
import { routing } from './app.routing';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards';
import { ErrorInterceptor } from './_helpers/error.interceptor';
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import { AuthenticationService } from './_services';
import { MapComponent } from './map/map.component';
import { MapService } from './map/map.service';
import { RequestComponent } from './request/request.component';
import { RequestsComponent } from './requests/requests.component';
import { RequestService } from 'projects/omslib/src/lib/_services/request.service';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    RequestComponent,
    LoginComponent,
    MapComponent,
    RequestsComponent
  ],
  imports: [
    BrowserModule,
    OmslibModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    routing,
    SweetAlert2Module.forRoot({
      buttonsStyling: false,
      customClass: 'modal-content',
      confirmButtonClass: 'btn btn-sm btn-success',
      cancelButtonClass: 'btn btn-sm btn-defaault ml-2',
      animation:false
    }),
    NgxMaskModule.forRoot(),
    NgbModule
  ],
  providers: [AuthenticationService, AuthGuard, MapService, RequestService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
