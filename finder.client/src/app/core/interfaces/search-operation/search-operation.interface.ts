import OperationType from '../../enums/search-operation/operation-type.enum';
import ISearchOperationContactInfo from './search-operation-contact-info.interface';
import ISearchOperationImage from './search-operation-image.interface';


interface ISearchOperation {
    id: string;
    title: string;
    description: string;
    tags: string[];
    issuerName: string;
    issuerImage: string;
    operationType: OperationType;
    issuerImageThumbnail: string;
    applicantsCount: number;
    images: ISearchOperationImage[];
    contactInfo: ISearchOperationContactInfo;
    createdAt: Date;
}

export default ISearchOperation;
