import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Court, SlotDetail } from '../tennis-booking-model';
import { UserService } from '../../services/user.service';
import { HttpClient } from '@angular/common/http';
import { User } from '../../users/users.model';

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
  showAvailableModal: boolean = false;
  showSuccessAlert = false;
  showUnblockedModal: boolean = false;
  showBookedModal: boolean = false;
  showUnblockSuccessAlert = false;

  constructor(private http: HttpClient, private userService: UserService) { }

  ngOnInit(): void {
    const start = new Date();
    start.setHours(10, 0, 0, 0);

    for (let i = 0; i < 24; i++) {
      const slot = new Date(start.getTime() + i * 30 * 60000);
      const time = slot.toTimeString().slice(0, 5); // e.g., "10:00"
      this.timeSlots.push(time); // e.g. "10:00"
    }

    this.currentUser = this.userService.getCurrentUser();
  }


  getSlotDetails(court: Court, time: string): SlotDetail | null {
    const selectedDate = this.parseTimeToDate(time);

    const booking = this.FindCourtBooking(court, selectedDate);
    const description = booking ? booking.user.name : "";
    const isBooked = description ? true : false;

    const blocking = this.FindCourtBlocking(court, selectedDate);
    const isBlocked = blocking ? true : false;

    return { description, isBooked, isBlocked };
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
        this.showAvailableModal = false;
        this.showSuccessAlert = true;
        this.refreshBookings();

        setTimeout(() => {
          this.showSuccessAlert = false;
        }, 3000);
      },
      error: (err: any) => {
        console.error('Booking failed', err);
        this.showAvailableModal = false;

      }
    });
  }

  blockSlot(court: Court | null, time: string | null) {
    if (!court || !time) return;

    const payload = {
      CourtId: court.id,
      Date: this.parseTimeToDate(time).toLocaleString('sv-SE').replace(' ', 'T'),
      UserId: this.currentUser?.id
    };

    this.http.put('/api/tennisbooking/block', payload).subscribe({
      next: () => {
        this.showAvailableModal = false;
        this.showSuccessAlert = true;
        this.refreshBookings();

        setTimeout(() => {
          this.showSuccessAlert = false;
        }, 3000);
      },
      error: (err: any) => {
        console.error('Booking failed', err);
        this.showAvailableModal = false;
      }
    });
  }

  unblockSlot(court: Court | null, time: string | null) {
    if (!court || !time) return;

    const payload = {
      CourtId: court.id,
      Date: this.parseTimeToDate(time).toLocaleString('sv-SE').replace(' ', 'T'),
      UserId: this.currentUser?.id
    };

    this.http.put('/api/tennisbooking/unblock', payload).subscribe({
      next: () => {
        this.showUnblockedModal = false;
        this.showUnblockSuccessAlert = true;
        this.refreshBookings();

        setTimeout(() => {
          this.showUnblockSuccessAlert = false;
        }, 3000);
      },
      error: (err: any) => {
        console.error('Booking failed', err);
        this.showUnblockedModal = false;
      }
    });
  }

  cancelSlot(court: Court | null, time: string | null) {
    if (!court || !time || !this.currentUser) return;

    const dateString = this.parseTimeToDate(time);

    const params = {
      CourtId: court.id,
      Date: this.parseTimeToDate(time).toLocaleString('sv-SE').replace(' ', 'T'),
      UserId: this.currentUser.id
    };

    this.http.delete('/api/tennisbooking', { params }).subscribe({
      next: () => {
        this.showBookedModal = false;
        this.showSuccessAlert = true;
        this.refreshBookings();

        setTimeout(() => {
          this.showSuccessAlert = false;
        }, 3000);
      },
      error: (err: any) => {
        console.error('Booking cancellation failed', err);
        this.showBookedModal = false;
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

  onAvailableSlotClick(court: Court, time: string) {
    this.selectedCourt = court;
    this.selectedTime = time;
    this.showAvailableModal = true;
  }
  
  onBookedSlotClick(court: Court, time: string) {
    this.selectedCourt = court;
    this.selectedTime = time;
    this.showBookedModal = true;
  }

  onBlockedSlotClick(court: Court, time: string) {
    this.selectedCourt = court;
    this.selectedTime = time;
    this.showUnblockedModal = true;
  }

  getBookingName(court: Court, time: string): string | null {
    const target = this.parseTimeToDate(time);
    const booking = this.FindCourtBooking(court, target);

    return booking ? booking.user.name : null;
  }

  getSlotStatus(detail: SlotDetail | null): string {
    if (!detail) return 'unknown';
    if (detail.isBlocked) return 'blocked';
    if (detail.isBooked) return 'booked';
    return 'available';
  }

  private parseTimeToDate(time: string): Date {
    const [hours, minutes] = time.split(':').map(Number);
    const baseDate = this.selectedDate ? new Date(this.selectedDate) : new Date();
    baseDate.setHours(hours, minutes, 0, 0);
    return baseDate;
  }

  private FindCourtBooking(court: Court, selectedDate: Date) {
    return court.bookings.find(b => {
      const bookingTime = new Date(b.date);
      return bookingTime.getHours() === selectedDate.getHours() &&
        bookingTime.getMinutes() === selectedDate.getMinutes();
    });
  }

  private FindCourtBlocking(court: Court, selectedDate: Date) {
    return court.blockings.find(b => {
      const bookingTime = new Date(b.date);
      return bookingTime.getHours() === selectedDate.getHours() &&
        bookingTime.getMinutes() === selectedDate.getMinutes();
    });
  }
}
