import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

import IResetPasswordRequest from 'src/app/core/interfaces/auth/reset-password-request.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';
import LoaderService from 'src/app/core/services/loader.service';


@Component({
    templateUrl: './forgot-password-dialog.component.html',
    styleUrls: ['./forgot-password-dialog.component.scss']
})
export default class ForgotPasswordDialogComponent {

    public constructor(
        private readonly dialogRef: MatDialogRef<ForgotPasswordDialogComponent>,
        private readonly authService: AuthenticationService,
        private readonly loader: LoaderService
    ) { }

    public userEmailFormControl = new FormControl('', [
        Validators.required,
        Validators.email
    ]);

    public userFormGroup = new FormGroup({
        email: this.userEmailFormControl,
    });


    public resetPassword(): void {
        this.authService.resetPassword(this.userFormGroup.value as IResetPasswordRequest, this.discard.bind(this));
    }

    public discard(): void {
        this.dialogRef.close(undefined);
    }

    public getErrorMessage(): string {
        if (this.userEmailFormControl.errors?.['required']) {
            return $localize`Email is required`;
        }
        if (this.userEmailFormControl.errors?.['email']) {
            return $localize`Invalid email`;
        }


        return '';
    }
}
