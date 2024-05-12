import { NgModule } from '@angular/core';

import SharedModule from '../shared/shared.module';
import SearchOperationsRouterModule from './search-operations.router.module';

import SearchOperationsComponent from './search-operations.component';
import SearchOperationComponent from './search-operation/search-operation.component';


@NgModule({
    imports: [
        SharedModule,
        SearchOperationsRouterModule
    ],
    declarations: [
        SearchOperationsComponent,
        SearchOperationComponent
    ]
})
export default class SearchOperationsModule {}
