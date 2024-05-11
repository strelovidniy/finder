import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import AuthenticationService from 'src/app/core/services/authentication.service';


@Component({
    selector: 'finder-pagenotfound',
    templateUrl: './pagenotfound.component.html',
    styleUrls: ['./pagenotfound.component.scss']
})
export default class PagenotfoundComponent implements OnInit {

    constructor(
        private readonly authenticationService: AuthenticationService,
        private readonly router: Router,
    ) { }

    public ngOnInit(): void {
        if (this.authenticationService.isAuthenticated()) {
            this.router.navigate(['/search-operations']);
        } else {
            this.router.navigate(['/auth/welcome']);
        }
    }

}
