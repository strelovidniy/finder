import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs/internal/operators/filter';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/internal/Subscription';

import NotifierService from '../core/services/notifier.service';
import LoaderService from '../core/services/loader.service';
import AuthenticationService from '../core/services/authentication.service';
import ImageService from '../core/services/placeholder.service';
import EventBusService from '../core/services/event-bus.service';
import UserDetailsService from '../core/services/user-details.service';

import ImageCropperDialogComponent from '../shared/components/dialogs/image-cropper-dialog/image-cropper-dialog.component';

import IUserMe from '../core/interfaces/auth/user-me.interface';
import IUpdateAddressRequest from '../core/interfaces/user/update-address-request.interface';
import IUpdateContactDetailsRequest from '../core/interfaces/user/update-contact-details-request.interface';


@Component({
    templateUrl: './account-preferences.component.html',
    styleUrls: ['./account-preferences.component.scss', './account-preferences-responsive.component.scss']
})
export default class AccountPreferencesComponent implements OnInit, OnDestroy {
    public user?: IUserMe;

    public addressForm: FormGroup = {} as FormGroup;
    public contactDetailsForm: FormGroup = {} as FormGroup;

    public addressLine1FormControl = new FormControl('', [
        Validators.maxLength(200),
        Validators.required,
    ]);

    public addressLine2FormControl = new FormControl('', [
        Validators.maxLength(200),
    ]);

    public cityFormControl = new FormControl('', [
        Validators.maxLength(200),
        Validators.required,
    ]);

    public stateFormControl = new FormControl('', [
        Validators.maxLength(200),
        Validators.required,
    ]);

    public postalCodeFormControl = new FormControl('', [
        Validators.maxLength(200),
        Validators.required,
    ]);

    public countryFormControl = new FormControl('', [
        Validators.maxLength(200),
        Validators.required,
    ]);

