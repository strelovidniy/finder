import { Injectable } from '@angular/core';


@Injectable({
    providedIn: 'root'
})
export default class ViewService {
    public isTableView(key: string): boolean {
        return !!localStorage.getItem(key) && localStorage.getItem(key) === 'table';
    }

    public setTableView(key: string, value: boolean): void {
        localStorage.setItem(key, value ? 'table' : 'card');
    }
}
