import { NgModule } from '@angular/core';

import SharedModule from '../shared/shared.module';
import AuthRouterModule from './auth.router.module';

import LoginComponent from './components/login/login.component';
import ForgotPasswordDialogComponent from './components/forgot-password-dialog/forgot-password-dialog.component';
import SignUpComponent from './components/sign-up/sign-up.component';
import AuthLayoutComponent from './components/auth-layout/auth-layout.component';
import ResetPasswordComponent from './components/reset-password/reset-password.component';
import WelcomeComponent from './components/welcome/welcome.component';
import ConfirmEmailComponent from './components/confirm-email/confirm-email.component';


@NgModule({
    imports: [
        SharedModule,
        AuthRouterModule
    ],
    declarations: [
        WelcomeComponent,
        LoginComponent,
        ForgotPasswordDialogComponent,
        SignUpComponent,
        AuthLayoutComponent,
        ResetPasswordComponent,
        ConfirmEmailComponent,
    ],
})
export default class AuthModule { }
