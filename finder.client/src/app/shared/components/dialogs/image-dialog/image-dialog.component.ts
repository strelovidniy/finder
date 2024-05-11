import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
    templateUrl: './image-dialog.component.html',
    styleUrls: ['./image-dialog.component.scss']
})
export default class ImageDialogComponent {

    constructor(
        private readonly dialogRef: MatDialogRef<ImageDialogComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: { imageUrl: string }
    ) { }


    public onDismiss(): void {
        this.dialogRef.close();
    }

    public get image(): string {
        return this.data.imageUrl;
    }
}
