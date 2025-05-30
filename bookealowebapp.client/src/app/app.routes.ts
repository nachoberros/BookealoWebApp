import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'weather', pathMatch: 'full' },
  { path: 'about', loadComponent: () => import('./about/about.component').then(m => m.AboutComponent) },
  { path: 'weather', loadComponent: () => import('./weather/weather.component').then(m => m.WeatherComponent) },
  { path: 'tennis', loadComponent:() => import('./tennis-booking//tennis-booking.component').then(m => m.TennisBookingComponent)}
];