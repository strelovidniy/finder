import IUser from '../user/user.interface';


interface IRole {
    id?: string;
    name?: string;
    type?: string;
    createdAt?: string;
    updatedAt?: string;
    users?: IUser[];
    canDeleteUsers: boolean;
    canRestoreUsers: boolean;
    canEditUsers: boolean;
    canCreateRoles: boolean;
    canEditRoles: boolean;
    canDeleteRoles: boolean;
    canSeeAllUsers: boolean;
    canSeeAllRoles: boolean;
    canSeeRoles: boolean;
    canMaintainSystem: boolean;
    canCreateHelpRequest: boolean;
    canSeeHelpRequests: boolean;
}

export default IRole;
