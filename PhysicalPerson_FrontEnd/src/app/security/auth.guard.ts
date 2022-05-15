import { Injectable } from '@angular/core';
import { Router,CanActivate,ActivatedRouteSnapshot } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UtilsService } from '../service/utils.service';

@Injectable({
    providedIn:'root'
})
export class AuthGuard implements CanActivate {
    constructor(private _route:Router,private _Cookie: CookieService ){}

    canActivate(route:ActivatedRouteSnapshot){
        // if (this._Cookie.get('sToken') !==null){
            return true;
        // }
        // this._route.navigate(['login']);
        // return false;
    }
}