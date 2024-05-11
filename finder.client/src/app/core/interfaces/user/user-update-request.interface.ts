interface IUserUpdateRequest {
    id: string;
    firstName: string;
    lastName: string;
    receiveCommentNotifications?: boolean;
}

export default IUserUpdateRequest;
