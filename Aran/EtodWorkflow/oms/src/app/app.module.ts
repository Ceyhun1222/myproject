import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { TreeviewModule } from 'ngx-treeview';
import {NgxMaskModule} from 'ngx-mask';
import { ColorPickerModule } from 'ngx-color-picker';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { ContentComponent } from './content/content.component';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component';

import { AuthGuard } from './_guards';
import { routing } from './app.routing';

import { JwtInterceptor, ErrorInterceptor, fakeBackendProvider } from './_helpers';
import { AuthenticationService, SettingsService } from './_services';
import { SidebarComponent } from './sidebar/sidebar.component';
import { MapComponent } from './map/map.component';

// import { UserFormComponent } from './user-form/user-form.component';
import { MapInfoComponent } from './map-info/map-info.component';
import { MapObstacleComponent } from './map-obstacle/map-obstacle.component';
import { DragDirective } from './_directives/drag.directive';
import { OmslibModule } from 'projects/omslib/src/public_api';
import { SettingsComponent } from './settings/settings.component';
import { MapFilterComponent } from './map-filter/map-filter.component';
import { RequestComponent } from './request/request.component';
import { RequestsComponent } from './requests/requests.component';
import { RequestService} from 'projects/omslib/src/lib/_services/request.service'
import { TooltipDirective } from './_directives/tooltip.directive';

@NgModule({
  declarations: [
    AppComponent,
    ContentComponent,
    HeaderComponent,
    LoginComponent,
    SidebarComponent,
    MapComponent,
    // UserFormComponent,
    MapInfoComponent,
    MapObstacleComponent,
    DragDirective,
    TooltipDirective,
    SettingsComponent,
    MapFilterComponent,
    RequestComponent,
    RequestsComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    routing,
    OmslibModule,
    SweetAlert2Module.forRoot({
      buttonsStyling: false,
      customClass: 'modal-content',
      confirmButtonClass: 'btn btn-sm btn-success',
      cancelButtonClass: 'btn btn-sm btn-defaault ml-2',
      animation:false
    }),
    TreeviewModule.forRoot(),
    NgxMaskModule.forRoot(),
    ColorPickerModule,
    NgbModule
  ],
  exports: [MapInfoComponent],
  providers: [
    AuthGuard,
    AuthenticationService,
    SettingsService,
    RequestService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },

    // provider used to create fake backend
    // fakeBackendProvider
  ],
  schemas:[CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule { }
