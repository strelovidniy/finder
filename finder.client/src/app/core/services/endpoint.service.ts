import { Injectable } from '@angular/core';

import environment from 'src/environments/environment';


@Injectable({
    providedIn: 'root'
})
export default class EndpointService {
    // User
    public readonly deleteUser = (id: string): string => `${environment.apiUrl}users/${id}`;
    public readonly users = (query: string): string => `${environment.apiUrl}users${query}`;
    public readonly setUserRole = (): string =>`${environment.apiUrl}users/change-roles`;
    public readonly setNewPassword = (): string => `${environment.apiUrl}users/set-new-password`;
    public readonly refreshToken = (refreshToken: string): string => `${environment.apiUrl}users/refresh-token?refreshToken=${refreshToken}`;
    public readonly login = (): string => `${environment.apiUrl}users/login`;
    public readonly resetPassword = (): string => `${environment.apiUrl}users/reset-password`;
    public readonly changePassword = (): string => `${environment.apiUrl}users/change-password`;
    public readonly signUp = (): string => `${environment.apiUrl}users/sign-up`;
    public readonly confirmEmail = (): string => `${environment.apiUrl}users/confirm-email`;
    public readonly userUpdate = (): string => `${environment.apiUrl}users/update`;
    public readonly userInfo = (): string => `${environment.apiUrl}users/me`;
    public readonly addUserPushSubscription = (): string => `${environment.apiUrl}users/add-push-subscription`;
    public readonly userInvite = (): string => `${environment.apiUrl}invite`;

    // User Details
    public readonly uploadUserAvatar = (): string => `${environment.apiUrl}user-details/upload-avatar`;
    public readonly updateAddress = (): string => `${environment.apiUrl}user-details/update-address`;
    public readonly updateContactDetails = (): string => `${environment.apiUrl}user-details/update-contact-details`;

    // Role
    public readonly rolesList = (query?: string ): string => `${environment.apiUrl}roles${query}`;
    public readonly rolesAutocomplete = (searchQuery?: string ): string => `${environment.apiUrl}roles?searchQuery=${searchQuery}`;
    public readonly deleteRoleById = (id: string): string => `${environment.apiUrl}roles/${id}`;
    public readonly updateRole = (): string => `${environment.apiUrl}roles`;
    public readonly createRole = (): string => `${environment.apiUrl}roles`;

    // Admin Odata
    public readonly adminOdataUsersUrl = (query: string): string => `${environment.apiUrl}odata/users${query}`;
    public readonly adminOdataRolesUrl = (query: string): string => `${environment.apiUrl}odata/roles${query}`;

    // Help Request
    public readonly getHelpRequests = (query: string): string => `${environment.apiUrl}help-requests${query}`;
    public readonly getHelpRequest = (id: string): string => `${environment.apiUrl}help-requests/get?id=${id}`;
    public readonly createHelpRequest = (): string => `${environment.apiUrl}help-requests/create`;
    public readonly updateHelpRequest = (): string => `${environment.apiUrl}help-requests/update`;
    public readonly deleteHelpRequest = (id: string): string => `${environment.apiUrl}help-requests/delete?id=${id}`;

    // Notifications
    public readonly updateNotificationsConfig = (): string => `${environment.apiUrl}notification-settings/update`;

}
