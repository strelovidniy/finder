import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SwPush } from '@angular/service-worker';
import { Router } from '@angular/router';
import { BehaviorSubject, finalize, Observable, of, tap, map, switchMap } from 'rxjs';
import lodash from 'lodash';

import environment from 'src/environments/environment';

import RoleType from '../enums/role/role-type.enum';
import Role from '../enums/role/role.enum';

import IRole from '../interfaces/role/role.interface';
import IUserUpdateRequest from '../interfaces/user/user-update-request.interface';
import IAuthRequest from '../interfaces/auth/auth-request.interface';
import IChangePasswordRequest from '../interfaces/auth/change-password-request.interface';
import IConfirmEmailRequest from '../interfaces/auth/confirm-email-request.interface';
import IResetPasswordRequest from '../interfaces/auth/reset-password-request.interface';
import IUserMe from '../interfaces/auth/user-me.interface';
import ISetNewPasswordRequest from '../interfaces/auth/set-new-password-request.interface';
import ISignupRequest from '../interfaces/auth/signup-request.interface';
import IToken from '../interfaces/auth/token.interface';

import EndpointService from './endpoint.service';
import LoaderService from './loader.service';
import NotifierService from './notifier.service';
import EventBusService from './event-bus.service';


@Injectable({
    providedIn: 'root',
})
export default class AuthenticationService {
    public currentUserSubject: BehaviorSubject<IUserMe> = new BehaviorSubject<IUserMe>(null as any as IUserMe);

    public constructor(
        private readonly http: HttpClient,
        private readonly endpointService: EndpointService,
        private readonly notifier: NotifierService,
        private readonly loader: LoaderService,
        private readonly router: Router,
        private readonly eventBus: EventBusService,
        private readonly swPush: SwPush
    ) {
        this.initCurrentUser();
    }

    private initCurrentUser(): void {
        this.currentUserSubject = new BehaviorSubject<IUserMe>(
            JSON.parse(localStorage.getItem('currentUser') || '{}')
        );
    }

    public get currentUserValue(): IUserMe {
        return this.currentUserSubject.value;
    }

    public get currentUser(): IUserMe {
        return this.currentUserSubject?.value;
    }

    public get currentUserAccesses(): IRole {
        return this.currentUserSubject?.value?.access;
    }

    public get isAdmin(): boolean {
        return this.currentUserSubject?.value?.access?.type === RoleType.admin;
    }

    public get isHelper(): boolean {
        return this.currentUserSubject?.value?.access?.type === RoleType.helper;
    }

    public get isRegularUser(): boolean {
        return this.currentUserSubject?.value?.access?.type === RoleType.user;
    }

    public refreshToken(): Observable<string | null> {
        const refreshToken = localStorage.getItem('refresh-token');

        if (!refreshToken) {
            return of(null);
        }

        return this.http.get(this.endpointService.refreshToken(refreshToken)).pipe(
            tap((res: any): void => this.setToken(res)),
            map((res: any): string => res.token?.token)
        );
    }

    public getToken(): string | null {
        const expDate = new Date(localStorage.getItem('token-exp') || '{}').getTime();

        if (expDate && new Date().getTime() > expDate) {
            return null;
        }

        return localStorage.getItem('token');
    }

    public login(
        data: IAuthRequest
    ): Observable<IUserMe> {
        this.loader.show();

        return this.http.post(this.endpointService.login(), data).pipe(
            tap((res: any): void => {
                this.setCurrentUser(res.user);
                this.setToken(res.token);
            }),
            switchMap((_): Observable<IUserMe> => this.getCurrentUser()),
            switchMap((currentUser: IUserMe): Observable<IUserMe> => {
                if (currentUser?.notificationsConfig?.enableNotifications) {
                    this.swPush
                        .requestSubscription({
                            serverPublicKey: environment.serverPublicKey,
                        })
                        .then((pushSubscription: PushSubscription): Promise<void> => this.addUserPushSubscription(pushSubscription).toPromise())
                        .catch((error): void => console.error(error));
                }

                return of(currentUser);
            }),
            finalize((): void => this.loader.hide())
        );
    }

    public setNewPassword(data: ISetNewPasswordRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.show();

        this.http.post(this.endpointService.setNewPassword(), data).subscribe({
            next: (): void => {
                this.notifier.success('');
                this.loader.hide();
                if (callback) {
                    callback();
                }
            },
            error: (error): void => {
                if (errorCallback) {
                    errorCallback();
                }
                this.loader.hideDialogLoading();
                this.loader.hide();
            }
        });
    }

    public getCurrentUser(): Observable<IUserMe> {
        return this.http.get<IUserMe>(this.endpointService.userInfo()).pipe(
            tap((userInfo: IUserMe): void => {
                this.currentUserSubject.next(userInfo);
                this.eventBus.refreshUiSubject.next();
                localStorage.setItem('currentUser', JSON.stringify(userInfo));
            })
        );
    }

