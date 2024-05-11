import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import EndpointService from './endpoint.service';

import IUpdateNotificationsConfigRequest from '../interfaces/notifications-config/update-notifications-config-request.interface';


@Injectable({
    providedIn: 'root'
})
export default class NotificationsConfigService {

    public constructor(
        private readonly endpointService: EndpointService,
        private readonly http: HttpClient
    ) { }

    public updateNotificationsConfig(request: IUpdateNotificationsConfigRequest, callback?: () => void, errorCallback?: () => void): void {
        this.http.put(this.endpointService.updateNotificationsConfig(), request).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }
            },
            error: (error): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}
