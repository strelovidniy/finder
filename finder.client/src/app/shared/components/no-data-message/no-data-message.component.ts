import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';

import LoaderService from 'src/app/core/services/loader.service';


@Component({
    selector: 'finder-no-data-message',
    templateUrl: './no-data-message.component.html',
    styleUrls: ['./no-data-message.component.scss']
})
export default class NoDataMessageComponent implements OnInit, OnDestroy {
    public loading: boolean = true;

    private loadingSubscription?: Subscription;

    constructor(
        public loaderService: LoaderService
    ) { }

    @Input() public message?: string;
    @Input() public iconName?: string;

    public ngOnInit(): void {
        this.loadingSubscription = this.loaderService.loading$.subscribe({
            next: (value): void => {
                this.loading = value;
            },
            error: (): void => {
                this.loading = false;
            }
        });
    }

    public ngOnDestroy(): void {
        this.loadingSubscription?.unsubscribe();
    }

    public getMessage(): string {
        return this.message ? this.message : $localize`No records found`;
    }
}
