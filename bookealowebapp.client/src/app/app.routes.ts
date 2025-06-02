import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard.spec';

export const routes: Routes = [
  { path: '', redirectTo: 'tennis', pathMatch: 'full' },
  { path: 'login',  loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
  { path: 'home',  loadComponent: () => import('./home/home.component').then(m => m.HomeComponent), canActivate: [AuthGuard] },
  { path: 'calendars', loadComponent: () => import('./calendars/calendars.component').then(m => m.CalendarsComponent), canActivate: [AuthGuard] },
  { path: 'calendars/edit/:calendarId', loadComponent: () => import('./calendars/edit-calendar/edit-calendar.component').then(m => m.EditCalendarComponent), canActivate: [AuthGuard] },
  { path: 'calendars/new',  loadComponent: () => import('./calendars/edit-calendar/edit-calendar.component').then(m => m.EditCalendarComponent), canActivate: [AuthGuard] },
  { path: 'users', loadComponent: () => import('./users/users.component').then(m => m.UsersComponent), canActivate: [AuthGuard] },
  { path: 'users/edit/:userId', loadComponent: () => import('./users/edit-user/edit-user.component').then(m => m.EditUserComponent), canActivate: [AuthGuard] },
  { path: 'users/new',  loadComponent: () => import('./users/edit-user/edit-user.component').then(m => m.EditUserComponent), canActivate: [AuthGuard] },
  { path: 'assets', loadComponent: () => import('./assets/assets.component').then(m => m.AssetsComponent), canActivate: [AuthGuard] },
  { path: 'assets/edit/:assetId', loadComponent: () => import('./assets/edit-asset/edit-asset.component').then(m => m.EditAssetComponent), canActivate: [AuthGuard] },
  { path: 'assets/new',  loadComponent: () => import('./assets/edit-asset/edit-asset.component').then(m => m.EditAssetComponent), canActivate: [AuthGuard] },
  { path: 'admin/tenniscalendar/:calendarId', loadComponent:() => import('./calendars/tennis-calendar/tennis-calendar.component').then(m => m.TennisCalendarComponent), canActivate: [AuthGuard]}
];