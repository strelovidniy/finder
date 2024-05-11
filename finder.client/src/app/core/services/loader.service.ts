import { Injectable } from '@angular/core';
import { BehaviorSubject, delay } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export default class LoaderService {
    private readonly loading = new BehaviorSubject<boolean>(false);
    public readonly loading$ = this.loading.asObservable().pipe(delay(1));

    private readonly dialogLoading = new BehaviorSubject<boolean>(false);
    public readonly dialogLoading$ = this.dialogLoading.asObservable().pipe(delay(1));


    public show(): void {
        this.loading.next(true);
    }

    public hide(): void {
        this.loading.next(false);
    }

    public showDialogLoading(): void {
        this.dialogLoading.next(true);
    }

    public hideDialogLoading(): void {
        this.dialogLoading.next(false);
    }
}
