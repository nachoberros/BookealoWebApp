import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Calendar, CalendarType } from './calendars.model';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
    selector: 'app-calendars',
    standalone: true,
    imports: [CommonModule, RouterModule, MatMenuModule, MatIconModule, MatButtonModule],
    templateUrl: './calendars.component.html',
    styleUrls: ['./calendars.component.css']
})
export class CalendarsComponent implements OnInit {
    public CalendarType = CalendarType;
    calendars: Calendar[] = [];
    loading: boolean = false;
    showSuccessAlert = false;

    constructor(
        private http: HttpClient,
        private router: Router,
        private clipboard: Clipboard
    ) { }

    ngOnInit(): void {
        this.refreshCalendars();
    }

    editCalendar(calendar: Calendar) {
        console.log('Edit', calendar);
        this.router.navigate(['/calendars/edit', calendar.id]);
    }

    deleteCalendar(calendar: Calendar) {
        if (!calendar) return;

        if (confirm(`Are you sure you want to delete "${calendar.name}"?`)) {
            this.calendars = this.calendars.filter(c => c.id !== calendar.id);

            const params = { id: calendar.id };

            this.http.delete('/api/calendar', { params }).subscribe({
                next: () => this.refreshCalendars(),
                error: err => console.error('Calendar deletion failed', err)
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

    copyPublicLink(calendar: Calendar) {
        const path = this.getUrlPathByCalendarType(calendar.type);
        const publicUrl = `https://localhost:4200/calendars/${path}/${calendar.id}`;
        this.clipboard.copy(publicUrl);
        this.showSuccessAlert = true;
        setTimeout(() => {
            this.showSuccessAlert = false;
        }, 3000);
    }

    copyAdminLink(calendar: Calendar) {
        const path = this.getUrlPathByCalendarType(calendar.type);
        const privateUrl = `https://localhost:4200/calendars/${path}/${calendar.id}?token=XYZ`;
        this.clipboard.copy(privateUrl);
        this.showSuccessAlert = true;
        setTimeout(() => {
            this.showSuccessAlert = false;
        }, 3000);
    }

    private getUrlPathByCalendarType(type: CalendarType): string {
        switch (type) {
            case CalendarType.Tennis:
                return 'tenniscalendar';
            case CalendarType.Barber:
                return 'barbercalendar';
            case CalendarType.CarRental:
                return 'carrentalcalendar';
            default:
                return 'calendar';
        }
    }
}
