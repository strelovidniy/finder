import { NgModule } from '@angular/core';

import SharedModule from '../shared/shared.module';
import NotificationsRouterModule from './notifications.router.module';

import NotificationsComponent from './notifications.component';


@NgModule({
    imports: [
        SharedModule,
        NotificationsRouterModule
    ],
    declarations: [
        NotificationsComponent
    ]
})
export default class NotificationsModule {}
