import { Component } from '@angular/core';
import { Router } from '@angular/router';


@Component({
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.component.scss', './welcome-responsive.component.scss']
})
export default class WelcomeComponent {

    constructor(
        private router: Router,
    ) { }

    public signUp(): void {
        this.router.navigate(['/auth/sign-up']);
    }

    public login(): void {
        this.router.navigate(['/auth/login']);
    }
}
