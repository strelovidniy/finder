import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import AuthGuard from '../core/guards/auth.guard';

import SharedModule from '../shared/shared.module';

import SearchOperationsComponent from './search-operations.component';

import RouteGuard from '../core/guards/route.guard';

import RoleType from '../core/enums/role/role-type.enum';
import SearchOperationComponent from './search-operation/search-operation.component';


@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild([{
            path: '',
            canActivate: [AuthGuard],
            children: [
                {
                    path: '',
                    component: SearchOperationsComponent,
                    canActivate: [RouteGuard],
                    data: {
                        type: [RoleType.user, RoleType.admin]
                    }
                },
                {
                    path: 'create',
                    component: SearchOperationComponent,
                    canActivate: [RouteGuard],
                    data: {
                        type: [RoleType.user, RoleType.admin]
                    }
                },
                {
                    path: 'edit/:id',
                    component: SearchOperationComponent,
                    canActivate: [RouteGuard],
                    data: {
                        type: [RoleType.user, RoleType.admin]
                    }
                }
            ]
        }])
    ],
    exports: [
        RouterModule
    ],
    providers: [AuthGuard]
})
export default class SearchOperationsRouterModule { }
