import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Calendar } from "../calendars/calendars.model";

@Injectable({
  providedIn: 'root',
})
export class CalendarService {
  constructor(private http: HttpClient) {

  }

  getCurrentCalendar(calendarId: string): Calendar | null {
    this.http.get<Calendar[]>(`/api/calendar/${calendarId}`).subscribe({
      next: data => {
        return data;
      },
      error: error => {
        console.error('Failed to fetch court data', error);
      }
    });

    return null;
  }
}