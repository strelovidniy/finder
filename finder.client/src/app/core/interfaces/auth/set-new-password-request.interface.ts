interface ISetNewPasswordRequest {
    verificationCode: string;
    newPassword: string;
    confirmNewPassword: string;
}

export default ISetNewPasswordRequest;
