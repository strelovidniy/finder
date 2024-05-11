import INotificationsConfig from '../notifications-config/notifications-config.interface';
import IRole from '../role/role.interface';
import IUserDetails from '../user/user-details.interface';


interface IUserMe {
    access: IRole;
    email: string;
    firstName: string;
    id: string;
    lastName: string;
    details?: IUserDetails;
    notificationsConfig?: INotificationsConfig;
}

export default IUserMe;
