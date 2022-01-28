import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService, private router: Router) { }

    canActivate(): boolean {
        const canActivate = this.authService.isAuthenticated();

        if (!canActivate) {
            this.router.navigateByUrl('/');
        }
        
        return canActivate;
    }
}