import { Routes } from '@angular/router';
import { AuthPageComponent } from './pages/auth-page/auth-page.component';
import { accessGuard } from './guards/access.guard';
import { HomePageComponent } from './pages/home-page/home-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full'},
    { path: 'home', component: HomePageComponent, data: { requiresAuth: true }, canActivate: [accessGuard]},
    { path: 'login', component: AuthPageComponent, data: { requiresAnon: true}, canActivate: [accessGuard] },
    { path: '**', redirectTo: '', canActivate: [accessGuard]}
];
