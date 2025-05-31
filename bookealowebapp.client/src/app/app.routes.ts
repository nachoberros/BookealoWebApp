import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard.spec';

export const routes: Routes = [
  { path: '', redirectTo: 'tennis', pathMatch: 'full' },
  { path: 'login',  loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
  { path: 'calendars', loadComponent: () => import('./calendars/calendars.component').then(m => m.CalendarsComponent), canActivate: [AuthGuard] },
  { path: 'tennis', loadComponent:() => import('./tennis-booking//tennis-booking.component').then(m => m.TennisBookingComponent), canActivate: [AuthGuard]}
];