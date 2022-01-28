import { Injectable } from '@angular/core';
import { error } from 'protractor';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor() { }

  showWarningMessage(message: string) {
    Swal.fire({
      icon: 'warning',
      text: message
    });
  }

  showErrorMessage(message: string) {
    Swal.fire({
      icon: 'error',
      text: message
    });
  }

  showErrorMessages(errors: string[], title: string) {
    const notificationBody = this.getErrorsAsHTMLList(errors);

    Swal.fire({
      title: title,
      icon: 'error',
      html: notificationBody
    });
  }

  private getErrorsAsHTMLList(errors: string[]) {
    let response = '<ul>';
    errors.forEach(error => { response += `<li class="text-danger">${error}</li>`});
    return response + '</ul>';
  }

  showSuccessMessage(message: string) {
    Swal.fire({
      icon: 'success',
      text: message
    });
  }

  showInfoMessage(message: string) {
    Swal.fire({
      icon: 'info',
      text: message
    });
  }

  showLoading() {
    Swal.fire({
      allowOutsideClick: false,
      showConfirmButton: false,
      onBeforeOpen: () => {
        Swal.showLoading();
      }
    });
  }

  closeLoading() {
    Swal.close();
  }
}