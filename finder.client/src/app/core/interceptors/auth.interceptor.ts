import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { catchError, switchMap } from 'rxjs/operators';

import AuthenticationService from '../services/authentication.service';
import NotifierService from '../services/notifier.service';

import StatusCode from '../enums/system/status-code.enum';

@Injectable()
export default class AuthInterceptor implements HttpInterceptor {
    constructor(
        private readonly auth: AuthenticationService,
        private readonly router: Router,
        private readonly notifier: NotifierService
    ) {
    }

    public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url.includes('refresh-token')) {
            return this.handleHttpRequest(req, next);
        }

        if (this.auth.isAuthenticated() && !this.auth.getToken()) {
            return this.auth.refreshToken().pipe(switchMap((_): Observable<HttpEvent<any>> => {
                const token = this.auth.getToken();

                if (token) {
                    req = this.setToken(req, token);
                }

                return this.handleHttpRequest(req, next);
            }));
        }


        if (this.auth.isAuthenticated()) {
            req = this.setToken(req, this.auth.getToken());
        }

        return this.handleHttpRequest(req, next);
    }

    private handleHttpRequest(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError((res: HttpErrorResponse): Observable<HttpEvent<any>> => {
                    if (res.status === 401) {
                        this.auth.logout();
                        this.router.navigate(['/auth/login']);
                    }

                    this.notifier.error(this.getStatusCodeErrorMessage(res?.error?.statusCode));


                    return throwError(res);
                })
            );
    }

    private setToken(req: HttpRequest<any>, token: string | null): HttpRequest<any> {
        return req.clone({
            setHeaders: {
                authorization: `Bearer ${token}`
            }
        });
    }

    private getStatusCodeErrorMessage(statusCode: StatusCode): string {
        switch (statusCode) {
            case StatusCode.methodNotAvailable:
                return $localize`Method not available`;
            case StatusCode.unauthorized:
                return $localize`Unauthorized`;
            case StatusCode.forbidden:
                return $localize`Forbidden`;
            case StatusCode.userNotFound:
                return $localize`User not found`;
            case StatusCode.roleNotFound:
                return $localize`Role not found`;
            case StatusCode.helperRoleNotFound:
                return $localize`Helper role not found`;
            case StatusCode.userRoleNotFound:
                return $localize`User role not found`;
            case StatusCode.directoryNotFound:
                return $localize`Directory not found`;
            case StatusCode.fileNotFound:
                return $localize`File not found`;
            case StatusCode.helpRequestNotFound:
                return $localize`Help request not found`;
            case StatusCode.imageNotFound:
                return $localize`Image not found`;
            case StatusCode.queryResultError:
                return $localize`Query result error`;
            case StatusCode.emailSendingError:
                return $localize`Email sending error`;
            case StatusCode.imageProcessingError:
                return $localize`Image processing error`;
            case StatusCode.userHasNoRole:
                return $localize`User has no role`;
            case StatusCode.fileHasAnUnacceptableFormat:
                return $localize`File has an unacceptable format`;
            case StatusCode.roleCannotBeUpdated:
                return $localize`Role cannot be updated`;
            case StatusCode.roleCannotBeDeleted:
                return $localize`Role cannot be deleted`;
            case StatusCode.userRoleCannotBeChanged:
                return $localize`User role cannot be changed`;
            case StatusCode.tagCannotBeEmpty:
                return $localize`Tag cannot be empty`;
            case StatusCode.firstNameTooLong:
                return $localize`First name too long`;
            case StatusCode.lastNameTooLong:
                return $localize`Last name too long`;
            case StatusCode.fileIsTooLarge:
                return $localize`File is too large`;
            case StatusCode.emailTooLong:
                return $localize`Email too long`;
            case StatusCode.addressLine1TooLong:
                return $localize`Address line 1 too long`;
            case StatusCode.addressLine2TooLong:
                return $localize`Address line 2 too long`;
            case StatusCode.cityTooLong:
                return $localize`City too long`;
            case StatusCode.stateTooLong:
                return $localize`State too long`;
            case StatusCode.postalCodeTooLong:
                return $localize`Postal code too long`;
            case StatusCode.countryTooLong:
                return $localize`Country too long`;
            case StatusCode.instagramTooLong:
                return $localize`Instagram too long`;
            case StatusCode.linkedInTooLong:
                return $localize`LinkedIn too long`;
            case StatusCode.phoneNumberTooLong:
                return $localize`Phone number too long`;
            case StatusCode.skypeTooLong:
                return $localize`Skype too long`;
            case StatusCode.telegramTooLong:
                return $localize`Telegram too long`;
            case StatusCode.otherTooLong:
                return $localize`Other too long`;
            case StatusCode.titleTooLong:
                return $localize`Title too long`;
            case StatusCode.descriptionTooLong:
                return $localize`Description too long`;
            case StatusCode.tagTooLong:
                return $localize`Tag too long`;
            case StatusCode.endpointTooLong:
                return $localize`Endpoint too long`;
            case StatusCode.authTooLong:
                return $localize`Auth too long`;
            case StatusCode.p256dhTooLong:
                return $localize`P256dh too long`;
            case StatusCode.incorrectPassword:
                return $localize`Incorrect password`;
            case StatusCode.passwordsDoNotMatch:
                return $localize`Passwords do not match`;
            case StatusCode.oldPasswordIncorrect:
                return $localize`Old password incorrect`;
            case StatusCode.userAlreadyExists:
                return $localize`User already exists`;
            case StatusCode.roleAlreadyExists:
                return $localize`Role already exists`;
            case StatusCode.emailAlreadyExists:
                return $localize`Email already exists`;
            case StatusCode.roleTypeRequired:
                return $localize`Role type required`;
            case StatusCode.roleIdRequired:
                return $localize`Role ID required`;
            case StatusCode.userIdRequired:
                return $localize`User ID required`;
            case StatusCode.lastNameRequired:
                return $localize`Last name required`;
            case StatusCode.firstNameRequired:
                return $localize`First name required`;
            case StatusCode.invitationTokenRequired:
                return $localize`Invitation token required`;
            case StatusCode.passwordRequired:
                return $localize`Password required`;
            case StatusCode.emailRequired:
                return $localize`Email required`;
            case StatusCode.roleNameRequired:
                return $localize`Role name required`;
            case StatusCode.verificationCodeRequired:
                return $localize`Verification code required`;
            case StatusCode.confirmPasswordRequired:
                return $localize`Confirm password required`;
            case StatusCode.addressLine1Required:
                return $localize`Address line 1 required`;
            case StatusCode.cityRequired:
                return $localize`City required`;
            case StatusCode.stateRequired:
                return $localize`State required`;
            case StatusCode.postalCodeRequired:
                return $localize`Postal code required`;
            case StatusCode.countryRequired:
                return $localize`Country required`;
            case StatusCode.imagesRequired:
                return $localize`Images required`;
            case StatusCode.descriptionRequired:
                return $localize`Description required`;
            case StatusCode.titleRequired:
                return $localize`Title required`;
            case StatusCode.authRequired:
                return $localize`Auth required`;
            case StatusCode.p256dhRequired:
                return $localize`P256dh required`;
            case StatusCode.KeysRequired:
                return $localize`Keys required`;
            case StatusCode.endpointRequired:
                return $localize`Endpoint required`;
            case StatusCode.filterTitlesRequired:
                return $localize`Filter titles required`;
            case StatusCode.filterTagsRequired:
                return $localize`Filter tags required`;
            case StatusCode.passwordLengthExceeded:
                return $localize`Password length exceeded`;
            case StatusCode.passwordMustHaveAtLeast8Characters:
                return $localize`Password must have at least 8 characters`;
            case StatusCode.passwordMustHaveNotMoreThan32Characters:
                return $localize`Password must have not more than 32 characters`;
            case StatusCode.passwordMustHaveAtLeastOneUppercaseLetter:
                return $localize`Password must have at least one uppercase letter`;
            case StatusCode.passwordMustHaveAtLeastOneLowercaseLetter:
                return $localize`Password must have at least one lowercase letter`;
            case StatusCode.passwordMustHaveAtLeastOneDigit:
                return $localize`Password must have at least one digit`;
            case StatusCode.invalidRoleType:
                return $localize`Invalid role type`;
            case StatusCode.invalidSortByProperty:
                return $localize`Invalid sort by property`;
            case StatusCode.invalidExpandProperty:
                return $localize`Invalid expand property`;
            case StatusCode.invalidEmailFormat:
                return $localize`Invalid email format`;
            case StatusCode.invalidEmailModel:
                return $localize`Invalid email model`;
            case StatusCode.invalidVerificationCode:
                return $localize`Invalid verification code`;
            case StatusCode.invalidFile:
                return $localize`Invalid file`;
            case StatusCode.invalidPhoneNumber:
                return $localize`Invalid phone number`;
            case StatusCode.invalidNotificationSettings:
                return $localize`Invalid notification settings`;
            case StatusCode.firstNameShouldNotContainWhiteSpace:
                return $localize`First name should not contain white space`;
            case StatusCode.lastNameShouldNotContainWhiteSpace:
                return $localize`Last name should not contain white space`;
            case StatusCode.helpRequestRemoved:
                return $localize`Help request removed`;
        }
    }
}
