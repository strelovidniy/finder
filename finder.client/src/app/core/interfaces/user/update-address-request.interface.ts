interface IUpdateAddressRequest {
    addressLine1: string;
    addressLine2?: string;
    city: string;
    state: string;
    postalCode: string;
    country: string;
}

export default IUpdateAddressRequest;
