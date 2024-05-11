import { Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import AuthenticationService from 'src/app/core/services/authentication.service';
import NotifierService from 'src/app/core/services/notifier.service';

import IConfirmEmailRequest from 'src/app/core/interfaces/auth/confirm-email-request.interface';


@Component({
    templateUrl: './confirm-email.component.html',
    styleUrls: ['./confirm-email.component.scss', './confirm-email-responsive.component.scss']
})
export default class ConfirmEmailComponent implements OnInit, OnDestroy {
    public verificationCode: string = '';

    public success: boolean = false;
    public error: boolean = false;
    public loading: boolean = false;

    private queryParamsSubscription?: Subscription;

    constructor(
        private readonly activatedRoute: ActivatedRoute,
        private readonly router: Router,
        private readonly location: Location,
        private readonly authenticationService: AuthenticationService,
        private readonly notifier: NotifierService
    ) { }

    public ngOnInit(): void {
        this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe((params: { vc?: string }): void => {
            this.verificationCode = params.vc || '';
            this.location.replaceState('/auth/confirm-email');
            if (!this.verificationCode) {
                this.router.navigate(['/auth/login']);
            } else {
                this.confirmEmail();
            }
        });
    }

    public ngOnDestroy(): void {
        this.queryParamsSubscription?.unsubscribe();
    }

    public confirmEmail(): void {
        this.loading = true;
        this.authenticationService.confirmEmail(
            {
                registrationToken: this.verificationCode
            } as IConfirmEmailRequest,
            (): void => {
                this.notifier.success($localize`Email confirmed successfully`);
                this.success = true;
                setTimeout((): void => {
                    this.router.navigate(['/auth/login']);
                }, 3000);
            },
            (): void => {
                this.error = true;
                setTimeout((): void => {
                    this.router.navigate(['/auth/login']);
                }, 3000);
            });
    }
}
