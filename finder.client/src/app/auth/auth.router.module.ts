import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import GuestGuard from '../core/guards/guest.guard';

import SharedModule from '../shared/shared.module';

import AuthLayoutComponent from './components/auth-layout/auth-layout.component';
import LoginComponent from './components/login/login.component';
import ResetPasswordComponent from './components/reset-password/reset-password.component';
import SignUpComponent from './components/sign-up/sign-up.component';
import ConfirmEmailComponent from './components/confirm-email/confirm-email.component';


@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild([{
            path: '',
            component: AuthLayoutComponent,
            children: [
                { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
                { path: 'sign-up', component: SignUpComponent, canActivate: [GuestGuard] },
                { path: 'create-new-password', component: ResetPasswordComponent, canActivate: [GuestGuard] },
                { path: 'confirm-email', component: ConfirmEmailComponent, canActivate: [GuestGuard] },
                { path: '', redirectTo: 'login', pathMatch: 'full' }
            ]
        }])
    ],
    exports: [
        RouterModule
    ],
    providers: [GuestGuard]
})
export default class AuthRouterModule { }
