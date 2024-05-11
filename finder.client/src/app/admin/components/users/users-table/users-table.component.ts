import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Subscription, debounceTime, tap } from 'rxjs';
import lodash from 'lodash';

import Role from 'src/app/core/enums/role/role.enum';

import IQeryParams from 'src/app/core/interfaces/system/query-params.interface';
import IUser from 'src/app/core/interfaces/user/user.interface';

import AuthenticationService from 'src/app/core/services/authentication.service';
import PaginationService from 'src/app/core/services/pagination.service';
import UserService from 'src/app/core/services/users.service';

import ConfirmDialogComponent from 'src/app/shared/components/dialogs/confirm-dialog/confirm-dialog.component';
import SetRoleDialogComponent from '../set-role-dialog/set-role-dialog.component';
import UserEditorComponent from '../user-editor/user-editor.component';


@Component({
    templateUrl: './users-table.component.html',
    styleUrls: ['./users-table.component.scss', './users-table-responsive.component.scss']
})
export default class UsersTableComponent implements OnInit, OnDestroy {

    public displayedColumns: string[] = ['firstName', 'lastName', 'role.name', 'status', 'email', 'actions'];

    public users: IUser[] = [];
    public totalCount: number = 0;

    public pageSizeOptions: number[] = [10];
    public pageSize: number = 10;
    private pageIndex: number = 1;

    private sortFiledName: string = '';
    private sortDirection: boolean | null = null;

    private searchTerm: string = '';
    public searchNameControl = new FormControl('', []);

    private usersSubscription?: Subscription;
    private totalCountSubscription?: Subscription;
    private searchSubscription?: Subscription;

    constructor(
        private readonly dialog: MatDialog,
        private readonly userService: UserService,
        private readonly paginationService: PaginationService,
        private readonly authService: AuthenticationService
    ) { }


    public ngOnInit(): void {
        this.pageSizeOptions = this.paginationService.pageSizeOptions;

        this.getData();

        this.usersSubscription = this.userService.usersSubject.subscribe({
            next: (users: IUser[]): void => {
                this.users = users;
            }
        });

        this.totalCountSubscription = this.userService.totalCountSubject.subscribe({
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
        this.usersSubscription?.unsubscribe();
        this.totalCountSubscription?.unsubscribe();
        this.searchSubscription?.unsubscribe();
    }

    public editUser(user: IUser): void {
        this.dialog.open(UserEditorComponent, {
            width: '500px',
            disableClose: true,
            data: {
                user
            }
        }).afterClosed().subscribe({
            next: (isUpdate): void => isUpdate && this.getData()
        });
    }

    public setRole(user: IUser): void {
        this.dialog.open(SetRoleDialogComponent, {
            width: '500px',
            autoFocus: false,
            disableClose: true,
            data: {
                user
            }
        }).afterClosed().subscribe({
            next: (isUpdate): void => isUpdate && this.getData()
        });
    }

    public deleteUser(id: string, event: MouseEvent): void {
        this.dialog.open(ConfirmDialogComponent, {
            maxWidth: '400px',
            data: {
                message: $localize`Are you sure you want to delete this user?`
            }
        }).updatePosition({
            right: '25px',
            top: `${event.y - 205}px`,
        }).afterClosed().subscribe((confirm: boolean): void => {
            if (confirm) {
                this.userService.deleteUser(id, this.getData.bind(this));
            }
        });
    }

    private getData(): void {
        const params: IQeryParams = {
            searchQuery: this.searchTerm,
            pageNumber: this.pageIndex,
            pageSize: this.pageSize,
            sortBy: this.sortFiledName,
            sortAscending: this.sortDirection,
            expandProperty: 'role'
        };
        const query = this.paginationService.queryBuilder(params);

        this.userService.getUsers(query);
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

    public get showAction(): boolean {
        return lodash.some([this.canDeleteUser, this.canEditUser]);
    }

    public get canDeleteUser(): boolean {
        return this.authService.checkRole(Role.canDeleteUsers);
    }

    public get canEditUser(): boolean {
        return this.authService.checkRole(Role.canEditUsers);
    }

    public get currentUserId(): string {
        return this.authService?.currentUser?.id;
    }
}
