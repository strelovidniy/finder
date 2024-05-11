import { NgModule } from '@angular/core';

import SharedModule from '../shared/shared.module';
import SearchOperationsRouterModule from './search-operations.router.module';

import SearchOperationsComponent from './search-operations.component';


@NgModule({
    imports: [
        SharedModule,
        SearchOperationsRouterModule
    ],
    declarations: [
        SearchOperationsComponent
    ]
})
export default class SearchOperationsModule {}
