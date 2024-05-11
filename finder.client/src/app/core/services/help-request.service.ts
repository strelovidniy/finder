import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import EndpointService from './endpoint.service';
import IHelpRequest from '../interfaces/help-request/help-request.interface';
import ICreateHelpRequestRequest from '../interfaces/help-request/create-help-request-request.interface';
import IUpdateHelpRequestRequest from '../interfaces/help-request/update-help-request-request.interface';
import IPagedCollectionView from '../interfaces/system/paged-collection-view.interface';


@Injectable({
    providedIn: 'root'
})
export default class HelpRequestService {
    public constructor(
        private readonly http: HttpClient,
        private readonly endpointService: EndpointService
    ) { }

    public getHelpRequests(query: string, callback: (helpRequests: IPagedCollectionView<IHelpRequest>) => void, errorCallback: () => void): void {
        this.http.get<IPagedCollectionView<IHelpRequest>>(this.endpointService.getHelpRequests(query)).subscribe({
            next: (response: IPagedCollectionView<IHelpRequest>): void => {
                response.items.forEach((item: IHelpRequest): void => {
                    item.deadline = !!item.deadline ? new Date(Date.parse(item.deadline.toString())) : undefined;
                });

                callback(response);
            },
            error: (): void => {
                errorCallback();
            }
        });
    }

    public createHelpRequest(request: ICreateHelpRequestRequest, callback?: () => void, errorCallback?: () => void): void {
        const formData = new FormData();

        formData.append('title', request.title);
        formData.append('description', request.description);

        if (request.tags?.length) {
            formData.append('tags', request.tags.join(','));
        }

        if (request.deadline) {
            formData.append('deadline', request.deadline.toISOString());
        }

        if (request.latitude) {
            formData.append('latitude', request.latitude.toString());
        }

        if (request.longitude) {
            formData.append('longitude', request.longitude.toString());
        }

        formData.append('showContactInfo', request.showContactInfo.toString());

        if (request.images?.length) {
            for (let i = 0; i < request.images.length; i++) {
                formData.append('images', request.images[i]);
            }
        }

        this.http.post(this.endpointService.createHelpRequest(), formData).subscribe({
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

    public updateHelpRequest(request: IUpdateHelpRequestRequest, callback?: () => void, errorCallback?: () => void): void {
        const formData = new FormData();

        formData.append('id', request.id);
        formData.append('title', request.title);
        formData.append('description', request.description);

        if (request.tags?.length) {
            formData.append('tags', request.tags.join(','));
        }

        if (request.deadline) {
            formData.append('deadline', new Date(Date.parse(request.deadline as any)).toISOString());
        }

        if (request.latitude) {
            formData.append('latitude', request.latitude.toString());
        }

        if (request.longitude) {
            formData.append('longitude', request.longitude.toString());
        }

        formData.append('showContactInfo', request.showContactInfo.toString());

        if (request.images?.length) {
            for (let i = 0; i < request.images.length; i++) {
                formData.append('images', request.images[i]);
            }
        }

        if (request.imagesToDelete?.length) {

            for (let i = 0; i < request.imagesToDelete.length; i++) {
                formData.append(`imagesToDelete[${i}]`, request.imagesToDelete[i]);
            }
        }

        this.http.put(this.endpointService.updateHelpRequest(), formData).subscribe({
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

    public deleteHelpRequest(id: string, callback?: () => void, errorCallback?: () => void): void {
        this.http.delete(this.endpointService.deleteHelpRequest(id)).subscribe({
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

    public getHelpRequest(id: string, callback: (helpRequest: IHelpRequest) => void, errorCallback?: () => void): void {
        this.http.get<IHelpRequest>(this.endpointService.getHelpRequest(id)).subscribe({
            next: (response: IHelpRequest): void => {
                callback(response);
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }
}
