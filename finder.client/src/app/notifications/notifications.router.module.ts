import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import AuthGuard from '../core/guards/auth.guard';

import SharedModule from '../shared/shared.module';

import AccountPreferencesComponent from './notifications.component';

import RouteGuard from '../core/guards/route.guard';

import RoleType from '../core/enums/role/role-type.enum';


@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild([{
            path: '',
            canActivate: [AuthGuard],
            children: [
                {
                    path: '',
                    component: AccountPreferencesComponent,
                    canActivate: [RouteGuard],
                    data: {
                        type: [RoleType.helper, RoleType.admin]
                    }
                },
            ]
        }])
    ],
    exports: [
        RouterModule
    ],
    providers: [AuthGuard]
})
export default class NotificationsRouterModule { }
