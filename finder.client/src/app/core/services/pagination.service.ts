import { Injectable } from '@angular/core';

import IQeryParams from '../interfaces/system/query-params.interface';


@Injectable({
    providedIn: 'root'
})
export default class PaginationService {

    public get pageSizeOptions(): number[] {
        return [10, 20, 50, 100];
    }

    public combineQuery(queries: string[]): string {

        return queries.filter((elem: string): boolean => elem !== '').join('&');
    };

    public getSortDirection(value: string): boolean | null {
        if (value === 'asc') {
            return true;
        }

        if (value === 'desc') {
            return false;
        }

        return null;
    }

    public queryBuilder(params: IQeryParams): string {
        const query: string[] = [];

        for (const key in params) {
            if (params.hasOwnProperty(key) && (params as any)[key] !== null && (params as any)[key] !== '') {
                query.push(`${key}=${(params as any)[key]}`);
            }
        }


        return `?${this.combineQuery(query)}`;
    }

}
