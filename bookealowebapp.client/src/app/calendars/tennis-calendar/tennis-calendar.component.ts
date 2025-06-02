import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TennisCalendarSearchComponent } from './tennis-calendar-search/tennis-calendar-search.component';
import { TennisCalendarAdminListComponent } from './tennis-calendar-admin-list/tennis-calendar-admin-list.component';
import { CommonModule } from '@angular/common';
import { Court } from './tennis-calendar.model';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Calendar } from '../calendars.model';

@Component({
    selector: 'app-tennis-calendar',
    standalone: true,
    imports: [CommonModule, TennisCalendarSearchComponent, TennisCalendarAdminListComponent],
    templateUrl: './tennis-calendar.component.html'
})
export class TennisCalendarComponent implements OnInit {
    isAuthenticated = false;
    calendarId: string = '';
    courts: Court[] | null = null;
    loading: boolean = true;
    selectedDate: string = '';

    constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }


    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('calendarId');

        const parsedId = Number(idParam);
        if (!idParam || isNaN(parsedId)) {
            console.error('Invalid calendar ID');
            this.router.navigate(['/not-found']);
            return;
        }

        this.calendarId = parsedId.toString();

        const token = localStorage.getItem('jwtToken');
        this.isAuthenticated = !!token;
    }

    onSearch(date: string) {
        this.loading = true;
        this.selectedDate = date;

         this.http.get<Court[]>(`/api/tenniscalendar?calendarId=${this.calendarId}&date=${date}`).subscribe({
            next: data => {
                 this.courts = data;

                setTimeout(() => this.loading = false, 500);
            },
            error: error => {
                console.error('Failed to fetch court data', error);
                this.loading = false;
            }
        });
    }
}