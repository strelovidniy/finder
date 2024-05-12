import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import ICreateSearchOperationRequest from 'src/app/core/interfaces/search-operation/create-search-operation-request.interface';
import ISearchOperationImage from 'src/app/core/interfaces/search-operation/search-operation-image.interface';
import ISearchOperation from 'src/app/core/interfaces/search-operation/search-operation.interface';
import IUpdateSearchOperationRequest from 'src/app/core/interfaces/search-operation/update-search-operation-request.interface';

import SearchOperationService from 'src/app/core/services/search-operation.service';

import LoaderService from 'src/app/core/services/loader.service';
import NotifierService from 'src/app/core/services/notifier.service';
import OperationType from 'src/app/core/enums/search-operation/operation-type.enum';
import AuthenticationService from 'src/app/core/services/authentication.service';


@Component({
    templateUrl: './search-operation.component.html',
    styleUrls: ['./search-operation.component.scss', './search-operation-responsive.component.scss']
})
export default class SearchOperationComponent implements OnInit, OnDestroy {
    public titleFormControl = new FormControl('', [Validators.required, Validators.maxLength(200)]);
    public descriptionFormControl = new FormControl('', [Validators.required, Validators.maxLength(2000)]);
    public tagsFormControl = new FormControl('');
    public categoryFormControl = new FormControl('', [Validators.required]);

    public showContactInfo: boolean = false;
    public isDeadline: boolean = false;

    public images: ISearchOperationImage[] = [];

    private imageIdsToRemove: string[] = [];

    private imagesToAdd: File[] = [];

    public categories: { value: OperationType, name: string }[] = [
        {
            value: OperationType.unknown,
            name: $localize`Unknown`
        },
        {
            value: OperationType.evacuation,
            name: $localize`Evacuation`
        },
        {
            value: OperationType.missing,
            name: $localize`Missing`
        },
        {
            value: OperationType.buildingCollapse,
            name: $localize`Building collapse`
        }
    ];

    public searchOperation: ISearchOperation | ICreateSearchOperationRequest | IUpdateSearchOperationRequest = {
        id: '',
        title: '',
        description: '',
        tags: [],
        applicantsCount: 0,
        contactInfo: undefined,
        issuerImage: '',
        issuerImageThumbnail: '',
        issuerName: '',
        operationType: OperationType.unknown,
        showContactInfo: false,
        deadline: undefined,
        images: []
    } as ISearchOperation | ICreateSearchOperationRequest | IUpdateSearchOperationRequest;

    private queryParamsSubscription?: Subscription;

    constructor(
        private readonly notifier: NotifierService,
        private readonly searchOperationService: SearchOperationService,
        private readonly router: Router,
        private readonly loader: LoaderService,
        private readonly activatedRoute: ActivatedRoute,
        private readonly location: Location,
        private readonly authService: AuthenticationService
    ) { }

    public ngOnInit(): void {
        this.loader.show();

        this.queryParamsSubscription = this.activatedRoute.queryParams.subscribe((params: { id?: string }): void => {
            if (!params.id) {
                this.location.replaceState('/search-operations/create');
                this.loader.hide();
            } else {
                this.searchOperationService.getSearchOperation(
                    params.id,
                    (searchOperation: ISearchOperation): void => {
                        if (!searchOperation) {
                            this.location.back();
                        } else {
                            this.searchOperation = searchOperation;

                            this.titleFormControl.setValue(searchOperation.title);
                            this.descriptionFormControl.setValue(searchOperation.description);
                            this.tagsFormControl.setValue(searchOperation.tags.join(', '));
                            this.showContactInfo = !!searchOperation.contactInfo;
                            this.images = searchOperation.images;
                        }

                        this.loader.hide();
                    },
                    (): void => {
                        this.location.back();
                    }
                );
            }
        });
    }

    public ngOnDestroy(): void {
        this.queryParamsSubscription?.unsubscribe();
    }

    public save(): void {
        if ((this.searchOperation as ISearchOperation).id) {
            this.updateSearchOperation();
        } else {
            this.createSearchOperation();
        }
    }

