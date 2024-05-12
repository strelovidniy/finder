import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { Location } from '@angular/common';

import LoaderService from 'src/app/core/services/loader.service';
import ImageService from 'src/app/core/services/placeholder.service';
import SearchOperationService from 'src/app/core/services/search-operation.service';
import ISearchOperation from 'src/app/core/interfaces/search-operation/search-operation.interface';
import ISearchOperationImage from 'src/app/core/interfaces/search-operation/search-operation-image.interface';
import NotifierService from 'src/app/core/services/notifier.service';


@Component({
    templateUrl: './search-operation-details.component.html',
    styleUrls: ['./search-operation-details.component.scss', './search-operation-details-responsive.component.scss']
})
export default class SearchOperationDetailsComponent implements OnInit, OnDestroy {
    public searchOperation: ISearchOperation = {} as ISearchOperation;

    private id?: string;

    private queryParamsSubscription?: Subscription;

    public defaultImageUrl: string;

    constructor(
        private readonly activatedRoute: ActivatedRoute,
        private readonly router: Router,
        private readonly imageService: ImageService,
        private readonly searchOperationService: SearchOperationService,
        private readonly loader: LoaderService,
        private readonly location: Location,
        private readonly notifier: NotifierService
    ) {
        this.defaultImageUrl = this.imageService.defaultImageUrl;
    }

    public ngOnInit(): void {
        this.loader.show();

        this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe({
            next: (params: { id?: string }): void => {
                this.id = params.id;

                if (!this.id) {
                    this.router.navigate(['/search-operations']);
                    this.loader.hide();
                } else {
                    this.searchOperationService.getSearchOperation(
                        this.id,
                        (searchOperation: ISearchOperation): void => {
                            if (!searchOperation) {
                                this.router.navigate(['/search-operations']);

                                return;
                            }

                            this.searchOperation = searchOperation;

                            this.loader.hide();
                        },
                        (): void => {
                            this.router.navigate(['/search-operations']);
                            this.loader.hide();
                        }
                    );
                }
            }
        });
    }

    public ngOnDestroy(): void {
        this.queryParamsSubscription?.unsubscribe();
    }

    public getMainImage(): ISearchOperationImage {
        return this.searchOperation.images.sort((a, b): number => a.position - b.position)[0];
    }

    public getSecondaryImages(): ISearchOperationImage[] {
        return this.searchOperation.images.sort((a, b): number => a.position - b.position).slice(1);
    }


    public goBack(): void {
        this.location.back();
    }

    public selectTag(tag: string): void {
        this.router.navigate(['/search-operations'], { queryParams: { tag } });
    }

    public apply(): void {
        if (this.id) {
            this.searchOperationService.applyToSearchOperation(
                this.id,
                (): void => {
                    this.notifier.success($localize`You have successfully applied to this operation`);
                },
                (): void => {
                    this.notifier.error($localize`An error occurred while applying to this operation`);
                }
            );
        }
    }

    public contactOrganizer(): void {

    }
}
