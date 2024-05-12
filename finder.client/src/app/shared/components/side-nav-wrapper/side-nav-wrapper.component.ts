import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs/internal/Subscription';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs';

import AuthenticationService from 'src/app/core/services/authentication.service';
import SideNavService from 'src/app/core/services/sidenaw.service';
import ImageService from 'src/app/core/services/placeholder.service';
import EventBusService from 'src/app/core/services/event-bus.service';

import ChangePasswordDialogComponent from '../dialogs/change-password-dialog/change-password-dialog.component';
import ProfileDialogComponent from '../dialogs/profile-dialog/profile-dialog.component';

import RoleType from 'src/app/core/enums/role/role-type.enum';
import Role from 'src/app/core/enums/role/role.enum';


@Component({
    selector: 'finder-side-nav-wrapper',
    templateUrl: './side-nav-wrapper.component.html',
    styleUrls: ['./side-nav-wrapper.component.scss', './side-nav-wrapper-responsive.component.scss']
})
export default class SideNavWrapperComponent implements OnInit, OnDestroy {

    public userName: string | null = null;
    public avatarUrl: string = '';
    public avatarThumbnailUrl: string = '';

    public isContentTransparent: boolean = false;

    public showSearchField: boolean = false;

    public selectedLocale?: string | null;

    public searchFormControl = new FormControl('');

    private refreshUiSubscription?: Subscription;
    private contentTransparencySubscription?: Subscription;
    private searchFieldSubscription?: Subscription;
    private searchValueSubscription?: Subscription;
    private setSeachValueSubscription?: Subscription;

    constructor(
        private readonly authService: AuthenticationService,
        private readonly dialog: MatDialog,
        private readonly sideNavService: SideNavService,
        private readonly imageService: ImageService,
        private readonly eventBus: EventBusService,
        private readonly router: Router
    ) { }

    public ngOnDestroy(): void {
        this.refreshUiSubscription?.unsubscribe();
        this.contentTransparencySubscription?.unsubscribe();
        this.searchFieldSubscription?.unsubscribe();
        this.searchValueSubscription?.unsubscribe();
        this.setSeachValueSubscription?.unsubscribe();
    }

    public ngOnInit(): void {
        this.searchFieldSubscription = this.sideNavService.searchFieldSubject.subscribe({
            next: (showSearchField: boolean): void => {
                this.showSearchField = showSearchField;
            }
        });

        this.setSeachValueSubscription = this.sideNavService.setSearchValueSubject.subscribe({
            next: (value: string): void => {
                this.searchFormControl.setValue(value);
            }
        });

        this.selectedLocale = location.href.replace(location.origin, '').split('/')[1];

        localStorage.setItem('locale', this.selectedLocale);

        this.refreshUiSubscription = this.eventBus.refreshUiSubject.subscribe({
            next: (): void => this.initInfo()
        });

        this.contentTransparencySubscription = this.sideNavService.contentTransparencySubject.subscribe({
            next: (isTransparent: boolean): void => {
                this.isContentTransparent = isTransparent;
            }
        });

        this.authService.getUserInfo();

        this.searchValueSubscription = this.searchFormControl.valueChanges.pipe(debounceTime(500)).subscribe({
            next: (value: string | null): void => {
                this.sideNavService.searchFieldChangeSubject.next(value || '');
            }
        });
    }

    private initInfo(): void {
        this.userName = this.authService.currentUser ? `${this.authService.currentUser.firstName} ${this.authService.currentUser.lastName}` : null;
        this.avatarUrl = this.authService.currentUser.details?.imageUrl ? this.authService.currentUser.details?.imageUrl : this.imageService.defaultImageUrl;
        this.avatarThumbnailUrl = this.authService.currentUser.details?.imageThumbnailUrl ? this.authService.currentUser.details?.imageThumbnailUrl : this.imageService.defaultImageUrl;
    }

    public logout(event: Event): void {
        event.preventDefault();
        this.authService.logout();
    }

    public changePasswordDialog(): void {
        this.dialog.open(ChangePasswordDialogComponent, {
            width: '400px',
            disableClose: true
        });
    }


    public profileDialog(): void {
        this.dialog.open(ProfileDialogComponent, {
            width: '400px',
            disableClose: true
        });
    }

    public goBack(): void {
        window.history.back();
    }

    public goForward(): void {
        window.history.forward();
    }

    public changeLanguage(locale: string): void {
        const existingLocale = location.href.replace(location.origin, '').split('/')[1];

        if (existingLocale !== locale) {
            localStorage.setItem('locale', locale);

            location.href = location.href.replace(existingLocale, locale);
        }
    }

    public goToProfileDetails(): void {
        this.router.navigate(['/account-preferences']);
    }

    public goToNotifications(): void {
        this.router.navigate(['/notifications']);
    }

    public goToHome(): void {
        this.router.navigate(['/search-operations']);
    }

    public goToUsers(): void {
        this.router.navigate(['/admin/users']);
    }

    public goToRoles(): void {
        this.router.navigate(['/admin/roles']);
    }

    public get showRoles(): boolean {
        return this.authService.checkRole(Role.canSeeAllRoles) || this.authService.checkRole(Role.canSeeRoles);
    }

    public get showNotifications(): boolean {
        return true;
    }

    public get showUsers(): boolean {
        return this.authService.checkRole(Role.canSeeAllUsers) || this.authService.checkRole(Role.canSeeUsers);
    }

    public get showAccountPreferences(): boolean {
        return this.authService.getRoleType() === RoleType.user || this.authService.getRoleType() === RoleType.admin;
    }
}
