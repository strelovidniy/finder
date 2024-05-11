import { AfterViewInit, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import NotifierService from 'src/app/core/services/notifier.service';


@Component({
    templateUrl: './copy-dialog.component.html',
    styleUrls: ['./copy-dialog.component.scss']
})
export default class CopyDialogComponent implements AfterViewInit {
    public textCopiedMessage: string = $localize`Text Copied`;
    public textsToCopy: [{ label: string, text: string }] = [] as any;

    constructor(
        @Inject(MAT_DIALOG_DATA) data: { textCopiedMessage: string, textsToCopy: [{ label: string, text: string }] },
        private readonly notifier: NotifierService
    ) {
        this.textCopiedMessage = data?.textCopiedMessage ? data.textCopiedMessage : this.textCopiedMessage;
        this.textsToCopy = data?.textsToCopy ? data.textsToCopy : this.textsToCopy;
    }

    public ngAfterViewInit(): void {
        if (this.textsToCopy.length === 1) {
            this.copyToClipboard(0);
        }
    }

    public copyToClipboard(index: number): void {
        const el = document.getElementById('copyInput' + index) as HTMLInputElement;

        el.value = this.textsToCopy[index].text;
        el.select();
        document.execCommand('copy');

        this.notifier.success(this.textCopiedMessage);
    }
}
