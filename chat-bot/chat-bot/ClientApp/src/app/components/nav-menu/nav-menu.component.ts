import { Component } from '@angular/core';
import { MessengerService } from '../../services/messenger.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(private messengerService: MessengerService) { }

  logout() {
    this.messengerService.closeConnectionForCurrentClient();
  }
}
