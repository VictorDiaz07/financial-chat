import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserLoginResponse } from '../../models/user-login-response.model';
import { UserLogin } from '../../models/user-login.model';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  
  constructor(private router: Router, private authService: AuthService, private notificationService: NotificationService) { }

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
      this.router.navigateByUrl('/chatroom');
    }
    this.buildForm();
  }

  private buildForm() {
    this.loginForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  validateControl(controlName: string) {
    return this.loginForm.controls[controlName].invalid && this.loginForm.controls[controlName].touched
  }

  hasError(controlName: string, errorName: string) {
    return this.loginForm.controls[controlName].hasError(errorName)
  }

  goToRegister() {
    this.router.navigateByUrl('/register');
  }

  loginUser(loginFormValue) {
    this.notificationService.showLoading();
    const login = {... loginFormValue };
    const userForAuth: UserLogin = {
      userName: login.userName,
      password: login.password
    };

    this.authService.login(userForAuth).then((response: UserLoginResponse) => {
      localStorage.setItem("token", response.token);
      this.notificationService.closeLoading();
      this.router.navigateByUrl('/chatroom');
    }).catch((error: HttpErrorResponse) => {
      this.notificationService.showErrorMessage(error.error.errorMessage);
    });
  }
}
