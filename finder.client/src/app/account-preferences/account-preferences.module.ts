import { NgModule } from '@angular/core';

import SharedModule from '../shared/shared.module';
import AccountPreferencesRouterModule from './account-preferences.router.module';

import AccountPreferencesComponent from './account-preferences.component';


@NgModule({
    imports: [
        SharedModule,
        AccountPreferencesRouterModule
    ],
    declarations: [
        AccountPreferencesComponent
    ]
})
export default class AccountPreferencesModule {}
