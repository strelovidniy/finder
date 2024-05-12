import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { Location } from '@angular/common';

import { MatDialog } from '@angular/material/dialog';

import LoaderService from 'src/app/core/services/loader.service';
import ImageService from 'src/app/core/services/placeholder.service';
import SearchOperationService from 'src/app/core/services/search-operation.service';
import NotifierService from 'src/app/core/services/notifier.service';
import AuthenticationService from 'src/app/core/services/authentication.service';

import ISearchOperation from 'src/app/core/interfaces/search-operation/search-operation.interface';
import ISearchOperationImage from 'src/app/core/interfaces/search-operation/search-operation-image.interface';


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
        private readonly notifier: NotifierService,
        private readonly authService: AuthenticationService,
        private readonly dialog: MatDialog
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

    public getMainImage(): ISearchOperationImage | undefined {
        return this.searchOperation.images?.sort((a, b): number => a.position - b.position)[0];
    }

    public getSecondaryImages(): ISearchOperationImage[] {
        return this.searchOperation.images?.sort((a, b): number => a.position - b.position).slice(1) || [];
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

    public downloadQr(): void {
        if (this.id) {
            this.searchOperationService.generateQr(
                this.id,
                (blob: Blob): void => {
                    const url = window.URL.createObjectURL(new Blob([blob]));
                    const a = document.createElement('a');

                    a.href = url;
                    a.download = 'qr.png';
                    a.target = '_blank';
                    document.body.appendChild(a);
                    a.click();
                    this.notifier.success($localize`QR code successfully downloaded`);
                },
                (): void => {
                    this.notifier.error($localize`An error occurred while downloading the QR code`);
                }
            );
        }
    }

    public downloadPdf(): void {
        if (this.id) {
            this.searchOperationService.downloadPdf(
                this.id,
                (blob: Blob): void => {
                    const url = window.URL.createObjectURL(new Blob([blob]));
                    const a = document.createElement('a');

                    a.href = url;
                    a.download = 'operation.pdf';
                    a.target = '_blank';
                    document.body.appendChild(a);
                    a.click();
                    this.notifier.success($localize`PDF successfully downloaded`);
                },
                (): void => {
                    this.notifier.error($localize`An error occurred while downloading the PDF`);
                }
            );
        }
    }

    public createChat(): void {
        if (this.id) {
            this.searchOperationService.createChat(
                this.id,
                (chatUrl: string): void => {
                    const alket = document.createElement('a');

                    alket.href = chatUrl;
                    alket.target = '_blank';
                    alket.click();

                    this.notifier.success($localize`Chat successfully created`);
                },
                (): void => {
                    this.notifier.error($localize`An error occurred while creating the chat`);
                }
            );
        }
    }

    public openChat(): void {
        if (this.searchOperation.chatUrl) {
            const alket = document.createElement('a');

            alket.href = this.searchOperation.chatUrl;
            alket.target = '_blank';
            alket.click();
        }

    }

    public facebookShare(): void {
        window.open(`https://www.facebook.com/sharer/sharer.php?u=${window.location.href}`);
    }

    public twitterShare(): void {
        window.open(`https://twitter.com/intent/tweet?source=tweetbutton&text=${this.searchOperation.title}&url=${window.location.href}`);
    }

    public linkedinShare(): void {
        window.open(`http://www.linkedin.com/shareArticle?mini=true&url=${window.location.href}&title=${this.searchOperation.title}&source=${window.location.origin}`);
    }

    public edit(): void {
        this.router.navigate(['/search-operations/edit'], { queryParams: { id: this.id } });
    }

    public get showEditButton(): boolean {
        return this.searchOperation.creatorId === this.authService.currentUser?.id;
    }
}
