import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';

import LoaderService from 'src/app/core/services/loader.service';


@Component({
    selector: 'finder-loader',
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.component.scss']
})
export default class LoaderComponent implements OnInit, OnDestroy {
    public showLoader: boolean = false;

    @Input() public type: 'dialog' | 'page' = 'page';

    private loadingSubscription?: Subscription;

    constructor(
        private readonly loaderService: LoaderService,
    ) { }

    public ngOnInit(): void {
        this.loadingSubscription = (this.type === 'dialog' ? this.loaderService.dialogLoading$ : this.loaderService.loading$).subscribe({
            next: (value: boolean): void => {
                this.showLoader = value;
            },
            error: (): void => {
                this.showLoader = false;
            },
            complete: (): void => {
                this.showLoader = false;
            }
        });
    }

    public ngOnDestroy(): void {
        this.loadingSubscription?.unsubscribe();
    }
}
