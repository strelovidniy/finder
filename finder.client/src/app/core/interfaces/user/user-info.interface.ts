import IRole from '../role/role.interface';


interface IUserInfo {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    access: IRole;
}

export default IUserInfo;
