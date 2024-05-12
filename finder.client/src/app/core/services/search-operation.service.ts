import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import ICreateSearchOperationRequest from '../interfaces/search-operation/create-search-operation-request.interface';
import IUpdateSearchOperationRequest from '../interfaces/search-operation/update-search-operation-request.interface';
import ISearchOperation from '../interfaces/search-operation/search-operation.interface';
import IPagedCollectionView from '../interfaces/system/paged-collection-view.interface';

import EndpointService from './endpoint.service';


@Injectable({
    providedIn: 'root'
})
export default class SearchOperationService {
    public constructor(
        private readonly http: HttpClient,
        private readonly endpointService: EndpointService
    ) { }

    public getSearchOperations(query: string, callback?: (helpRequests: IPagedCollectionView<ISearchOperation>) => void, errorCallback?: () => void): void {
        this.http.get<IPagedCollectionView<ISearchOperation>>(this.endpointService.getSearchOperations(query)).subscribe({
            next: (response: IPagedCollectionView<ISearchOperation>): void => {
                if (callback) {
                    callback(response);
                }
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }

    public createSearchOperation(request: ICreateSearchOperationRequest, callback?: () => void, errorCallback?: () => void): void {
        const formData = new FormData();

        formData.append('title', request.title);
        formData.append('description', request.description);

        if (request.tags?.length) {
            formData.append('tags', request.tags.join(','));
        }

        formData.append('showContactInfo', request.showContactInfo.toString());
        formData.append('operationType', request.operationType.toString());

        if (request.images?.length) {
            for (let i = 0; i < request.images.length; i++) {
                formData.append('images', request.images[i]);
            }
        }

        this.http.post(this.endpointService.createSearchOperation(), formData).subscribe({
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

    public updateSearchOperation(request: IUpdateSearchOperationRequest, callback?: () => void, errorCallback?: () => void): void {
        const formData = new FormData();

        formData.append('id', request.id);
        formData.append('title', request.title);
        formData.append('description', request.description);

        if (request.tags?.length) {
            formData.append('tags', request.tags.join(','));
        }

        formData.append('showContactInfo', request.showContactInfo.toString());
        formData.append('operationType', request.operationType.toString());

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

        this.http.put(this.endpointService.updateSearchOperation(), formData).subscribe({
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

    public deleteSearchOperation(id: string, callback?: () => void, errorCallback?: () => void): void {
        this.http.delete(this.endpointService.deleteSearchOperation(id)).subscribe({
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

    public getSearchOperation(id: string, callback: (helpRequest: ISearchOperation) => void, errorCallback?: () => void): void {
        this.http.get<ISearchOperation>(this.endpointService.getSearchOperation(id)).subscribe({
            next: (response: ISearchOperation): void => {
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
