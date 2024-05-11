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

    public constructor() {
        this.isSideNavOpened = window.innerWidth > 768;
    }

    public setContentTransparency(isTransparent: boolean): void {
        this.contentTransparencySubject.next(isTransparent);
    }
}
