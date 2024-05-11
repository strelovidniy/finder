import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import ISignupRequest from 'src/app/core/interfaces/auth/signup-request.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';
import LoaderService from 'src/app/core/services/loader.service';
import NotifierService from 'src/app/core/services/notifier.service';

import FormValidators from 'src/app/shared/validators/form.validator';


@Component({
    templateUrl: './sign-up.component.html',
    styleUrls: ['./sign-up.component.scss', './sign-up-responsive.component.scss']
})
export default class SignUpComponent {
    public passwordVisibility: boolean = false;
    public confirmPasswordVisibility: boolean = false;

    public termsAndConditions: boolean = false;


    constructor(
        private readonly router: Router,
        private readonly authService: AuthenticationService,
        private readonly loader: LoaderService,
        private readonly notifier: NotifierService
    ) {
    }

    public passwordFormControl = new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        Validators.required,
        // check whether the entered password has a number
        FormValidators.patternValidator(/\d/, { hasNumber: true }),
        // check whether the entered password has upper case letter
        FormValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        // check whether the entered password has a lower case letter
        FormValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
    ]);

    public emailFormControl = new FormControl('', [
        Validators.required,
        Validators.email
    ]);

    public confirmPasswordFormControl = new FormControl('', [
        Validators.required,
        FormValidators.matchValidator('password', 'confirmPassword'),
    ]);

    public logIn(): void {
        this.router.navigate(['/auth/login']);
    }

    public firstNameFormControl = new FormControl('', [
        Validators.required,
        FormValidators.cannotContainSpace
    ]);

    public lastNameFormControl = new FormControl('', [
        Validators.required,
        FormValidators.cannotContainSpace
    ]);

    public signUpFormGroup = new FormGroup({
        password: this.passwordFormControl,
        confirmPassword: this.confirmPasswordFormControl,
        email: this.emailFormControl,
        firstName: this.firstNameFormControl,
        lastName: this.lastNameFormControl
    });

    public get errorMessage(): string {
        return this.signUpFormGroup.valid ? '' : $localize`Something wrong with one or more fields`;
    }

    public signUp(): void {
        if (!this.signUpFormGroup.valid) {
            this.signUpFormGroup.markAllAsTouched();

            return;
        }

        this.authService.signUp(
            { ...this.signUpFormGroup.value } as ISignupRequest,
            (): void => {
                localStorage.removeItem('credentialId');
                this.notifier.success($localize`You have successfully registered. Please check your email to confirm your account`);
                this.router.navigate(['/login']);
            }
        );
    }

    public get btnIsDisabled(): boolean {
        return !this.signUpFormGroup.valid;
    }
}
