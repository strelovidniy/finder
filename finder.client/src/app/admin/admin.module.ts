import { NgModule } from '@angular/core';

import AdminRouterModule from './admin.router.module';
import SharedModule from '../shared/shared.module';

import UserEditorComponent from './components/users/user-editor/user-editor.component';
import ActionsButtonsComponent from './components/users/actions-buttons/actions-buttons.component';
import RolesTableComponent from './components/roles/roles-table/roles-table.component';
import RoleDialogComponent from './components/roles/role-dialog/role-dialog.component';
import UsersTableComponent from './components/users/users-table/users-table.component';
import SetRoleDialogComponent from './components/users/set-role-dialog/set-role-dialog.component';


@NgModule({
    imports: [
        SharedModule,
        AdminRouterModule,
    ],
    declarations: [
        UserEditorComponent,
        ActionsButtonsComponent,
        RolesTableComponent,
        RoleDialogComponent,
        UsersTableComponent,
        SetRoleDialogComponent
    ],
})
export default class AdminModule { }
