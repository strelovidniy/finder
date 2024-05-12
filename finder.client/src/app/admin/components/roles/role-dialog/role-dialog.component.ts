import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import lodash from 'lodash';

import AdminRole from 'src/app/core/enums/role/admin-role.enum';
import RolePresetType from 'src/app/core/enums/role/role-preset-type.enum';
import RoleType from 'src/app/core/enums/role/role-type.enum';
import Role from 'src/app/core/enums/role/role.enum';
import UserRole from 'src/app/core/enums/role/user-role.enum';

import ICreateRolerequest from 'src/app/core/interfaces/role/create-role-request.interface';
import IRoleNestedOption from 'src/app/core/interfaces/role/role-nested-option.interface';
import IRoleOption from 'src/app/core/interfaces/role/role-option.interface';
import IRolePreset from 'src/app/core/interfaces/role/role-preset.interface';
import IRole from 'src/app/core/interfaces/role/role.interface';
import IUpdateRoleRequest from 'src/app/core/interfaces/role/update-role-request.interface';

import LoaderService from 'src/app/core/services/loader.service';
import NotifierService from 'src/app/core/services/notifier.service';
import RolesService from 'src/app/core/services/roles.service';
import AuthenticationService from 'src/app/core/services/authentication.service';


@Component({
    selector: 'finder-role-dialog',
    templateUrl: './role-dialog.component.html',
    styleUrls: ['./role-dialog.component.scss']
})
export default class RoleDialogComponent implements OnInit {

    public rolePresetType: string | null = RolePresetType.custom;
    public roleType: string = RoleType.admin;

    public roleTypeOptions = [
        {
            name: $localize`Admin`,
            type: RoleType.admin,
        },
        {
            name: $localize`User`,
            type: RoleType.user,
        }
    ];

    public userPresets = [
        {
            name: $localize`Custom`,
            type: RolePresetType.custom
        },
        {
            name: $localize`User`,
            type: RolePresetType.user
        }
    ];

    public adminPresets = [
        {
            name: $localize`Custom`,
            type: RolePresetType.custom
        },
        {
            name: $localize`Admin`,
            type: RolePresetType.admin
        },
        {
            name: $localize`Helper`,
            type: RolePresetType.helper
        },
        {
            name: $localize`User`,
            type: RolePresetType.user
        }
    ];

    public adminRoleOptions: IRoleOption[] = [
        {
            name: $localize`Users Management`,
            index: 'users',
            checked: true,
            expanded: true,
            options: [
                { name: $localize`View Users`, index: AdminRole.inviteDeleteUsers, checked: true, value: [Role.canSeeAllUsers, Role.canSeeUsers] },
                { name: $localize`Delete Users`, index: AdminRole.inviteDeleteUsers, checked: true, value: [Role.canDeleteUsers] },
                { name: $localize`Edit User`, index: AdminRole.editUsers, checked: true, value: [Role.canEditUsers] }
            ]
        },
        {
            name: $localize`Roles Management`,
            index: 'roles',
            checked: true,
            expanded: true,
            options: [
                { name: $localize`View Roles`, index: AdminRole.viewRoles, checked: true, value: [Role.canSeeRoles, Role.canSeeAllRoles] },
                { name: $localize`Create and Delete Roles`, index: AdminRole.createDeleteRole, checked: true, value: [Role.canCreateRoles, Role.canDeleteRoles] },
                { name: $localize`Edit Roles`, index: AdminRole.editRole, checked: true, value: [Role.canEditRoles] }
            ]
        },
        {
            name: $localize`System Maintenance`,
            index: 'maintenance',
            checked: false,
            expanded: true,
            options: [
                { name: $localize`Maintain System`, index: AdminRole.canMaintainSystem, checked: false, value: [Role.canMaintainSystem] }
            ]
        },
        {
            name: $localize`Help Requests`,
            index: 'help',
            checked: true,
            expanded: true,
            options: [
                { name: $localize`Create Help Request`, index: AdminRole.canCreateHelpRequest, checked: true, value: [Role.canCreateHelpRequest] },
                { name: $localize`View Help Requests`, index: AdminRole.canSeeHelpRequests, checked: true, value: [Role.canSeeHelpRequests] }
            ]
        }
    ];

    public userOptions: IRoleOption[] = [
        {
            name: $localize`Help Requests`,
            index: 'help',
            checked: false,
            expanded: true,
            options: [
                { name: $localize`Create Help Request`, index: UserRole.canCreateHelpRequest, checked: false, value: [Role.canCreateHelpRequest] },
                { name: $localize`View Help Requests`, index: UserRole.canSeeHelpRequests, checked: true, value: [Role.canSeeHelpRequests] }
            ]
        }
    ];

    public roleNameFormControl = new FormControl<string>('', [
        Validators.required
    ]);

    public rolesOptions: IRoleOption[] = this.adminRoleOptions;

