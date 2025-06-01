import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard.spec';

export const routes: Routes = [
  { path: '', redirectTo: 'tennis', pathMatch: 'full' },
  { path: 'login',  loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
  { path: 'calendars', loadComponent: () => import('./calendars/calendars.component').then(m => m.CalendarsComponent), canActivate: [AuthGuard] },
  { path: 'calendars/edit/:calendarId', loadComponent: () => import('./calendars/edit-calendar/edit-calendar.component').then(m => m.EditCalendarComponent), canActivate: [AuthGuard] },
  { path: 'calendars/new',  loadComponent: () => import('./calendars/edit-calendar/edit-calendar.component').then(m => m.EditCalendarComponent), canActivate: [AuthGuard] },
  { path: 'tennis', loadComponent:() => import('./tennis-booking//tennis-booking.component').then(m => m.TennisBookingComponent), canActivate: [AuthGuard]}
];