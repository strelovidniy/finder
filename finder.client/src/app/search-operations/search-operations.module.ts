import { NgModule } from '@angular/core';

import SharedModule from '../shared/shared.module';
import SearchOperationsRouterModule from './search-operations.router.module';

import SearchOperationsComponent from './search-operations.component';
import SearchOperationComponent from './search-operation/search-operation.component';
import SearchOperationDetailsComponent from './search-operation-details/search-operation-details.component';


@NgModule({
    imports: [
        SharedModule,
        SearchOperationsRouterModule
    ],
    declarations: [
        SearchOperationsComponent,
        SearchOperationComponent,
        SearchOperationDetailsComponent
    ]
})
export default class SearchOperationsModule {}