    public getUserInfo(): void {
        this.loader.showDialogLoading();
        this.getCurrentUser().subscribe({
            next: (userInfo: IUserMe): void => {
                this.eventBus.refreshUiSubject.next();
                this.loader.hideDialogLoading();
            },
            error: (error): void => {
                this.eventBus.refreshUiSubject.next();
                this.loader.hideDialogLoading();
            },
        });
    }

    public resetPassword(data: IResetPasswordRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.showDialogLoading();
        this.http.post(`${this.endpointService.resetPassword()}`, data).subscribe({
            next: (): void => {
                this.notifier.success($localize`New password has been sent to the mail`);
                if (callback) {
                    callback();
                }
                this.loader.hideDialogLoading();
            },
            error: (error): void => {
                if (errorCallback) {
                    errorCallback();
                }
                this.loader.hideDialogLoading();
            }
        });
    }

    public confirmEmail(data: IConfirmEmailRequest, callback?: () => void, errorCallback?: () => void): void {
        this.http.post(`${this.endpointService.confirmEmail()}`, data).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }
                this.loader.hide();
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
            }
        });
    }

    public changePassword(data: IChangePasswordRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.showDialogLoading();
        this.http.post(this.endpointService.changePassword(), data).subscribe({
            next: (): void => {
                this.notifier.success($localize`Password changed`);
                if (callback) {
                    callback();
                }
                this.loader.hideDialogLoading();
            },
            error: (error): void => {
                if (errorCallback) {
                    errorCallback();
                }
                this.loader.hideDialogLoading();
            }
        });
    }


    public signUp(data: ISignupRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.show();
        this.http.post(`${this.endpointService.signUp()}`, data).subscribe({
            next: (): void => {
                if (callback) {
                    callback();
                }
                this.loader.hide();
            },
            error: (): void => {
                if (errorCallback) {
                    errorCallback();
                }
                this.loader.hide();
            },
        });
    }

    public updateUserProfile(data: IUserUpdateRequest, callback?: () => void, errorCallback?: () => void): void {
        this.loader.showDialogLoading();
        this.http.put(this.endpointService.userUpdate(), data).subscribe({
            next: (): void => {
                this.loader.hideDialogLoading();
                this.notifier.success($localize`Сhanges applied`);
                if (callback) {
                    callback();
                }
            },
            error: (error): void => {
                if (errorCallback) {
                    errorCallback();
                }
                this.loader.hideDialogLoading();
            },
        });
    }

    public isAuthenticated(): boolean {
        const refreshTokenExpireAt = localStorage.getItem('refresh-token-exp');
        const refreshToken = localStorage.getItem('refresh-token');
        const tokenExpireAt = localStorage.getItem('token-exp');
        const token = localStorage.getItem('token');

        if (!token || !tokenExpireAt) {
            return false;
        }

        if (new Date().getTime() <= new Date(tokenExpireAt).getTime()) {
            return true;
        }

        if (!refreshTokenExpireAt || !refreshToken) {
            return false;
        }

        if (new Date().getTime() <= new Date(refreshTokenExpireAt).getTime()) {
            return true;
        }

        return false;
    }

    public logout(): void {
        this.loader.hide();
        this.loader.hideDialogLoading();

        localStorage.clear();
        this.currentUserSubject.next(null as any);
        this.router.navigate(['/auth/welcome']);
    }

    private setToken(token: IToken | null): void {
        if (token) {
            localStorage.setItem('token', token.token);
            localStorage.setItem('token-exp', token.expireAt);

            if (token.refreshToken) {
                localStorage.setItem('refresh-token', token.refreshToken);
            }

            if (token.refreshTokenExpireAt) {
                localStorage.setItem('refresh-token-exp', token.refreshTokenExpireAt);
            }

        } else {
            localStorage.clear();
        }
    }

    private setCurrentUser(user: IUserMe): void {
        localStorage.setItem('currentUser', JSON.stringify(user));

        this.initCurrentUser();

        this.currentUserSubject.next(user);
    }

    public checkRole(roleName: Role): boolean {
        const result = (this.currentUserSubject.value?.access as any)?.[roleName];


        return result;
    }

    public getRoleType(): string {
        const result = this.currentUserSubject.value?.access?.type;

        return result || '';
    }

    public hasAtLeastOnePermission(permissions: Role[]): boolean {
        const permissionsList = permissions.map((elem: Role): boolean =>
            this.checkRole(elem)
        );


        return lodash.some(permissionsList);
    }

    public hasAllPermissions(permissions: Role[]): boolean {
        const permissionsList = permissions.map((elem: Role): boolean =>
            this.checkRole(elem)
        );

        return lodash.every(permissionsList);
    }

    public addUserPushSubscription(pushSubscription: PushSubscription): Observable<void> {
        return this.http.post<void>(`${this.endpointService.addUserPushSubscription()}`, pushSubscription);
    }
}
