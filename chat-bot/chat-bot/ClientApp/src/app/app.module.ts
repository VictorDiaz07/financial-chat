import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { AccountLayoutComponent } from './layouts/account-layout/account-layout.component';
import { SiteLayoutComponent } from './layouts/site-layout/site-layout.component';
import { ChatRoomComponent } from './pages/chat-room/chat-room.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { tokenGetter } from './shared/functions/token-getter';
import { AuthGuard } from './shared/guards/auth.guard';
import { TokenInterceptor } from './shared/helpers/token-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ChatRoomComponent,
    SiteLayoutComponent,
    AccountLayoutComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(AppRoutes),
    ReactiveFormsModule,
    SweetAlert2Module,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    }),
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
