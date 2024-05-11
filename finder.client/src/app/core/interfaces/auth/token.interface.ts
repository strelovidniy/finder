interface IToken {
    expireAt: string;
    refreshToken?: string;
    refreshTokenExpireAt?: string;
    token: string;
}

export default IToken;
