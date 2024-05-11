import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Subscription, debounceTime, tap } from 'rxjs';
import lodash from 'lodash';

import RoleTableFields from 'src/app/core/enums/role/role-table-fields.enum';
import RoleTypeName from 'src/app/core/enums/role/role-type-name.enum';
import Role from 'src/app/core/enums/role/role.enum';

import IQeryParams from 'src/app/core/interfaces/system/query-params.interface';
import IRole from 'src/app/core/interfaces/role/role.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';
import PaginationService from 'src/app/core/services/pagination.service';
import RolesService from 'src/app/core/services/roles.service';

import ConfirmDialogComponent from 'src/app/shared/components/dialogs/confirm-dialog/confirm-dialog.component';
import RoleDialogComponent from '../role-dialog/role-dialog.component';


@Component({
    templateUrl: './roles-table.component.html',
    styleUrls: ['./roles-table.component.scss', './roles-table-responsive.component.scss']
})
export default class RolesTableComponent implements OnInit, OnDestroy {

    public displayedColumns: string[] = [RoleTableFields.name, RoleTableFields.type, RoleTableFields.createdAt, RoleTableFields.updatedAt, RoleTableFields.actions];
    public tableColumnsName = RoleTableFields;
    public roles: IRole[] = [];

    public totalCount: number = 0;

    public pageSizeOptions: number[] = [10];
    public pageSize: number = 10;
    private pageIndex: number = 1;

    private sortFiledName: string = '';
    private sortDirection: boolean | null = null;

    private searchTerm: string = '';
    public searchNameControl = new FormControl('', []);

    private rolesSubscription?: Subscription;
    private totalCountSubscription?: Subscription;
    private searchSubscription?: Subscription;

    constructor(
        private readonly rolesService: RolesService,
        private readonly dialog: MatDialog,
        private readonly paginationService: PaginationService,
        private readonly authService: AuthenticationService
    ) { }


    public ngOnInit(): void {
        this.pageSizeOptions = this.paginationService.pageSizeOptions;
        this.getData();

        this.rolesSubscription = this.rolesService.rolesSubject.subscribe({
            next: (roles: IRole[]): void => {
                this.roles = roles;
            }
        });

        this.totalCountSubscription = this.rolesService.totalCountSubject.subscribe({
            next: (totalCount: number): void => {
                this.totalCount = totalCount;
            }
        });

        this.searchSubscription = this.searchNameControl.valueChanges.pipe(
            debounceTime(500),
            tap((value: any): void => {
                this.searchTerm = value;
                this.getData();
            })).subscribe();
    }

    public ngOnDestroy(): void {
        this.rolesSubscription?.unsubscribe();
        this.totalCountSubscription?.unsubscribe();
        this.searchSubscription?.unsubscribe();
    }

    private getData(): void {
        const params: IQeryParams = {
            searchQuery: this.searchTerm,
            pageNumber: this.pageIndex,
            pageSize: this.pageSize,
            sortBy: this.sortFiledName,
            sortAscending: this.sortDirection
        };

        const query = this.paginationService.queryBuilder(params);

        this.rolesService.getRoles(query);
    }

    public openRoleDialog(isEdit?: boolean, role?: IRole): void {
        this.dialog.open(RoleDialogComponent, {
            width: '500px',
            data: {
                isEdit,
                role
            }
        });
    }

    public removeRole(id: string, name: string, event: MouseEvent): void {
        this.dialog.open(ConfirmDialogComponent, {
            maxWidth: '400px',
            data: {
                message: $localize`Are you sure you want to delete this role?`
            }
        }).updatePosition({
            right: '25px',
            top: `${event.y - 205}px`,
        }).afterClosed().subscribe((confirm: boolean): void => {
            if (confirm) {
                this.rolesService.deleteRole(id, name);
            }
        });
    }

    public handlePageEvent(event: PageEvent): any {
        const { pageSize, pageIndex } = event;

        this.pageSize = pageSize;
        this.pageIndex = pageIndex + 1;

        this.getData();
    }

    public sortChange(sortState: Sort): void {
        const { active, direction } = sortState;

        this.sortDirection = this.paginationService.getSortDirection(direction);
        this.sortFiledName = this.sortDirection !== null ? active : '';
        this.getData();

    }

    public search(): void {
        this.pageIndex = 1;
        this.getData();
    }

    public getRoleType(type: string): string {
        return (RoleTypeName as any)[type];
    }

    public get showAction(): boolean {
        return !lodash.some([this.canDeleteRoles, this.canEditRoles]);
    }

    public get canDeleteRoles(): boolean {
        return this.authService.checkRole(Role.canDeleteRoles);
    }

    public get canEditRoles(): boolean {
        return this.authService.checkRole(Role.canEditRoles);
    }

    public get canCreateRoles(): boolean {
        return this.authService.checkRole(Role.canCreateRoles);
    }
}
