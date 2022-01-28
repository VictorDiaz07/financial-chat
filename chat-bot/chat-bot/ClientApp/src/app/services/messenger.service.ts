import { HttpErrorResponse } from '@angular/common/http';
import { EventEmitter, Inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { ClientMessage } from '../models/client-message.model';
import { ConnectedUser } from '../models/connected-user.model';
import { tokenGetter } from '../shared/functions/token-getter';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})
export class MessengerService {
    private connection: signalR.HubConnection;
    connectedUsers = new EventEmitter<ConnectedUser[]>();
    currentMessages = new EventEmitter<ClientMessage[]>();
    newMessage = new EventEmitter<ClientMessage>();

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private toastr: ToastrService,
        private authService: AuthService) {
        this.connection = new signalR.HubConnectionBuilder().withUrl(`${this.baseUrl}hub/chat`, { accessTokenFactory: tokenGetter }).build();
        this.startConnection();
     }

    private startConnection() {
        this.connection.serverTimeoutInMilliseconds = 36000000;
        this.connection.keepAliveIntervalInMilliseconds = 1800000;

        this.connection.start().then(() => {
            this.receiveConnectedUsers();
            this.receiveCurrentMessages();
            this.receiveMessage();
            this.toastr.success("", "Connected", {
                positionClass: 'toast-bottom-right'
            });
        }).catch((error: HttpErrorResponse) => {
            this.toastr.error(error.error, 'Error connecting to chatroom', {
                positionClass: 'toast-bottom-right'
            });
        });
    }

    private receiveMessage() {
      this.connection.on("NewMessage", (message: ClientMessage) => {
          this.newMessage.emit(message);
       });
    }

    private receiveConnectedUsers() {
        this.connection.on("ConnectedUsersChanged", (response: ConnectedUser[]) => {
            this.connectedUsers.emit(response);
        });
    }

    private receiveCurrentMessages() {
        this.connection.on("CurrentMessages", (messages: ClientMessage[]) => {
            this.currentMessages.emit(messages)
        });
    }

    closeConnectionForCurrentClient() {
        const userName = this.authService.getCurrentUser().userName;
        this.connection.invoke("DisconnectUser", userName).then(() => {
            this.authService.logout();
        }).catch(() => {
            this.toastr.error("An error occurred while loging you out.", "Error", {
                positionClass: 'toast-bottom-right'
            });
        });
    }

    sendNewMessage(message: ClientMessage) {
        return this.connection.invoke("SendMessage", message);
    }

    saveBotMessage(message: ClientMessage) {
        return this.connection.invoke("SaveBotMessage", message);
    }
}