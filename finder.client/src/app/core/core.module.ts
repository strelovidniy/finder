// modules
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';

import SharedModule from '../shared/shared.module';

import AuthGuard from './guards/auth.guard';


@NgModule({
    declarations: [
    ],
    imports: [
        SharedModule,
        CommonModule,
        HttpClientModule,
        BrowserModule,
        BrowserAnimationsModule,
        RouterModule,
        ToastrModule.forRoot({
            timeOut: 3000,
            positionClass: 'toast-top-right',
            preventDuplicates: false,
            progressBar: true,
            maxOpened: 0,
            titleClass: 'ngx-toastr-title',
            messageClass: 'ngx-toastr-message'
        }),
    ],
    exports: [
        CommonModule,
    ],
    providers: [AuthGuard]

})
export default class CoreModule {
}