    public phoneNumberFormControl = new FormControl('', [
        Validators.maxLength(100),
        Validators.pattern(/\+?([\d|\(][\s|\(\d{3}\)|\.|\-|\d]{4,}\d)/)
    ]);

    public linkedInFormControl = new FormControl('', [
        Validators.maxLength(200),
    ]);

    public instagramFormControl = new FormControl('', [
        Validators.maxLength(200),
    ]);

    public skypeFormControl = new FormControl('', [
        Validators.maxLength(200),
    ]);

    public telegramFormControl = new FormControl('', [
        Validators.maxLength(200),
    ]);

    public otherFormControl = new FormControl('', [
        Validators.maxLength(2000),
    ]);

    private changeUiSubscription?: Subscription;

    public get defaultImageUrl(): string {
        return this.imageService.defaultImageUrl;
    }

    private dialogSubscription?: Subscription;

    constructor(
        private readonly notifier: NotifierService,
        private readonly loader: LoaderService,
        private readonly authService: AuthenticationService,
        private readonly dialog: MatDialog,
        private readonly userDetailsService: UserDetailsService,
        private readonly imageService: ImageService,
        private readonly eventBus: EventBusService
    ) { }

    public ngOnInit(): void {
        this.changeUiSubscription = this.eventBus.refreshUiSubject.subscribe({
            next: (): void => {
                this.user = this.authService.currentUser;
            }
        });

        this.user = this.authService.currentUser;

        this.addressLine1FormControl.setValue(this.user.details?.address?.addressLine1 || null);
        this.addressLine2FormControl.setValue(this.user.details?.address?.addressLine2 || null);
        this.cityFormControl.setValue(this.user.details?.address?.city || null);
        this.stateFormControl.setValue(this.user.details?.address?.state || null);
        this.postalCodeFormControl.setValue(this.user.details?.address?.postalCode || null);
        this.countryFormControl.setValue(this.user.details?.address?.country || null);

        this.instagramFormControl.setValue(this.user.details?.contactInfo?.instagram || null);
        this.linkedInFormControl.setValue(this.user.details?.contactInfo?.linkedIn || null);
        this.phoneNumberFormControl.setValue(this.user.details?.contactInfo?.phoneNumber || null);
        this.skypeFormControl.setValue(this.user.details?.contactInfo?.skype || null);
        this.telegramFormControl.setValue(this.user.details?.contactInfo?.telegram || null);
        this.otherFormControl.setValue(this.user.details?.contactInfo?.other || null);

        this.contactDetailsForm = new FormGroup({
            phoneNumber: this.phoneNumberFormControl,
            linkedIn: this.linkedInFormControl,
            instagram: this.instagramFormControl,
            skype: this.skypeFormControl,
            telegram: this.telegramFormControl,
            other: this.otherFormControl,
        });

        this.addressForm = new FormGroup({
            addressLine1: this.addressLine1FormControl,
            addressLine2: this.addressLine2FormControl,
            city: this.cityFormControl,
            state: this.stateFormControl,
            postalCode: this.postalCodeFormControl,
            country: this.countryFormControl,
        });
    }

    public ngOnDestroy(): void {
        this.changeUiSubscription?.unsubscribe();
        this.dialogSubscription?.unsubscribe();
    }

    public updateUserDetails(): void {
        if (this.addressForm.valid) {
            this.userDetailsService.updateAddress(
                this.addressForm.value as IUpdateAddressRequest,
                (): void => {
                    this.authService.getUserInfo();
                    this.notifier.success($localize`Address updated successfully`);
                    this.loader.hide();
                },
                (): void => {
                    this.notifier.error($localize`Address update failed`);
                    this.loader.hide();
                });
        } else {
            this.addressForm.markAllAsTouched();
            this.notifier.warning($localize`Address is invalid and not updated`);
        }
        if (this.contactDetailsForm.valid) {
            this.userDetailsService.updateContactDetails(
                this.contactDetailsForm.value as IUpdateContactDetailsRequest,
                (): void => {
                    this.authService.getUserInfo();
                    this.notifier.success($localize`Contact details updated successfully`);
                    this.loader.hide();
                },
                (): void => {
                    this.notifier.error($localize`Contact details update failed`);
                    this.loader.hide();
                });
        } else {
            this.contactDetailsForm.markAllAsTouched();
            this.notifier.warning($localize`Contact details is invalid and not updated`);
        }
    }

    public isUpdateButtonDisabled(): boolean {
        if (this.addressForm.invalid && this.contactDetailsForm.invalid) {
            return true;
        }

        if ((this.addressForm.value.addressLine1 || '') === (this.user?.details?.address?.addressLine1 || '')
            && (this.addressForm.value.addressLine2 || '') === (this.user?.details?.address?.addressLine2 || '')
            && (this.addressForm.value.city || '') === (this.user?.details?.address?.city || '')
            && (this.addressForm.value.state || '') === (this.user?.details?.address?.state || '')
            && (this.addressForm.value.postalCode || '') === (this.user?.details?.address?.postalCode || '')
            && (this.addressForm.value.country || '') === (this.user?.details?.address?.country || '')
            && (this.contactDetailsForm.value.phoneNumber || '') === (this.user?.details?.contactInfo?.phoneNumber || '')
            && (this.contactDetailsForm.value.linkedIn || '') === (this.user?.details?.contactInfo?.linkedIn || '')
            && (this.contactDetailsForm.value.instagram || '') === (this.user?.details?.contactInfo?.instagram || '')
            && (this.contactDetailsForm.value.skype || '') === (this.user?.details?.contactInfo?.skype || '')
            && (this.contactDetailsForm.value.telegram || '') === (this.user?.details?.contactInfo?.telegram || '')
            && (this.contactDetailsForm.value.other || '') === (this.user?.details?.contactInfo?.other || '')) {
            return true;
        }

        return false;
    }

    public openImageInput(): void {
        const input = document.createElement('input');

        input.type = 'file';
        input.accept = '.png,.jpeg,.jpg';
        input.id = 'userPictureInput';
        input.onchange = (event: Event): void => this.selectUserAvatar(event);
        input.click();
    }

    private removeImageInput(): void {
        const input = document.getElementById('userPictureInput');

        if (input) {
            input.remove();
        }
    }

    private selectUserAvatar(event: Event): void {
        if (!(event.target as HTMLInputElement)?.files?.[0]) {
            return;
        }
        const dialogRef = this.dialog.open(ImageCropperDialogComponent, {
            data: event,
            maxHeight: '90vh'
        });

        this.dialogSubscription = dialogRef.afterClosed()
            .pipe(filter((value: any): boolean => !!value))
            .subscribe((avatar: any): void => {
                this.uploadUserAvatar(avatar);
                this.removeImageInput();
            });
    }

    private uploadUserAvatar(avatar: Blob): void {
        this.loader.show();

        const fileReader = new FileReader();

        fileReader.readAsDataURL(avatar);

        const formData = new FormData();

        formData.append('file', avatar);

        this.userDetailsService.uploadUserAvatar(
            formData,
            (): void => {
                this.authService.getUserInfo();
                this.notifier.success($localize`Profile image uploaded successfully`);
                this.loader.hide();
            },
            (): void => {
                this.notifier.error($localize`Profile image upload failed`);
                this.loader.hide();
            });
    }
}
