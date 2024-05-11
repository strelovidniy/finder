import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import AuthenticationService from '../services/authentication.service';

@Injectable({
    providedIn: 'root'
})
export default class AuthGuard implements CanActivate {

    constructor(
        private readonly auth: AuthenticationService,
        private readonly router: Router,
    ) { }

    public canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

        const url: string = state.url;

        if (this.auth.isAuthenticated()) {
            return true;
        } else {
            this.auth.logout();
            this.router.navigate(['/auth/welcome']);

            return false;
        }
    }
}
