import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { ImageCroppedEvent } from 'ngx-image-cropper';


@Component({
    templateUrl: './image-cropper-dialog.component.html',
    styleUrls: ['./image-cropper-dialog.component.scss']
})
export default class ImageCropperDialogComponent {
    private avatarFile?: Blob;
    public event: Event;

    constructor(
		@Inject(MAT_DIALOG_DATA) dialogData: Event,
        private readonly dialogRef: MatDialogRef<ImageCropperDialogComponent>
    ) {
        this.event = dialogData;
    }

    public imageCropped(event: ImageCroppedEvent): void {
        if (event.blob) {
            this.avatarFile = event.blob;
        }
    }

    private save(): void {
        if (!this.avatarFile) {
            this.onDismiss();

            return;
        }

        this.dialogRef.close(this.avatarFile);
    }

    public onConfirm(): void {
        this.save();
    }

    public onDismiss(): void {
        this.dialogRef.close();
    }
}
