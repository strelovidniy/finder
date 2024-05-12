import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription, debounceTime, tap } from 'rxjs';
import { Location } from '@angular/common';

import IPagedCollectionView from '../core/interfaces/system/paged-collection-view.interface';
import IQeryParams from '../core/interfaces/system/query-params.interface';
import ISearchOperationImage from '../core/interfaces/search-operation/search-operation-image.interface';
import ISearchOperation from '../core/interfaces/search-operation/search-operation.interface';

import SideNavService from '../core/services/sidenaw.service';
import SearchOperationService from '../core/services/search-operation.service';
import LoaderService from '../core/services/loader.service';
import NotifierService from '../core/services/notifier.service';
import PaginationService from '../core/services/pagination.service';
import ImageService from '../core/services/placeholder.service';
import ViewService from '../core/services/view.service';

import ConfirmDialogComponent from '../shared/components/dialogs/confirm-dialog/confirm-dialog.component';


@Component({
    templateUrl: './search-operations.component.html',
    styleUrls: ['./search-operations.component.scss', './search-operations-responsive.component.scss']
})
export default class SearchOperationsComponent implements OnInit, OnDestroy {

    public items: ISearchOperation[] = [];

    public totalCount: number = 0;

    public pageSizeOptions: number[] = [10];
    public pageSize: number = 10;
    private pageIndex: number = 1;

    public sortFiledName: string = '';
    public sortDirection: boolean | null = null;

    private searchTerm: string = '';
    public searchNameControl = new FormControl('', []);

    public defaultImageUrl: string;

    public tableView: boolean = true;

    private searchSubscription?: Subscription;
    private queryParamsSubscription?: Subscription;
    private removeItemSubscription?: Subscription;

    constructor(
        private readonly searchOperationService: SearchOperationService,
        private readonly dialog: MatDialog,
        private readonly paginationService: PaginationService,
        private readonly notifier: NotifierService,
        private readonly loader: LoaderService,
        private readonly router: Router,
        private readonly imageService: ImageService,
        private readonly sideNavService: SideNavService,
        private readonly activatedRoute: ActivatedRoute,
        private readonly location: Location,
        private readonly viewService: ViewService
    ) {
        this.defaultImageUrl = this.imageService.defaultImageUrl;
    }

    public ngOnInit(): void {

        this.tableView = this.viewService.isTableView('search-operations');

        this.pageSizeOptions = this.paginationService.pageSizeOptions;
        this.getData();

        this.searchSubscription = this.searchNameControl.valueChanges.pipe(
            debounceTime(500),
            tap((value: any): void => {
                this.searchTerm = value;
                this.getData();
            })).subscribe();

        this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe({
            next: (params: { tag?: string }): void => {
                this.searchNameControl.setValue(params.tag || '');
                this.sideNavService.setSearchValue(params.tag || '');
                this.location.replaceState('/search-operations');
            }
        });

        this.sideNavService.enableSearchField();
        this.searchSubscription = this.sideNavService.searchFieldChangeSubject.subscribe({
            next: (value: string): void => {
                this.searchNameControl.setValue(value);
            }
        });
    }

    public ngOnDestroy(): void {
        this.searchSubscription?.unsubscribe();
        this.queryParamsSubscription?.unsubscribe();
        this.removeItemSubscription?.unsubscribe();

        this.sideNavService.disableSearchField();
    }

    public getMainImage(item: ISearchOperation): ISearchOperationImage {
        return item.images.sort((a, b): number => (a.position || 0) - (b.position || 0))[0];
    }

    public getSecondaryImages(item: ISearchOperation): ISearchOperationImage[] {
        return item.images.sort((a, b): number => (a.position || 0) - (b.position || 0)).slice(1);
    }

    public openDetails(id: string): void {
        this.router.navigate(['/search-operations/details'], { queryParams: { id } });
    }

    public getDescription(item: ISearchOperation): string {
        return item.description.length > 100 ? `${item.description.replace(/<[^>]*>?/gm, '').substring(0, 100)}...` : item.description.replace(/<[^>]*>?/gm, '');
    }

    private getData(): void {
        this.loader.show();

        const params: IQeryParams = {
            searchQuery: this.searchTerm,
            pageNumber: this.pageIndex,
            pageSize: this.pageSize,
            sortBy: this.sortFiledName,
            sortAscending: this.sortDirection,
            expandProperty: 'images'
        };

        const query = this.paginationService.queryBuilder(params);

        this.searchOperationService.getSearchOperations(
            query,
            (searchOperations: IPagedCollectionView<ISearchOperation>): void => {
                this.items = searchOperations.items;
                this.totalCount = searchOperations.totalCount;

                this.loader.hide();
            },
            (): void => {
                this.loader.hide();
            }
        );
    }

    public handlePageEvent(event: PageEvent): any {
        const { pageSize, pageIndex } = event;

        this.pageSize = pageSize;
        this.pageIndex = pageIndex + 1;

        this.getData();
    }

    public sortChange(sortState: Sort): void {
        const { active, direction } = sortState;

        this.sortDirection = this.paginationService.getSortDirection(direction);
        this.sortFiledName = this.sortDirection !== null ? active : '';
        this.getData();

    }

    public search(): void {
        this.pageIndex = 1;

        this.getData();
    }

    public createSearchOperation(): void {
        this.router.navigate(['/search-operations/create']);
    }

    public editSearchOperation(id: string): void {
        this.router.navigate(['/search-operations/update'], { queryParams: { id } });
    }

    public selectTag(tag: string, event: MouseEvent): void {
        event.stopPropagation();

        if (this.searchNameControl.value === tag) {
            this.searchNameControl.setValue('');
        } else {
            this.searchNameControl.setValue(tag);
        }
    }

    public isTagActive(tag: string): boolean {
        return this.searchNameControl.value === tag;
    }

    public removeItem(id: string): void {
        this.removeItemSubscription = this.dialog.open(ConfirmDialogComponent, {
            maxWidth: '400px',
            data: {
                message: $localize`Are you sure you want to remove this request? This action can not be undone.`
            }
        }).afterClosed().subscribe((confirm: boolean): void => {
            if (confirm) {
                this.loader.show();

                this.searchOperationService.deleteSearchOperation(
                    id,
                    (): void => {
                        this.notifier.success($localize`Request removed successfully`);

                        this.getData();
                    },
                    (): void => {
                        this.notifier.error($localize`Something went wrong. Request was not removed.`);

                        this.loader.hide();
                    }
                );
            }
        });
    }

    public switchView(): void {
        this.tableView = !this.tableView;
        this.viewService.setTableView('search-operations', this.tableView);
    }

    public getSwitchViewButtonText(): string {
        return this.tableView ? $localize`Switch to Card View` : $localize`Switch to Table View`;
    }

    public getIssuerImageUrl(item: ISearchOperation): string {
        return item.issuerImage || this.defaultImageUrl;
    }

    public getIssuerImageThumbnailUrl(item: ISearchOperation): string {
        return item.issuerImageThumbnail || this.defaultImageUrl;
    }

    public selectIssuer(issuerName: string, event: MouseEvent): void {
        event.stopPropagation();

        if (this.searchNameControl.value === issuerName) {
            this.searchNameControl.setValue('');
        } else {
            this.searchNameControl.setValue(issuerName);
        }
    }

    public get canCreateSearchOperation(): boolean {
        return true;
    }
}
