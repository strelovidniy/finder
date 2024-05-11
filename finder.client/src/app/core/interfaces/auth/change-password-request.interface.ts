interface IChangePasswordRequest {
    oldPassword: string;
    newPassword: string;
    confirmNewPassword: string;
}

export default IChangePasswordRequest;