    constructor(
        private readonly dialogRef: MatDialogRef<any>,
        private readonly rolesService: RolesService,
        private readonly loader: LoaderService,
        private readonly notifier: NotifierService,
        private readonly authService: AuthenticationService,
        @Inject(MAT_DIALOG_DATA) public data: { isEdit: boolean, role: IRole }

    ) { }

    public ngOnInit(): void {
        this.rolesOptions = this.authService.isAdmin ? this.adminRoleOptions : this.userOptions;

        if (this.data.isEdit) {
            this.initPermissionsValues();
        }
    }

    private defineRoles(): void {
        switch (this.roleType) {
            case RoleType.user:
                this.rolesOptions = this.userOptions;
                break;

            case RoleType.admin:
                this.rolesOptions = this.adminRoleOptions;
                break;
            default:
                break;
        }
    }

    public changeRoleType(): void {
        this.defineRoles();
        this.rolePresetType = RolePresetType.custom;
    }

    public allOptionsInGroupAllowed(options: IRoleNestedOption[]): boolean {
        const result = !!options.find((elem: IRoleNestedOption): boolean => elem.checked === false);


        return !result;
    }

    public updateAllRolesInGroup(value: boolean, index: string): void {
        const result = this.rolesOptions.map((elem: IRoleOption): IRoleOption => {
            if (elem.index === index) {
                const newOptions = elem.options.map((item): IRoleNestedOption => ({ ...item, checked: value }));

                return { ...elem, options: newOptions };
            }

            return elem;
        });

        this.rolesOptions = result;
    }


    public setPresetRole(type: string): void {
        if (type !== RolePresetType.custom) {
            const rolePreset = this.rolesService.getRoleType(type);

            const result = this.rolesOptions.map((elem: IRoleOption): IRoleOption => {
                const newOptions = elem.options.map((item): IRoleNestedOption => ({ ...item, checked: this.roleIsAllowed(rolePreset, item.value || []) }));

                return { ...elem, checked: this.allOptionsInGroupAllowed(newOptions), options: newOptions };
            });

            this.rolesOptions = result;
        }
    }

    private getRoleValue(index: string): boolean {
        return !!lodash.get(this.data.role, index);
    }


    private roleIsChecked(item: IRoleNestedOption): boolean {
        const values = item.value?.map((elem: string): boolean => this.getRoleValue(elem));

        return lodash.every(values);
    }

    private roleIsAllowed(roles: IRolePreset, index: string[]): boolean {
        const result: any[] = [];

        index.forEach((elem: string): void => {
            result.push(lodash.get(roles, elem));
        });

        return lodash.every(result);
    }

    public initPermissionsValues(): void {
        this.roleType = this.data.role.type || '';

        this.defineRoles();

        const result = this.rolesOptions.map((elem: IRoleOption): IRoleOption => {
            const newOptions = elem.options.map((item): IRoleNestedOption => (
                { ...item, checked: this.roleIsChecked(item) }
            ));


            return { ...elem, checked: this.allOptionsInGroupAllowed(newOptions), options: newOptions };
        });

        this.rolesOptions = result;
        this.roleNameFormControl.setValue(this.data.role.name as any);
    }

    public handlePermissionsParams(): { [key: string]: boolean } | null {
        const defaultParams: ICreateRolerequest | IUpdateRoleRequest | any = this.roleType === RoleType.admin ?
            { canSeeAllOrders: true, canSeeAllOrderItems: true } : {};

        const permissionsParams = {};

        this.rolesOptions.forEach((blockElem: IRoleOption): void => {
            blockElem.options.forEach((elem: IRoleNestedOption): void => {
                elem.value?.forEach((item: string): void => {
                    (permissionsParams as any)[item] = elem.checked;
                });
            });
        });

        const roles = lodash.keys(lodash.pickBy(permissionsParams, lodash.identity));

        return lodash.isEmpty(roles) ? null : { ...defaultParams, ...permissionsParams };
    }

    public discard(): void {
        this.dialogRef.close(undefined);
    }

    public save(): void {
        const permissionsValues = this.handlePermissionsParams();

        if (!permissionsValues) {
            this.notifier.error($localize`You can\'t create role without selecting permissions.`);

            return;
        }

        const data: ICreateRolerequest = {
            name: this.roleNameFormControl.value || '',
            type: this.roleType,
            ...permissionsValues,
        };


        this.rolesService.createRole(data, this.discard.bind(this));
    }

    public update(): void {
        const permissionsValues = this.handlePermissionsParams();

        if (!permissionsValues) {
            this.notifier.error($localize`You can\'t create role without selecting permissions.`);

            return;
        }

        const data: IUpdateRoleRequest = {
            id: this.data.role.id || '',
            name: this.roleNameFormControl.value || '',
            ...permissionsValues
        };

        this.rolesService.updateRole(data, this.discard.bind(this));
    }

    public get presets(): { type: string, name: string }[] | undefined {
        switch (this.roleType) {
            case RoleType.user:
                return this.userPresets;

            case RoleType.admin:
                return this.adminPresets;
            default:
                return undefined;
        }
    }

    public get isAdminType(): boolean {
        return this.roleType === RoleType.admin;
    }
}
