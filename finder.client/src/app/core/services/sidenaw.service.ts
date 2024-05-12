import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export default class SideNavService {
    public isAdminMenuExpanded: boolean = false;
    public isWidgetsMenuExpanded: boolean = false;
    public isSideNavOpened: boolean = false;

    public contentTransparencySubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public searchFieldSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public searchFieldChangeSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

    public constructor() {
        this.isSideNavOpened = window.innerWidth > 768;
    }

    public setContentTransparency(isTransparent: boolean): void {
        this.contentTransparencySubject.next(isTransparent);
    }

    public enableSearchField(): void {
        this.searchFieldSubject.next(true);
    }

    public disableSearchField(): void {
        this.searchFieldSubject.next(false);
    }
}
