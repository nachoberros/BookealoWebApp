import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard.spec';

export const routes: Routes = [
  { path: '', redirectTo: 'tennis', pathMatch: 'full' },
  { path: 'login',  loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
  { path: 'about', loadComponent: () => import('./about/about.component').then(m => m.AboutComponent), canActivate: [AuthGuard] },
  { path: 'tennis', loadComponent:() => import('./tennis-booking//tennis-booking.component').then(m => m.TennisBookingComponent), canActivate: [AuthGuard]}
];