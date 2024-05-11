import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { ToastrService } from 'ngx-toastr';


@Injectable({
    providedIn: 'root'
})
export default class NotifierService {
    private readonly horyzontalPosition: MatSnackBarHorizontalPosition = 'right';
    private readonly verticalPosition: MatSnackBarVerticalPosition = 'bottom';
    private readonly defaultDuration: number = 3000;

    constructor(
        private readonly snackbar: MatSnackBar,
        private readonly toastr: ToastrService
    ) { }

    public success(message: string, action?: string): void {
        // this.showSnackBar(message, action ?? 'Success', 'snackbar-success');
        this.toastr.success(message, action ?? 'Success');
    }

    public neutral(message: string, action?: string): void {
        // this.showSnackBar(message, action ?? '', 'snackbar-neutral');
        this.toastr.info(message, action ?? '');
    }

    public warning(message: string, action?: string): void {
        // this.showSnackBar(message, action ?? 'Warning', 'snackbar-warning');
        this.toastr.warning(message, action ?? 'Warning');
    }

    public error(message: string, action?: string): void {
        // this.showSnackBar(message, action ?? 'Error', 'snackbar-error');
        this.toastr.error(message, action ?? 'Error');
    }

    private showSnackBar(message: string, action: string, className: string): void {
        this.snackbar.open(message, action, {
            duration: this.defaultDuration,
            panelClass: [className, 'snackbar-config'],
            horizontalPosition: this.horyzontalPosition,
            verticalPosition: this.verticalPosition
        });
    }


}
