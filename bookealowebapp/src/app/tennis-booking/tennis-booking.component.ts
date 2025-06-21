import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TennisBookingSearchComponent } from './tennis-booking-search/tennis-booking-search.component';
import { TennisBookingListComponent } from './tennis-booking-list/tennis-booking-list.component';
import { CommonModule } from '@angular/common';
import { Court } from './tennis-booking-model'; // Adjust path as needed

@Component({
  selector: 'app-tennis-booking',
  standalone: true,
  imports: [CommonModule, TennisBookingSearchComponent, TennisBookingListComponent],
  templateUrl: './tennis-booking.component.html'
})
export class TennisBookingComponent {
  results: Court[] = [];
  loading: boolean = true;
  selectedDate: string = '';

  constructor(private http: HttpClient) { }

  onSearch(date: string) {
    this.loading = true;
    this.selectedDate = date;
    this.http.get<Court[]>(`/api/tennisbooking?date=${date}`).subscribe({
      next: data => {
        this.results = data;
        setTimeout(() => this.loading = false, 500);
      },
      error: error => {
        console.error('Failed to fetch court data', error);
        this.loading = false;
      }
    });
  }
}