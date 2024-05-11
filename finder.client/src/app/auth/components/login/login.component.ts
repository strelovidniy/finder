import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import IAuthRequest from 'src/app/core/interfaces/auth/auth-request.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';
import LoaderService from 'src/app/core/services/loader.service';
import NotifierService from 'src/app/core/services/notifier.service';

import ForgotPasswordDialogComponent from '../forgot-password-dialog/forgot-password-dialog.component';
import IUserMe from 'src/app/core/interfaces/auth/user-me.interface';


@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss', './login-responsive.component.scss']
})
export default class LoginComponent implements OnInit, OnDestroy {
    public form: FormGroup = {} as any;
    public passwordVisibility: boolean = false;
    public rememberMe: boolean = false;

    private loginSubscription?: Subscription;

    constructor(
        private readonly router: Router,
        private readonly authSevice: AuthenticationService,
        private readonly dialog: MatDialog
    ) { }


    public ngOnInit(): void {
        this.form = new FormGroup({
            email: new FormControl(null, [
                Validators.required,
                Validators.email
            ]),
            password: new FormControl(null, [
                Validators.required,
                Validators.minLength(6)
            ]),
            rememberMe: new FormControl(false, [
                Validators.required
            ])
        });
    }

    public ngOnDestroy(): void {
        this.loginSubscription?.unsubscribe();
    }

    public forgotPassword(): void {
        this.dialog.open(ForgotPasswordDialogComponent, {
            width: '500px',
            disableClose: true
        });
    }

    public async onSubmit(): Promise<void> {
        if (this.form.invalid) {
            return;
        }

        const data: IAuthRequest = this.form.value as IAuthRequest;

        this.authSevice.login(data).subscribe((res: IUserMe): void => {
            this.router.navigate(['/requests']);
        });
    }
}
