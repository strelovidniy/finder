interface ICompleteRegistrationRequest {
    registrationToken: string;
    password: string;
    confirmPassword: string;
    firstName: string;
    lastName: string;
}

export default ICompleteRegistrationRequest;
