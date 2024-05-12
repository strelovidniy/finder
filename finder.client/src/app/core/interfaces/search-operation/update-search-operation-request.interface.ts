import OperationType from '../../enums/search-operation/operation-type.enum';


interface IUpdateSearchOperationRequest {
    id: string;
    title: string;
    description: string;
    tags: string[];
    operationType: OperationType;
    showContactInfo: boolean;
    images: File[];
    imagesToDelete: string[];
}

export default IUpdateSearchOperationRequest;
