import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import ISetRoleRequest from '../interfaces/user/set-role-request.interface';
import IUserUpdateRequest from '../interfaces/user/user-update-request.interface';
import IUser from '../interfaces/user/user.interface';

import EndpointService from './endpoint.service';
import LoaderService from './loader.service';
import NotifierService from './notifier.service';
import IPagedCollectionView from '../interfaces/system/paged-collection-view.interface';


@Injectable({
    providedIn: 'root'
})
export default class UserService {
    public usersSubject: BehaviorSubject<IUser[]> = new BehaviorSubject<IUser[]>([]);
    public totalCountSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    constructor(
        private readonly endpointService: EndpointService,
        private readonly http: HttpClient,
        private readonly loader: LoaderService,
        private readonly notifier: NotifierService
    ) {
    }

    public getUsers(query: string, callback?: () => void, errorCallback?: () => void): void {
        this.loader.show();


        this.http.get<IPagedCollectionView<IUser>>(`${this.endpointService.users(query)}`).subscribe({
            next: (response: IPagedCollectionView<IUser>): void => {
                this.usersSubject.next(response.items);
                this.totalCountSubject.next(response.totalCount);

                if (callback) {
                    callback();
                }

                this.loader.hide();
            },
            error: (error): void => {
                if (errorCallback) {
                    errorCallback();
                }

                this.loader.hide();
            }
        });
    }

    public updateUser(data: IUserUpdateRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.showDialogLoading();
        this.http.put(this.endpointService.userUpdate(), data).subscribe({
            next: (): void => {

                if (callback) {
                    callback();
                }

                this.loader.hideDialogLoading();
                this.notifier.success('Сhanges applied');
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }

                this.loader.hideDialogLoading();
            }
        });
    }

    public deleteUser(id: string, callback?: () => void, errorCallback?: () => void): void {
        this.loader.show();
        this.http.delete(this.endpointService.deleteUser(id)).subscribe({

            next: (): void => {
                if (callback) {
                    callback();
                }

                this.loader.hide();
                this.notifier.success('User has ben deleted');
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }

                this.loader.hide();
            }
        });
    }

    public setUserRole(data: ISetRoleRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.showDialogLoading();
        this.http.post(this.endpointService.setUserRole(), data).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }

                this.notifier.success('Сhanges applied');
                this.loader.hideDialogLoading();
            },
            error: (): void => {

                if (errorCallback) {
                    errorCallback();
                }

                this.loader.hideDialogLoading();
            }
        });
    }
}
