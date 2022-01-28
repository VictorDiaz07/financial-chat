import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CurrentUser } from '../models/current-user.model';
import { UserLoginResponse } from '../models/user-login-response.model';
import { UserLogin } from '../models/user-login.model';
import { UserRegistrationResponse } from '../models/user-registration-response.model';
import { UserRegistration } from '../models/user-registration.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient , @Inject('BASE_URL') private baseUrl: string, private jwtHelper: JwtHelperService, private router: Router) { }

  registerUser(request: UserRegistration) {
    return this.http.post<UserRegistrationResponse>(`${this.baseUrl}api/accounts/register`, request).toPromise();
  }

  login(request: UserLogin) {
    return this.http.post<UserLoginResponse>(`${this.baseUrl}api/accounts/login`, request).toPromise();
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['']);
  }

  getCurrentUser() {
    return this.jwtHelper.decodeToken(localStorage.getItem('token')) as CurrentUser;
  }

  isAuthenticated() {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }
}