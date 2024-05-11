import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import EndpointService from './endpoint.service';

import IUpdateAddressRequest from '../interfaces/user/update-address-request.interface';
import IUpdateContactDetailsRequest from '../interfaces/user/update-contact-details-request.interface';


@Injectable({
    providedIn: 'root'
})
export default class UserDetailsService {

    public constructor(
        private readonly http: HttpClient,
        private readonly endpointService: EndpointService
    ) { }


    public uploadUserAvatar(formData: FormData, callback?: () => void, errorCallback?: () => void): void {
        this.http.post(this.endpointService.uploadUserAvatar(), formData).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }

    public updateAddress(request: IUpdateAddressRequest, callback?: () => void, errorCallback?: () => void): void {
        this.http.put(this.endpointService.updateAddress(), request).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }

    public updateContactDetails(request: IUpdateContactDetailsRequest, callback?: () => void, errorCallback?: () => void): void {
        this.http.put(this.endpointService.updateContactDetails(), request).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}
