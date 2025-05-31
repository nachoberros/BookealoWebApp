import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Calendar } from './candelars.model';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-calendars',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './calendars.component.html',
    styleUrls: ['./calendars.component.css']
})
export class CalendarsComponent implements OnInit {
    calendars: Calendar[] = [];
    loading: boolean = false;

    constructor(private http: HttpClient, private router: Router) { }
    ngOnInit(): void {
        this.loading = true;
        this.http.get<Calendar[]>(`/api/calendar`).subscribe({
            next: data => {
                this.calendars = data;
                setTimeout(() => this.loading = false, 500);
            },
            error: error => {
                console.error('Failed to fetch court data', error);
                this.loading = false;
            }
        });
    }

    editCalendar(calendar: Calendar) {
        console.log('Edit', calendar);
        this.router.navigate(['/calendars/edit', calendar.id]);
    }

    deleteCalendar(calendar: Calendar) {

        if (!calendar) return;

        if (confirm('Are you sure you want to delete "${calendar.name}"?')) {
            this.calendars = this.calendars.filter(c => c.id !== calendar.id);

            const params = {
                id: calendar.id
            };

            this.http.delete('/api/calendar', { params }).subscribe({
                next: () => {
                    this.refreshCalendars();
                },
                error: (err: any) => {
                    console.error('Calendar deletion failed', err);
                }
            });
        }
    }

    refreshCalendars() {
        this.loading = true;
        this.http.get<Calendar[]>(`/api/calendar`).subscribe({
            next: data => {
                this.calendars = data;
                setTimeout(() => this.loading = false, 500);
            },
            error: error => {
                console.error('Failed to fetch court data', error);
                this.loading = false;
            }
        });
    }
}