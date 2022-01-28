import { Routes } from '@angular/router';
import { AccountLayoutComponent } from './layouts/account-layout/account-layout.component';
import { SiteLayoutComponent } from './layouts/site-layout/site-layout.component';
import { ChatRoomComponent } from './pages/chat-room/chat-room.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { AuthGuard } from './shared/guards/auth.guard';

export const AppRoutes: Routes = [
    {
        path: '',
        component: AccountLayoutComponent,
        children: [
            {
                path: '',
                component: LoginComponent
            },
            {
                path: 'register',
                component: RegisterComponent
            }
        ]
    },
    {
        path: '',
        component: SiteLayoutComponent,
        children: [
            {
                path: 'chatroom',
                canActivate: [AuthGuard],
                component: ChatRoomComponent
            }
        ]
    },
    {
        path: '**',
        component: LoginComponent
    }
];