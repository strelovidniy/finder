interface IQeryParams {
    searchQuery?: string;
    sortBy?: string;
    expandProperty?: string;
    sortAscending?: boolean | null;
    pageNumber?: number;
    pageSize?: number;
}

export default IQeryParams;

