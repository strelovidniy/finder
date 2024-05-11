import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export default class EventBusService {
    public refreshUiSubject: Subject<void> = new Subject<void>();
}
