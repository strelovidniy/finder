import IToken from './token.interface';
import IUserMe from './user-me.interface';


interface IAuthResponse {
    token: IToken;
    user: IUserMe;
}

export default IAuthResponse;
