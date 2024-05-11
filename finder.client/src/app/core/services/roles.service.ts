import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import RolePresetType from '../enums/role/role-preset-type.enum';

import IRole from '../interfaces/role/role.interface';
import IUpdateRoleRequest from '../interfaces/role/update-role-request.interface';
import ICreateRolerequest from '../interfaces/role/create-role-request.interface';
import IPagedCollectionView from '../interfaces/system/paged-collection-view.interface';

import EndpointService from './endpoint.service';
import LoaderService from './loader.service';
import NotifierService from './notifier.service';


@Injectable({
    providedIn: 'root'
})
export default class RolesService {
    public rolesSubject: BehaviorSubject<IRole[]> = new BehaviorSubject<IRole[]>([]);
    public totalCountSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);

    public rolesAutocompleteSubject: BehaviorSubject<IRole[]> = new BehaviorSubject<IRole[]>([]);

    constructor(
        private readonly endpointService: EndpointService,
        private readonly http: HttpClient,
        private readonly loader: LoaderService,
        private readonly notifier: NotifierService
    ) { }

    public get roles(): IRole[] {
        return this.rolesSubject?.value;
    }

    public getRoles(query: string): void {
        this.loader.show();

        this.http.get<IPagedCollectionView<IRole>>(`${this.endpointService.rolesList(query)}`).subscribe({
            next: (response: IPagedCollectionView<IRole>): void => {
                this.rolesSubject.next(response.items);
                this.totalCountSubject.next(response.totalCount);

                this.loader.hide();
            },
            error: (error): void => {
                this.loader.hide();
            }
        });
    }

    public createRole(data: ICreateRolerequest, callback: () => void): void {
        this.loader.showDialogLoading();
        this.http.post<IRole>(`${this.endpointService.createRole()}`, data).subscribe({
            next: (role): void => {
                this.rolesSubject.next([role, ...this.rolesSubject.value]);
                this.loader.hideDialogLoading();
                this.notifier.success('New role has been created');
                callback();
            },
            error: (error): void => {
                this.loader.hideDialogLoading();
            }
        });
    }

    public updateRole(data: IUpdateRoleRequest, callback: () => void): void {
        this.loader.showDialogLoading();
        this.http.put<IRole>(`${this.endpointService.updateRole()}`, data).subscribe({
            next: (role): void => {
                const newRolesList: IRole[] = this.rolesSubject.value.map((elem: IRole): IRole => data.id === elem.id ? role : elem);

                this.rolesSubject.next(newRolesList);
                this.loader.hideDialogLoading();
                this.notifier.success('Role has been updated');
                callback();
            },
            error: (error): void => {
                this.loader.hideDialogLoading();
            }
        });
    }

    public deleteRole(id: string, name: string): void {
        this.loader.show();

        this.http.delete<IRole>(`${this.endpointService.deleteRoleById(id)}`).subscribe({
            next: (): void => {
                const newRolesList: IRole[] = this.rolesSubject.value.filter((elem: IRole): boolean => elem.id !== id);

                this.notifier.success(`Role ${name} has been deleted`);
                this.rolesSubject.next(newRolesList);
                this.loader.hide();
            },
            error: (error): void => {
                this.loader.hide();
            }
        });
    }

    public roleSearch(query: string): void {
        this.loader.show();

        this.http.get<IRole[]>(this.endpointService.adminOdataRolesUrl(query)).subscribe({
            next: (roles: IRole[]): void => {
                this.rolesSubject.next(roles);
                this.loader.hide();

            },
            error: (error): void => {
                this.loader.hide();
            }
        });
    }

    public roleAutocomplete(query: string): Observable<IRole[]> {
        return this.http.get<IRole[]>(this.endpointService.rolesAutocomplete(query));
    }


    public getRoleType(type: string): { [key: string]: boolean } {

        switch (type) {
            case RolePresetType.admin:
                return {
                    canDeleteUsers: true,
                    canEditUsers: true,
                    canRestoreUsers: true,
                    canCreateRoles: true,
                    canEditRoles: true,
                    canDeleteRoles: true,
                    canSeeAllUsers: true,
                    canSeeUsers: true,
                    canSeeAllRoles: true,
                    canSeeRoles: true,
                    canMaintainSystem: true
                };
            case RolePresetType.helper:
                return {
                    canDeleteUsers: false,
                    canEditUsers: false,
                    canRestoreUsers: false,
                    canCreateRoles: false,
                    canEditRoles: false,
                    canDeleteRoles: false,
                    canSeeAllUsers: false,
                    canSeeUsers: true,
                    canSeeAllRoles: false,
                    canSeeRoles: false,
                    canMaintainSystem: false
                };
            case RolePresetType.user:
                return {
                    canDeleteUsers: false,
                    canEditUsers: false,
                    canRestoreUsers: false,
                    canCreateRoles: false,
                    canEditRoles: false,
                    canDeleteRoles: false,
                    canSeeAllUsers: false,
                    canSeeUsers: true,
                    canSeeAllRoles: false,
                    canSeeRoles: false,
                    canMaintainSystem: false
                };
            default:
                return {};
        }
    };
}
