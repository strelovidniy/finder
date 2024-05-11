import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs/internal/Subscription';

import Role from 'src/app/core/enums/role/role.enum';

import AuthenticationService from 'src/app/core/services/authentication.service';
import SideNavService from 'src/app/core/services/sidenaw.service';
import ImageService from 'src/app/core/services/placeholder.service';
import EventBusService from 'src/app/core/services/event-bus.service';

import ChangePasswordDialogComponent from '../dialogs/change-password-dialog/change-password-dialog.component';
import ProfileDialogComponent from '../dialogs/profile-dialog/profile-dialog.component';
import RoleType from 'src/app/core/enums/role/role-type.enum';


@Component({
    selector: 'finder-side-nav-wrapper',
    templateUrl: './side-nav-wrapper.component.html',
    styleUrls: ['./side-nav-wrapper.component.scss', './side-nav-wrapper-responsive.component.scss']
})
export default class SideNavWrapperComponent implements OnInit, AfterViewInit, OnDestroy {

    public showAdminMenu: boolean = false;
    public showWidgetsMenu: boolean = false;

    public userName: string | null = null;
    public avatarUrl: string = '';
    public avatarThumbnailUrl: string = '';

    public isContentTransparent: boolean = false;

    public selectedLocale?: string | null;

    private refreshUiSubscription?: Subscription;
    private contentTransparencySubscription?: Subscription;

    public showBar: boolean = false;

    private touchableOverlay: HTMLElement | null = null;

    constructor(
        private readonly authService: AuthenticationService,
        private readonly dialog: MatDialog,
        private readonly sideNavService: SideNavService,
        private readonly imageService: ImageService,
        private readonly eventBus: EventBusService
    ) { }

    public ngOnDestroy(): void {
        this.refreshUiSubscription?.unsubscribe();
        this.contentTransparencySubscription?.unsubscribe();
    }

    public ngOnInit(): void {
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

        this.showBar = this.sideNavService.isSideNavOpened;

        this.authService.getUserInfo();

        this.sideNavService.isAdminMenuExpanded = this.sideNavService.isAdminMenuExpanded || location.href.includes('/admin/');
        this.sideNavService.isWidgetsMenuExpanded = this.sideNavService.isWidgetsMenuExpanded || location.href.includes('/widgets/');

        this.showAdminMenu = this.sideNavService.isAdminMenuExpanded;
        this.showWidgetsMenu = this.sideNavService.isWidgetsMenuExpanded;
    }

    public ngAfterViewInit(): void {
        this.touchableOverlay = document.getElementById('navBarTouchableOpacityOverlay');
    }

    private initInfo(): void {
        this.userName = this.authService.currentUser ? `${this.authService.currentUser.firstName} ${this.authService.currentUser.lastName}` : null;
        this.avatarUrl = this.authService.currentUser.details?.imageUrl ? this.authService.currentUser.details?.imageUrl : this.imageService.defaultImageUrl;
        this.avatarThumbnailUrl = this.authService.currentUser.details?.imageThumbnailUrl ? this.authService.currentUser.details?.imageThumbnailUrl : this.imageService.defaultImageUrl;
    }

    public toggleAdmin(): void {
        this.showAdminMenu = !this.showAdminMenu;

        this.sideNavService.isAdminMenuExpanded = this.showAdminMenu;
    }

    public toggleWidgets(): void {
        this.showWidgetsMenu = !this.showWidgetsMenu;

        this.sideNavService.isWidgetsMenuExpanded = this.showWidgetsMenu;
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

    public toggleBar(showBar: boolean = !this.showBar): void {
        this.showBar = showBar;
        this.sideNavService.isSideNavOpened = this.showBar;

        if (this.touchableOverlay) {
            this.touchableOverlay.style.opacity = this.showBar ? '1' : '0';
        }

        if (this.showBar) {
            if (this.touchableOverlay) {
                this.touchableOverlay.style.display = 'flex';
            }
        }

        if (!this.showBar) {
            setTimeout((): void => {
                if (this.touchableOverlay) {
                    this.touchableOverlay.style.display = 'none';
                }
            }, 225);
        }
    }

    public closeBarIfMobile(): void {
        if (window.innerWidth < 768) {
            this.toggleBar(false);
        }
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

    public get showRoles(): boolean {
        return this.authService.checkRole(Role.canSeeAllRoles) || this.authService.checkRole(Role.canSeeRoles);
    }

    public get showNotifications(): boolean {
        return this.authService.checkRole(Role.canSeeHelpRequests);
    }

    public get showUsers(): boolean {
        return this.authService.checkRole(Role.canSeeAllUsers) || this.authService.checkRole(Role.canSeeUsers);
    }

    public get showAccountPreferences(): boolean {
        return this.authService.getRoleType() === RoleType.user || this.authService.getRoleType() === RoleType.admin;
    }
}
