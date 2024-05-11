import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { SwPush } from '@angular/service-worker';
import * as lodash from 'lodash';

import NotifierService from '../core/services/notifier.service';
import LoaderService from '../core/services/loader.service';
import AuthenticationService from '../core/services/authentication.service';

import INotificationsConfig from '../core/interfaces/notifications-config/notifications-config.interface';
import IUpdateNotificationsConfigRequest from '../core/interfaces/notifications-config/update-notifications-config-request.interface';
import NotificationsConfigService from '../core/services/notifications-config.service';

import environment from 'src/environments/environment';


@Component({
    templateUrl: './notifications.component.html',
    styleUrls: ['./notifications.component.scss', './notifications-responsive.component.scss']
})
export default class NotificationsComponent implements OnInit {

    public notificationsConfig: INotificationsConfig | IUpdateNotificationsConfigRequest = {
        enableNotifications: false,
        enableUpdateNotifications: false,
        enableTagFiltration: false,
        filterTags: [],
        enableTitleFiltration: false,
        filterTitles: []
    } as INotificationsConfig | IUpdateNotificationsConfigRequest;

    public filterTagsFormControl: FormControl = new FormControl('', [Validators.required]);
    public filterTitlesFormControl: FormControl = new FormControl('', [Validators.required]);

    constructor(
        private readonly notifier: NotifierService,
        private readonly loader: LoaderService,
        private readonly authService: AuthenticationService,
        private readonly notificationConfigService: NotificationsConfigService,
        private readonly swPush: SwPush
    ) { }

    public ngOnInit(): void {
        if (this.authService.currentUser.notificationsConfig) {
            this.notificationsConfig = JSON.parse(JSON.stringify(this.authService.currentUser.notificationsConfig)) as INotificationsConfig;

            if (this.notificationsConfig.filterTags) {
                this.filterTagsFormControl.setValue(this.notificationsConfig.filterTags.join(', '));
            }

            if (this.notificationsConfig.filterTitles) {
                this.filterTitlesFormControl.setValue(this.notificationsConfig.filterTitles.join(', '));
            }
        }
    }

    public setTags(event: Event): void {
        const target = event.target as HTMLInputElement;

        this.notificationsConfig.filterTags = target.value.split(',').map((tag): string => tag.trim());
    }

    public setTitles(event: Event): void {
        const target = event.target as HTMLInputElement;

        this.notificationsConfig.filterTitles = target.value.split(',').map((title): string => title.trim());
    }

    public isButtonDisabled(): boolean {
        return this.notificationsConfig.enableNotifications === this.authService.currentUser.notificationsConfig?.enableNotifications
            && this.notificationsConfig.enableUpdateNotifications === this.authService.currentUser.notificationsConfig?.enableUpdateNotifications
            && this.notificationsConfig.enableTagFiltration === this.authService.currentUser.notificationsConfig?.enableTagFiltration
            && lodash.isEqual(this.notificationsConfig.filterTags, this.authService.currentUser.notificationsConfig?.filterTags)
            && this.notificationsConfig.enableTitleFiltration === this.authService.currentUser.notificationsConfig?.enableTitleFiltration
            && lodash.isEqual(this.notificationsConfig.filterTitles, this.authService.currentUser.notificationsConfig?.filterTitles);
    }

    public save(): void {
        this.loader.show();

        if (this.filterTagsFormControl.invalid && this.notificationsConfig.enableTagFiltration) {
            this.filterTagsFormControl.markAsTouched();

            this.notifier.error($localize`Tags are required`);

            this.loader.hide();

            return;
        }

        if (this.filterTitlesFormControl.invalid && this.notificationsConfig.enableTitleFiltration) {
            this.filterTitlesFormControl.markAsTouched();

            this.notifier.error($localize`Titles are required`);

            this.loader.hide();

            return;
        }

        if (this.notificationsConfig.enableNotifications) {

            this.swPush.requestSubscription({
                serverPublicKey: environment.serverPublicKey,
            }).then((pushSubscription): Promise<void> => this.authService.addUserPushSubscription(pushSubscription).toPromise())
                .then((): void => {
                    this.notifier.success($localize`You have successfully subscribed to push notifications`);
                })
                .catch((error): void => {
                    this.notifier.error($localize`An error occurred while subscribing to push notifications`);

                    console.error($localize`Failed to subscribe to push notifications`);
                    console.error(error);
                });
        }

        if (!this.notificationsConfig.enableNotifications) {
            this.notificationsConfig.enableUpdateNotifications = false;
            this.notificationsConfig.enableTagFiltration = false;
            this.notificationsConfig.enableTitleFiltration = false;
            this.notificationsConfig.filterTags = undefined;
            this.notificationsConfig.filterTitles = undefined;

            this.filterTagsFormControl.setValue('');
            this.filterTitlesFormControl.setValue('');
        } else {
            if (!this.notificationsConfig.enableTagFiltration) {
                this.filterTagsFormControl.setValue('');

                this.notificationsConfig.filterTags = undefined;
            }

            if (!this.notificationsConfig.enableTitleFiltration) {
                this.filterTitlesFormControl.setValue('');

                this.notificationsConfig.filterTitles = undefined;
            }

            if (!this.notificationsConfig.enableTagFiltration && !this.notificationsConfig.enableTitleFiltration) {
                this.notificationsConfig.enableUpdateNotifications = false;
            }
        }

        this.notificationConfigService.updateNotificationsConfig(
            this.notificationsConfig as IUpdateNotificationsConfigRequest,
            (): void => {
                this.loader.hide();

                this.authService.currentUser.notificationsConfig = JSON.parse(JSON.stringify(this.notificationsConfig)) as INotificationsConfig;

                this.notifier.success($localize`Your notifications settings have been updated`);
            },
            (): void => {
                this.loader.hide();

                this.notifier.error($localize`An error occurred while updating your notifications settings`);
            }
        );
    }
}
