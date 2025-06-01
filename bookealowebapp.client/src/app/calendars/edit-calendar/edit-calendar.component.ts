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

    // ✅ Injected services using Angular v14+ `inject()` API (alternative to constructor DI)
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
        const request = this.calendarId
            ? this.http.put(`/api/calendar/${this.calendarId}`, this.calendar)
            : this.http.post(`/api/calendar`, this.calendar);

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
}
