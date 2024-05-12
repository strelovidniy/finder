import OperationType from '../../enums/search-operation/operation-type.enum';


interface ICreateSearchOperationRequest {
    title: string;
    description: string;
    tags: string[];
    operationType: OperationType;
    showContactInfo: boolean;
    images: File[];
}

export default ICreateSearchOperationRequest;
