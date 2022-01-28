import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserRegistration } from '../../models/user-registration.model';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit { 
  public registerForm: FormGroup;
  
  constructor(private authService: AuthService, private router: Router, private notificationService: NotificationService) { }

  ngOnInit() {
    this.buildForm();
    if (this.authService.isAuthenticated()) {
      this.router.navigateByUrl('/chatroom');
    }
  }

  private buildForm() {
    this.registerForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('', [Validators.required])
    });
  }

  validateControl(controlName: string) {
    return this.registerForm.controls[controlName].invalid && this.registerForm.controls[controlName].touched
  }

  hasError(controlName: string, errorName: string) {
    return this.registerForm.controls[controlName].hasError(errorName)
  }

  goToLogin() {
    this.router.navigateByUrl('/');
  }

  registerUser(registerFormValue) {
    this.notificationService.showLoading();
    const formValues = { ...registerFormValue };
    const user: UserRegistration = {
      userName: formValues.userName,
      password: formValues.password,
      confirmPassword: formValues.confirm
    };
    this.authService.registerUser(user).then(() => {
      this.notificationService.showSuccessMessage('Registered successfully');
      this.router.navigateByUrl('/');
    }).catch((error: HttpErrorResponse) => {
      this.notificationService.showErrorMessages(error.error.errors, 'Some errors occurred while registering');
    });
  }
}
