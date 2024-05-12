import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

import IInviteUserRequest from 'src/app/core/interfaces/user/invite-user-request.interface';

import LoaderService from 'src/app/core/services/loader.service';
import RolesService from 'src/app/core/services/roles.service';
import UserService from 'src/app/core/services/users.service';

import FormValidators from 'src/app/shared/validators/form.validator';


@Component({
    selector: 'finder-invite-user-dialog',
    templateUrl: './invite-user-dialog.component.html',
    styleUrls: ['./invite-user-dialog.component.scss']
})
export default class InviteUserDialogComponent {

    public constructor(
        private dialogRef: MatDialogRef<InviteUserDialogComponent>,
        private userService: UserService,
        public loader: LoaderService,
        public rolesService: RolesService
    ) { }

    public userEmailFormControl = new FormControl('', [
        Validators.required,
        Validators.email
    ]);


    public vendorNameFormControl = new FormControl(null, [
        FormValidators.validateProperty('id')
    ]);

    public roleNameFormControl = new FormControl('', [
        FormValidators.validateProperty('id')
    ]);

    public userFormGroup = new FormGroup({
        email: this.userEmailFormControl,
    });

    public inviteNewUser(): void {
        if (this.roleNameFormControl.errors?.['invalidProperty'] || this.vendorNameFormControl.errors?.['invalidProperty']) {
            this.roleNameFormControl.markAllAsTouched();
            this.userFormGroup.markAllAsTouched();

            return;
        };
        this.userService.userInvite({
            email: this.userEmailFormControl.value
        } as IInviteUserRequest, this.callback.bind(this));
    }

    public discard(): void {
        this.dialogRef.close(undefined);
    }

    public callback(): void {
        this.dialogRef.close(true);
    }
}

