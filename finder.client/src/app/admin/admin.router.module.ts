import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import Role from '../core/enums/role/role.enum';

import AuthGuard from '../core/guards/auth.guard';
import RouteGuard from '../core/guards/route.guard';

import RolesTableComponent from './components/roles/roles-table/roles-table.component';
import UsersTableComponent from './components/users/users-table/users-table.component';


@NgModule({
    imports: [
        RouterModule.forChild([{
            path: '',
            canActivate: [AuthGuard],
            children: [
                { path: 'roles', component: RolesTableComponent, canActivate: [RouteGuard], data: { roles: [Role.canSeeRoles] } },
                { path: 'users', component: UsersTableComponent, canActivate: [RouteGuard], data: { roles: [Role.canSeeUsers] } },
            ]
        }])
    ],
    exports: [
        RouterModule
    ],
    providers: [AuthGuard]
})
export default class AdminRouterModule { }
