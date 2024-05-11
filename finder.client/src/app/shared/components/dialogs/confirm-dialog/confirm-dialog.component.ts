import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.scss']
})
export default class ConfirmDialogComponent {


    public title: string = $localize`Confirm Action`;
    public message: string = $localize`Are you sure you want do this?`;

    constructor(
        private readonly dialogRef: MatDialogRef<ConfirmDialogComponent>,
        @Inject(MAT_DIALOG_DATA) data: { title: string, message: string }
    ) {
        this.title = data?.title ? data.title : this.title;
        this.message = data?.message ? data.message : this.message;
    }

    public onConfirm(): void {
        this.dialogRef.close(true);
    }

    public onDismiss(): void {
        this.dialogRef.close(false);
    }
}
