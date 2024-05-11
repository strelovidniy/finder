import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import ISetNewPasswordRequest from 'src/app/core/interfaces/auth/set-new-password-request.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';
import LoaderService from 'src/app/core/services/loader.service';

import FormValidators from 'src/app/shared/validators/form.validator';


@Component({
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss', './reset-password-responsive.component.scss']
})
export default class ResetPasswordComponent implements OnInit, OnDestroy {

    private verificationCode: string = '';
    public newPasswordVisibility: boolean = false;
    public confirmPasswordVisibility: boolean = false;

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
        newPassword: this.newPasswordFormControl,
        confirmNewPassword: this.confirmNewPasswordFormControl
    });

    private queryParamsSubscription?: Subscription;

    constructor(
        private readonly activatedRoute: ActivatedRoute,
        private readonly router: Router,
        private readonly location: Location,
        private readonly authenticationService: AuthenticationService,
        private readonly loader: LoaderService
    ) { }

    public ngOnInit(): void {
        this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe((params: { vc?: string }): void => {
            this.verificationCode = params.vc || '';
            this.location.replaceState('/auth/create-new-password');
            if (!this.verificationCode) {
                // this.router.navigate(['/auth/login']);
            }
        });
    }

    public ngOnDestroy(): void {
        this.queryParamsSubscription?.unsubscribe();
    }

    public resetPassword(): void {
        this.newPasswordFormControl.markAsTouched();
        this.confirmNewPasswordFormControl.markAsTouched();
        if (this.passwordFormGroup.valid) {
            const data = {
                verificationCode: this.verificationCode,
                ...this.passwordFormGroup.value
            };

            this.authenticationService.setNewPassword(data as ISetNewPasswordRequest, this.redirect.bind(this));
        }
    }

    private redirect(): void {
        this.router.navigate(['/login']);
    }

    public get btnIsDisabled(): boolean {
        return this.passwordFormGroup.invalid && (this.newPasswordFormControl.touched || this.confirmNewPasswordFormControl.touched);
    }
}
