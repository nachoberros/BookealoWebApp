import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Calendar, CalendarType } from '../candelars.model';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
    selector: 'app-edit-calendar',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule, MatProgressSpinnerModule],
    templateUrl: './edit-calendar.component.html',
    styleUrls: ['./edit-calendar.component.css']
})
export class EditCalendarComponent implements OnInit {
    public CalendarType = CalendarType;
    calendarId!: string;
    calendar: Calendar = {
        id: 0, name: '',
        type: CalendarType.Tennis,
        users: [],
        assets: [],
        startDate: undefined,
        endDate: undefined,
        startTime: undefined,
        endTime: undefined,
        isOnSaturday: false,
        isOnSunday: false,
        saturdayStartTime: undefined,
        saturdayEndTime: undefined,
        sundayStartTime: undefined,
        sundayEndTime: undefined
    };
    loading: boolean = false;

    private route = inject(ActivatedRoute);
    private http = inject(HttpClient);
    private router = inject(Router);

    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('calendarId');

        if (idParam) {
            this.calendarId = idParam;
            this.loading = true;
            this.http.get<Calendar>(`/api/calendar/${this.calendarId}`).subscribe({
                next: data => {
                    this.formatBackendDates(data);
                    this.calendar = data;
                    this.loading = false;
                },
                error: err => {
                    console.error('Failed to load calendar', err);
                    this.loading = false;
                }
            });
        }
    }

    saveCalendar() {
        if (this.calendar.type !== CalendarType.Tennis) {
            alert('Only Tennis calendars can be saved from this form.');
            return;
        }
        const payload = {
            ...this.calendar,
            sundayStartTime: this.formatTimeString(this.calendar.sundayStartTime),
            sundayEndTime: this.formatTimeString(this.calendar.sundayEndTime),
            saturdayStartTime: this.formatTimeString(this.calendar.saturdayStartTime),
            saturdayEndTime: this.formatTimeString(this.calendar.saturdayEndTime),
            startTime: this.formatTimeString(this.calendar.startTime),
            endTime: this.formatTimeString(this.calendar.endTime)
        };
        const request = this.calendarId
            ? this.http.put(`/api/calendar`, payload)
            : this.http.post(`/api/calendar`, payload);

        request.subscribe({
            next: () => {
                alert(`Calendar ${this.calendarId ? 'updated' : 'created'} successfully!`);
                this.router.navigate(['/calendars']);
            },
            error: err => {
                console.error('Failed to save calendar', err);
            }
        });
    }

    cancel() {
        this.router.navigate(['/calendars']);
    }

    private formatTimeString(time: string | undefined): string | undefined {
        if (!time) return undefined;

        // Already in HH:mm:ss format
        if (/^\d{2}:\d{2}:\d{2}$/.test(time)) {
            return time;
        }

        // In HH:mm format — convert to HH:mm:ss
        if (/^\d{2}:\d{2}$/.test(time)) {
            return `${time}:00`;
        }

        console.warn(`Invalid time format: ${time}`);
        return undefined;
    }

    private formatDateToLocalInput(dateString: string): string {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    private formatBackendDates(data: Calendar) {
        if (data.endDate) {
            data.endDate = this.formatDateToLocalInput(data.endDate);
        }
        if (data.startDate) {
            data.startDate = this.formatDateToLocalInput(data.startDate);
        }
    }
}
