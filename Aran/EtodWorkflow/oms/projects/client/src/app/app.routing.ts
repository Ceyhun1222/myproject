import { Routes, RouterModule } from '@angular/router';
import { RequestComponent } from './request/request.component';
import { UrlNotFoundComponent } from 'projects/omslib/src/lib/url-not-found/url-not-found.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_guards';
import { RequestsComponent } from './requests/requests.component';
import { ResetPasswordComponent } from 'projects/omslib/src/lib/reset-password/reset-password.component';
import { RecoveryComponent } from 'projects/omslib/src/lib/recovery/recovery.component';
import { UserFormComponent } from 'projects/omslib/src/lib/user-form/user-form.component';

const appRoutes: Routes = [
    { path: 'request', component: RequestComponent, canActivate: [AuthGuard]},
    { path: 'request/:id', component: RequestComponent, canActivate: [AuthGuard]},
    { path: 'requests', component: RequestsComponent, canActivate: [AuthGuard]},
    { path: 'login', component: LoginComponent},
    { path: 'signup', component: UserFormComponent},
    { path: 'myprofile', component: UserFormComponent},
    { path: 'recovery', component: RecoveryComponent },
    { path: 'reset-password/:code/:email', component: ResetPasswordComponent },
    // { path: 'user/:id', component: UserFormComponent, canActivate: [AuthGuard]},
    { path: '', redirectTo:'requests', pathMatch:'full' },
    { path: '**', component: UrlNotFoundComponent}
];

export const routing = RouterModule.forRoot(appRoutes, {onSameUrlNavigation:'reload'});