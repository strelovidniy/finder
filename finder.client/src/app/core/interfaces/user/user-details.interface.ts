import IContactDetails from './contact-details.interface';
import IUserAddress from './user-address.interface';


export interface IUserDetails {
    imageUrl?: string;
    imageThumbnailUrl?: string;
    address?: IUserAddress;
    contactInfo?: IContactDetails;
}

export default IUserDetails;
