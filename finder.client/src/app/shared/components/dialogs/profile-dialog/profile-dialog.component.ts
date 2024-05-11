import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

import IUserMe from 'src/app/core/interfaces/auth/user-me.interface';
import IUserUpdateRequest from 'src/app/core/interfaces/user/user-update-request.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';


@Component({
    templateUrl: './profile-dialog.component.html',
    styleUrls: ['./profile-dialog.component.scss']
})
export default class ProfileDialogComponent implements OnInit, OnDestroy {

    private userId: string = '';

    private currentUserSubscription?: Subscription;

    constructor(
        private readonly dialogRef: MatDialogRef<ProfileDialogComponent>,
        private readonly authService: AuthenticationService
    ) { }

    public get errorMessage(): string {

        return this.profileFormGroup.valid ? '' : $localize`Something\'s wrong with one or more fields`;
    }

    public emailFormControl = new FormControl({ value: '', disabled: true }, [
        Validators.required,
    ]);

    public firstNameFormControl = new FormControl('', [
        Validators.required,
    ]);

    public lastNameFormControl = new FormControl('', [
        Validators.required,
    ]);


    public commentNotificationsFormControl = new FormControl<boolean>(false, [
        Validators.required,
    ]);


    public profileFormGroup = new FormGroup({
        firstName: this.firstNameFormControl,
        lastName: this.lastNameFormControl,
        receiveCommentNotifications: this.commentNotificationsFormControl
    });


    public ngOnInit(): void {
        this.authService.getUserInfo();

        this.currentUserSubscription = this.authService.currentUserSubject.subscribe({
            next: (value: IUserMe): void => {
                this.firstNameFormControl.setValue(value.firstName);
                this.lastNameFormControl.setValue(value.lastName);
                this.emailFormControl.setValue(value.email);
                this.userId = value.id;
            }
        });
    }

    public ngOnDestroy(): void {
        this.currentUserSubscription?.unsubscribe();
    }

    public save(): void {
        this.authService.updateUserProfile(
            { id: this.userId, ...this.profileFormGroup.value } as IUserUpdateRequest,
            (): void => {
                this.authService.getUserInfo();
                this.dialogRef.close(undefined);
            });
    }

    public discard(): void {
        this.dialogRef.close(undefined);
    }
}
