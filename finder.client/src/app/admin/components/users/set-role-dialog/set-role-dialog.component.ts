import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, Subscription, debounceTime, switchMap } from 'rxjs';

import IRole from 'src/app/core/interfaces/role/role.interface';
import IUser from 'src/app/core/interfaces/user/user.interface';

import LoaderService from 'src/app/core/services/loader.service';
import RolesService from 'src/app/core/services/roles.service';
import UserService from 'src/app/core/services/users.service';
import FormValidators from 'src/app/shared/validators/form.validator';


@Component({
    selector: 'finder-set-role-dialog',
    templateUrl: './set-role-dialog.component.html',
    styleUrls: ['./set-role-dialog.component.scss']
})
export default class SetRoleDialogComponent implements OnInit, OnDestroy {

    public rolesOptions: IRole[] = [];
    public preloader: boolean = true;

    private autoCompleteSubscription?: Subscription;
    private roleNameSubscription?: Subscription;

    constructor(
        private readonly dialogRef: MatDialogRef<SetRoleDialogComponent>,
        private readonly userService: UserService,
        private readonly loader: LoaderService,
        private readonly rolesService: RolesService,
        @Inject(MAT_DIALOG_DATA) private data: { user: IUser }
    ) { }

    public ngOnInit(): void {
        this.initAutocompleteOptions();
        this.roleNameSubscription = this.roleNameFormControl.valueChanges.pipe(
            debounceTime(100),
            switchMap((value): Observable<IRole[]> => this.rolesService.roleAutocomplete((value as any)?.id ? '' : value || ''))
        ).subscribe({
            next: (roles: IRole[]): void => {
                this.rolesOptions = this.filterRolesOptions(roles);
            }
        });
    }

    public ngOnDestroy(): void {
        this.autoCompleteSubscription?.unsubscribe();
        this.roleNameSubscription?.unsubscribe();
    }

    private filterRolesOptions(roles: IRole[]): IRole[] {
        const roleType = this.data.user?.role?.type;

        return roles.filter((elem: IRole): boolean => elem.type === roleType);
    }

    public roleNameFormControl = new FormControl(null, [
        FormValidators.validateProperty('id')
    ]);

    private initAutocompleteOptions(): void {
        this.autoCompleteSubscription = this.rolesService.roleAutocomplete('').subscribe({
            next: (value: IRole[]): void => {
                this.rolesOptions = value;
                this.preloader = false;
                const currentRole = this.rolesOptions.find((elem: IRole): any => this.data.user.role.id === elem.id);

                currentRole && this.roleNameFormControl.setValue(currentRole as any);
            }
        });
    }

    public discard(): void {
        this.dialogRef.close();
    }

    public callback(): void {
        this.dialogRef.close(true);
    }

    public setRole(): void {
        if (this.roleNameFormControl.errors?.['invalidProperty']) {
            this.roleNameFormControl.markAllAsTouched();

            return;
        };
        this.userService.setUserRole({
            userId: this.data.user.id,
            roleId: this.roleNameFormControl.value?.['id'] || ''
        }, this.callback.bind(this));
    }

    public getName(role: IRole): string {
        return (role ? role.name : '') || '';
    }

}
