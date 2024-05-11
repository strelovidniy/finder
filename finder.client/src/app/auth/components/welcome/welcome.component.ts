import { Component } from '@angular/core';
import { Router } from '@angular/router';
import NotifierService from 'src/app/core/services/notifier.service';


@Component({
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.component.scss', './welcome-responsive.component.scss']
})
export default class WelcomeComponent {

    constructor(
        private readonly router: Router,
        private readonly notifier: NotifierService
    ) { }

    public signUp(): void {
        this.router.navigate(['/auth/sign-up']);
    }

    public login(): void {
        this.router.navigate(['/auth/login']);
    }

    public loginWithDiia() {
        this.notifier.error($localize`This feature is not available yet.`)
    }
}
