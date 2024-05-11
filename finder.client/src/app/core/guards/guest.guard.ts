import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import AuthenticationService from '../services/authentication.service';

@Injectable({
    providedIn: 'root'
})
export default class GuestGuard implements CanActivate {

    constructor(
        private readonly auth: AuthenticationService,
        private readonly router: Router
    ) { }

    public canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        ;

        if (this.auth.isAuthenticated()) {
            this.router.navigate(['/search-operations']);


            return false;
        } else {
            return true;
        }
    }
}
