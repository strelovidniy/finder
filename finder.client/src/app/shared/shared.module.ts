import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { ImageCropperModule } from 'ngx-image-cropper';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { ColorPickerModule } from '@iplab/ngx-color-picker';
import { LazyLoadImageModule } from 'ng-lazyload-image';

import MaterialModule from './material/material.module';

import StatusTagComponent from './components/status-tag/status-tag.component';
import PagenotfoundComponent from './components/pagenotfound/pagenotfound.component';
import NoDataMessageComponent from './components/no-data-message/no-data-message.component';
import ConfirmDialogComponent from './components/dialogs/confirm-dialog/confirm-dialog.component';
import ChangePasswordDialogComponent from './components/dialogs/change-password-dialog/change-password-dialog.component';
import SideNavWrapperComponent from './components/side-nav-wrapper/side-nav-wrapper.component';
import LoaderComponent from './components/loader/loader.component';
import ProfileDialogComponent from './components/dialogs/profile-dialog/profile-dialog.component';
import ImageDialogComponent from './components/dialogs/image-dialog/image-dialog.component';
import ImageCropperDialogComponent from './components/dialogs/image-cropper-dialog/image-cropper-dialog.component';
import ToggleComponent from './components/toggle/toggle.component';
import ColorSelectorComponent from './components/color-selector/color-selector.component';
import ColorCircleComponent from './components/color-circle/color-circle.component';
import CopyDialogComponent from './components/dialogs/copy-dialog/copy-dialog.component';


const sharedModules = [
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    NgxSkeletonLoaderModule,
    RouterModule,
    ImageCropperModule,
    SlickCarouselModule,
    LazyLoadImageModule
];

const sharedComponents = [
    StatusTagComponent,
    NoDataMessageComponent,
    ConfirmDialogComponent,
    ChangePasswordDialogComponent,
    SideNavWrapperComponent,
    LoaderComponent,
    ProfileDialogComponent,
    ImageDialogComponent,
    ImageCropperDialogComponent,
    ToggleComponent,
    ColorSelectorComponent,
    ColorCircleComponent,
    CopyDialogComponent
];

@NgModule({
    imports: [
        ...sharedModules,
        ColorPickerModule,
    ],
    exports: [
        ...sharedModules,
        ...sharedComponents
    ],
    declarations: [
        PagenotfoundComponent,
        ...sharedComponents
    ],
})
export default class SharedModule {}
