import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import IUserUpdateRequest from 'src/app/core/interfaces/user/user-update-request.interface';
import IUser from 'src/app/core/interfaces/user/user.interface';

import LoaderService from 'src/app/core/services/loader.service';
import UserService from 'src/app/core/services/users.service';


@Component({
    selector: 'finder-user-editor',
    templateUrl: './user-editor.component.html',
    styleUrls: ['./user-editor.component.scss']
})
export default class UserEditorComponent implements OnInit {

    public constructor(
        private readonly dialogRef: MatDialogRef<UserEditorComponent>,
        private readonly userService: UserService,
        private readonly loader: LoaderService,
        @Inject(MAT_DIALOG_DATA) private data: { user: IUser }
    ) { }

    public userFirstNameFormControl = new FormControl('', [
        Validators.required
    ]);

    public userLastNameFormControl = new FormControl('', [
        Validators.required
    ]);


    public userEmailFormControl = new FormControl({ value: '', disabled: true });

    public userFormGroup = new FormGroup({
        email: this.userEmailFormControl,
        firstName: this.userFirstNameFormControl,
        lastName: this.userLastNameFormControl
    });

    public ngOnInit(): void {
        if (this.data.user) {
            this.userFirstNameFormControl.setValue(this.data.user.firstName);
            this.userLastNameFormControl.setValue(this.data.user.lastName);
            this.userEmailFormControl.setValue(this.data.user.email);
        }
    }

    public discard(): void {
        this.dialogRef.close();
    }

    public callback(): void {
        this.dialogRef.close(true);
    }

    public update(): void {
        this.userService.updateUser({ id: this.data.user.id, ...this.userFormGroup.value } as IUserUpdateRequest, this.callback.bind(this));
    }


    public get buttonIsDisabled(): boolean {
        return this.userFormGroup.invalid;
    }

}
