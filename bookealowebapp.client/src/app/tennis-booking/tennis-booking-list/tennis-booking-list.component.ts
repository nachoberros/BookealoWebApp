import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Court } from '../tennis-booking-model';
import { User, UserService } from '../../services/user.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-tennis-booking-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tennis-booking-list.component.html',
  styleUrls: ['./tennis-booking-list.component.css']
})
export class TennisBookingListComponent {
  @Input() courts: any[] = [];
  @Input() isLoading: boolean = true;
  @Input() selectedDate: string = '';
  

  currentUser: User | null = null;
  timeSlots: string[] = [];
  selectedCourt: Court | null = null;
  selectedTime: string | null = null;
  showModal: boolean = false;
  showSuccessAlert = false;

  constructor(private http: HttpClient, private userService: UserService) { }

  ngOnInit(): void {
    const start = new Date();
    start.setHours(10, 0, 0, 0);

    for (let i = 0; i < 24; i++) {
      const slot = new Date(start.getTime() + i * 30 * 60000);
      this.timeSlots.push(slot.toTimeString().slice(0, 5)); // e.g. "10:00"
    }

    this.userService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  confirmBooking(court: Court | null, time: string | null) {
    if (!court || !time) return;

    const payload = {
      CourtId: court.id,
      Date: this.parseTimeToDate(time).toLocaleString('sv-SE').replace(' ', 'T'),
      UserId: this.currentUser?.id
    };

    this.http.post('/api/tennisbooking', payload).subscribe({
      next: () => {
        this.showModal = false;
        this.showSuccessAlert = true;
        this.refreshBookings();

        setTimeout(() => {
          this.showSuccessAlert = false;
        }, 3000);
      },
      error: (err: any) => {
        console.error('Booking failed', err);
        this.showModal = false;

      }
    });
  }
  
refreshBookings() {
  if (!this.selectedDate) return;

  this.http
    .get<Court[]>(`/api/tennisbooking?date=${this.selectedDate}`)
    .subscribe(data => {
      this.courts = data;
    });
}

  onAvailableCourtClick(court: Court, time: string) {
    this.selectedCourt = court;
    this.selectedTime = time;
    this.showModal = true;
  }

  getBookingName(court: Court, time: string): string | null {
    const target = this.parseTimeToDate(time);

    const booking = court.bookings.find(b => {
      const bookingTime = new Date(b.date);
      return bookingTime.getHours() === target.getHours() &&
        bookingTime.getMinutes() === target.getMinutes();
    });

    return booking ? booking.user.name : null;
  }

  private parseTimeToDate(time: string): Date {
    const [hours, minutes] = time.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
  }
}
