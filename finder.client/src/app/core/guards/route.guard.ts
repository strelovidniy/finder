import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import lodash from 'lodash';

import AuthenticationService from '../services/authentication.service';

import Role from '../enums/role/role.enum';

@Injectable({
    providedIn: 'root'
})
export default class RouteGuard implements CanActivate {

    constructor(
        private auth: AuthenticationService,
        private router: Router,
    ) { }

    public canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        const roleType: string[] = next.data?.['type'];
        const roles = next.data?.['roles'] as Role[];


        if (this.checkRoleType(roleType) && this.checkRoles(roles)) {

            return true;
        }

        return false;
    }

    private redirect(): void {
        if (this.auth.isAdmin) {
            this.router.navigate(['/admin/maintenance']);
        } else {
            this.router.navigate(['/requests']);
        }
    }

    private checkRoleType(roleType: string[]): boolean {
        const userRoleType = this.auth.getRoleType();

        if (roleType?.includes(userRoleType) || lodash.isEmpty(roleType)) {
            return true;
        }

        this.redirect();


        return false;
    }


    private checkRoles(roles: Role[]): boolean {

        if (!roles || !roles?.length) {
            return true;
        }

        const result = roles.map((elem: Role): boolean => this.auth.checkRole(elem));


        if (lodash.every(result)) {
            return true;
        }

        this.redirect();

        return false;
    }
}
