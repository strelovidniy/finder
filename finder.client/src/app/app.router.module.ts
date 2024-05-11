import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule } from '@angular/router';

import AdminModule from './admin/admin.module';
import AuthModule from './auth/auth.module';

import PagenotfoundComponent from './shared/components/pagenotfound/pagenotfound.component';
import SideNavWrapperComponent from './shared/components/side-nav-wrapper/side-nav-wrapper.component';
import AccountPreferencesModule from './account-preferences/account-preferences.module';
import NotificationsModule from './notifications/notifications.module';


@NgModule({
    imports: [
        RouterModule.forRoot(
            [
                {
                    path: '',
                    component: SideNavWrapperComponent,
                    children: [
                        {
                            path: '',
                            redirectTo: '/redirect',
                            pathMatch: 'full'
                        },
                        {
                            path: 'admin',
                            loadChildren: (): Promise<any> => import('./admin/admin.module').then((adminModule): AdminModule => adminModule.default),
                        },
                        {
                            path: 'account-preferences',
                            loadChildren: (): Promise<any> => import('./account-preferences/account-preferences.module').then((accountPreferencesModule): AccountPreferencesModule => accountPreferencesModule.default),
                        },
                        {
                            path: 'notifications',
                            loadChildren: (): Promise<any> => import('./notifications/notifications.module').then((notificationsModule): NotificationsModule => notificationsModule.default),
                        }
                    ]
                },
                {
                    path: 'auth',
                    loadChildren: (): Promise<any> => import('./auth/auth.module').then((authModule): AuthModule => authModule.default),
                },
                {
                    path: 'redirect',
                    component: PagenotfoundComponent
                },
                {
                    path: '**',
                    pathMatch: 'full',
                    component: PagenotfoundComponent
                }
            ],
            {
                preloadingStrategy: PreloadAllModules,
                useHash: true
            })
    ],
    exports: [
        RouterModule
    ]
})
export default class AppRouterModule { }