    private updateSearchOperation(): void {
        this.loader.show();

        const request = {
            id: (this.searchOperation as ISearchOperation).id,
            title: this.titleFormControl.value,
            description: this.descriptionFormControl.value,
            tags: this.tagsFormControl.value ? this.tagsFormControl.value.split(',').map((tag: string): string => tag.trim()) : [],
            showContactInfo: this.showContactInfo,
            operationType: this.searchOperation.operationType,
            images: this.imagesToAdd,
            imagesToDelete: this.imageIdsToRemove
        } as IUpdateSearchOperationRequest;

        if (this.searchOperation.images.length - request.imagesToDelete.length + request.images.length <= 0) {
            this.notifier.error($localize`Please add at least one image`);

            this.loader.hide();

            return;
        }

        this.searchOperationService.updateSearchOperation(
            request,
            (): void => {
                this.notifier.success($localize`Request updated successfully`);

                this.loader.hide();

                this.router.navigate(['/search-operations']);
            },
            (): void => {
                this.notifier.error($localize`Failed to update search operation`);

                this.loader.hide();
            }
        );
    }

    private createSearchOperation(): void {
        this.loader.show();

        const request = {
            title: this.titleFormControl.value,
            description: this.descriptionFormControl.value,
            tags: this.tagsFormControl.value ? this.tagsFormControl.value.split(',').map((tag: string): string => tag.trim()) : [],
            showContactInfo: this.showContactInfo,
            operationType: OperationType.unknown,
            images: this.imagesToAdd
        } as ICreateSearchOperationRequest;

        if (request.images.length === 0) {
            this.notifier.error($localize`Please add at least one image`);

            this.loader.hide();

            return;
        }

        this.searchOperationService.createSearchOperation(
            request,
            (): void => {
                this.notifier.success($localize`Request created successfully`);

                this.loader.hide();

                this.router.navigate(['/search-operations']);

            },
            (): void => {
                this.notifier.error($localize`Failed to create request`);

                this.loader.hide();
            }
        );
    }

    public removeImage(image: ISearchOperationImage): void {
        if (image.id) {
            this.imageIdsToRemove.push(image.id);
        }

        this.images = this.images.filter((i: ISearchOperationImage): boolean => i !== image);
        this.imagesToAdd = this.imagesToAdd.filter((i: File): boolean => i !== (image as any).file);
    }

    public addImages(): void {
        const input = document.createElement('input');

        input.type = 'file';
        input.accept = '.png,.jpeg,.jpg';
        input.id = 'userPictureInput';
        input.multiple = true;
        input.onchange = (event: Event): void => this.addImageFiles(event);
        input.click();
    }

    private addImageFiles(event: Event): void {
        if (!event.target) {
            return;
        }

        const input = event.target as HTMLInputElement;
        const files = input.files;

        if (!files) {
            return;
        }

        for (let i = 0; i < files.length; i++) {
            const file = files[i];

            if (file.size > 104857600) {
                this.notifier.error($localize`The image size should not exceed 100MB`);
                this.notifier.warning($localize`Images with size over 100MB ignored`);

                continue;
            }

            this.imagesToAdd.push(file);

            const reader = new FileReader();

            reader.onload = (e: ProgressEvent<FileReader>): void => {
                if (!e.target) {
                    return;
                }

                const image = {
                    id: '',
                    imageUrl: e.target.result as string,
                    imageThumbnailUrl: e.target.result as string,
                    position: 0,
                    file
                } as ISearchOperationImage;

                this.images.push(image);
            };

            reader.readAsDataURL(file);
        }


        this.removeImageInput();
    }

    private removeImageInput(): void {
        const input = document.getElementById('userPictureInput');

        if (input) {
            input.remove();
        }
    }

    public goBack(): void {
        this.location.back();
    }

    public getTitle(): string {
        return this.authService.isAdmin ? $localize`Create search operation` : $localize`Create search operation request`;
    }
}
