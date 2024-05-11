import IUserRole from './user-role.interface';


interface IUser {
    firstName: string;
    lastName: string;
    fullName: string;
    email: string;
    passwordHash: string;
    refreshToken: string;
    refreshTokenExpiresAt: string;
    role: IUserRole;
    id: string;
    createdAt: string;
    updatedAt: string;
    vendorId: string;
}

export default IUser;
