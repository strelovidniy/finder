import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

import LoaderService from 'src/app/core/services/loader.service';
import AuthenticationService from 'src/app/core/services/authentication.service';

import FormValidators from '../../../validators/form.validator';

import IChangePasswordRequest from 'src/app/core/interfaces/auth/change-password-request.interface';


@Component({
    templateUrl: './change-password-dialog.component.html',
    styleUrls: ['./change-password-dialog.component.scss']
})
export default class ChangePasswordDialogComponent {


    public oldPasswordVisibility: boolean = false;
    public newPasswordVisibility: boolean = false;
    public confirmPasswordVisibility: boolean = false;

    constructor(
        private readonly dialogRef: MatDialogRef<ChangePasswordDialogComponent>,
        private readonly authService: AuthenticationService,
        private readonly loader: LoaderService
    ) { }

    public get errorMessage(): string {

        return this.passwordFormGroup.valid ? '' : $localize`Something\'s wrong with one or more fields`;
    }

    public oldPasswordFormControl = new FormControl('', [
        Validators.required,
    ]);

    public newPasswordFormControl = new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        // check whether the entered password has a number
        FormValidators.patternValidator(/\d/, { hasNumber: true }),
        // check whether the entered password has upper case letter
        FormValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        // check whether the entered password has a lower case letter
        FormValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
    ]);

    public confirmNewPasswordFormControl = new FormControl('', Validators.compose([
        Validators.required,
        FormValidators.matchValidator('newPassword', 'confirmNewPassword')
    ]));

    public passwordFormGroup = new FormGroup({
        oldPassword: this.oldPasswordFormControl,
        newPassword: this.newPasswordFormControl,
        confirmNewPassword: this.confirmNewPasswordFormControl
    });

    public changePassword(): void {
        this.authService.changePassword(this.passwordFormGroup.value as IChangePasswordRequest, this.discard.bind(this));
    }

    public discard(): void {
        this.dialogRef.close(undefined);
    }
}
