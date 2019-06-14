import { Routes, RouterModule } from '@angular/router';

import { ContentComponent } from './content/content.component';
import { LoginComponent } from './login/login.component';
import { MapComponent } from './map/map.component';
import { AuthGuard } from './_guards';
import { MapInfoComponent } from './map-info/map-info.component';
import { MapObstacleComponent } from './map-obstacle/map-obstacle.component';
import { SettingsComponent } from './settings/settings.component';
import { UrlNotFoundComponent } from 'projects/omslib/src/lib/url-not-found/url-not-found.component';
import { MapFilterComponent } from './map-filter/map-filter.component';
import { RequestComponent } from './request/request.component';
import { RequestsComponent } from './requests/requests.component';
import { ResetPasswordComponent } from 'projects/omslib/src/lib/reset-password/reset-password.component';
import { RecoveryComponent } from 'projects/omslib/src/lib/recovery/recovery.component';
// import { UserFormComponent } from './user-form/user-form.component';
import { UserFormComponent } from 'projects/omslib/src/lib/user-form/user-form.component';

const appRoutes: Routes = [
    { path: 'users/:status', component: ContentComponent, canActivate: [AuthGuard] },
    { path: 'users/:status/:time', component: ContentComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard] },
    { path: 'recovery', component: RecoveryComponent },
    { path: 'reset-password/:code/:email', component: ResetPasswordComponent },
    { path: 'map', component: MapComponent, canActivate: [AuthGuard], children:[
        {
            path:'info/:Name/:Designator', component:MapInfoComponent
        },{
            path:'obstacle', component:MapObstacleComponent
        },{
            path:'filter', component:MapFilterComponent
        }
    ] },
    { path: 'signup', component: UserFormComponent},
    { path: 'myprofile', component: UserFormComponent},
    // { path: 'user/:action/:id', component: UserFormComponent, canActivate: [AuthGuard]},
    // { path: 'user/:action', component: UserFormComponent, canActivate: [AuthGuard]},
    { path: 'user', component: UserFormComponent, canActivate: [AuthGuard]},
    { path: 'user/:id', component: UserFormComponent, canActivate: [AuthGuard]},
    { path: 'request/:id', component: RequestComponent, canActivate: [AuthGuard], children:[
        {
            path:'info/:Name/:Designator', component:MapInfoComponent
        },{
            path:'obstacle', component:MapObstacleComponent
        },{
            path:'filter', component:MapFilterComponent
        }
    ]},
    { path: 'requests', component: RequestsComponent, canActivate: [AuthGuard]},
    { path: 'requests/:time', component: RequestsComponent, canActivate: [AuthGuard]},
    { path: '', redirectTo:'requests', pathMatch:'full' },
    { path: '**', component: UrlNotFoundComponent, canActivate: [AuthGuard]}
    
];

export const routing = RouterModule.forRoot(appRoutes, {onSameUrlNavigation:'reload'});