import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { User } from '../../../users/users.model';
import { Court, SlotDetail } from '../tennis-calendar.model';
import { UserService } from '../../../services/user.service';
import { CalendarService } from '../../../services/calendar.service';
import { Calendar } from '../../calendars.model';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tennis-calendar-public-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tennis-calendar-public-list.component.html',
  styleUrls: ['./tennis-calendar-public-list.component.css']
})
export class TennisCalendarPublicListComponent {

  @Input() isLoading: boolean = true;
  @Input() selectedDate: string = '';
  @Input() calendar: Calendar | null = null;
  @Input() courts: Court[] | null = null;

  currentUser: User | null = null;
  timeSlots: string[] = [];
  selectedCourt: Court | null = null;
  selectedTime: string | null = null;
  showAvailableModal: boolean = false;
  showSuccessAlert = false;
  showBookedModal: boolean = false;
  showEnterDetailsModal: boolean = false;
  showEnterDetailsConfirmationModal: boolean = false;
  public guestUserEmail: string | null = null;
  public guestName: string | null = null;

  constructor(private http: HttpClient, private userService: UserService, private calendarService: CalendarService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.currentUser = this.userService.getCurrentUser();
    this.guestUserEmail = this.route.snapshot.queryParamMap.get('guestEmail');
  }

  ngOnChanges(): void {
    if (this.calendar) {
      this.generateTimeSlots();
    }
  }

  getSlotDetails(court: Court, time: string): SlotDetail | null {
    const selectedDate = this.parseTimeToDate(time);

    const booking = this.FindCourtBooking(court, selectedDate);
    let isMyBooking = false;
    let description = '';

    let isBooked = booking ? true : false;

    if (this.guestUserEmail && this.guestUserEmail.toLowerCase() == booking?.user.email?.toLowerCase()) {
      description = booking?.user.name;
      isMyBooking = true; 
      isBooked = false;
    }

    const blocking = this.FindCourtBlocking(court, selectedDate);
    const isBlocked = blocking ? true : false;

    return { description, isBooked, isBlocked, isMyBooking };
  }

  confirmBookingDetails(court: Court | null, time: string | null, name: string | null, email: string | null) {
    if (!court || !time) return;

    const payload = {
      CalendarId: this.calendar?.id,
      AssetId: court.id,
      Date: this.parseTimeToDate(time).toLocaleString('sv-SE').replace(' ', 'T'),
      UserId: this.currentUser?.id,
      Name: name,
      Email: email
    };

    this.http.post('/api/tenniscalendar/public', payload).subscribe({
      next: () => {
        this.showEnterDetailsModal = false;
        this.showAvailableModal = false;
        this.showEnterDetailsConfirmationModal = true;
        this.guestUserEmail = email;
      },
      error: (err: any) => {
        console.error('Booking failed', err);
        this.showAvailableModal = false;
      }
    });
  }

  continueBooking(court: Court | null, time: string | null) {
    this.selectedCourt = court;
    this.selectedTime = time;
    this.showAvailableModal = false;
    this.showEnterDetailsModal = true;
  }

  cancelSlot(court: Court | null, time: string | null) {
    if (!court || !time || !this.currentUser || !this.calendar?.id) return;

    const dateString = this.parseTimeToDate(time);

    const params = {
      CalendarId: this.calendar?.id,
      CourtId: court.id,
      Date: this.parseTimeToDate(time).toLocaleString('sv-SE').replace(' ', 'T'),
      UserId: this.currentUser.id
    };

    this.http.delete('/api/tenniscalendar', { params }).subscribe({
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
      .get<Court[]>(`/api/tenniscalendar?calendarId=${this.calendar?.id}&date=${this.selectedDate}`)
      .subscribe(data => {
        this.courts = data;
      });
  }

  onAvailableSlotClick(court: Court, time: string) {
    this.selectedCourt = court;
    this.selectedTime = time;
    this.showAvailableModal = true;
  }

  onMyBookedSlotClick(court: Court, time: string) {
    const booking = this.FindCourtBooking(court, new Date(`${this.selectedDate}T${time}`));
    if (this.guestUserEmail && this.guestUserEmail.toLowerCase() == booking?.user.email?.toLowerCase()) {
      this.selectedCourt = court;
      this.selectedTime = time;
      this.showBookedModal = true;
    }
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
    if (detail.isMyBooking) return 'mybooking';
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

  generateTimeSlots(): void {
    this.timeSlots = [];

    const today = new Date().getDay(); // 0 = Sunday, 6 = Saturday
    let startTime: string | undefined;
    let endTime: string | undefined;

    if (today === 6 && this.calendar?.isOnSaturday) {
      startTime = this.calendar.saturdayStartTime;
      endTime = this.calendar.saturdayEndTime;
    } else if (today === 0 && this.calendar?.isOnSunday) {
      startTime = this.calendar.sundayStartTime;
      endTime = this.calendar.sundayEndTime;
    } else {
      startTime = this.calendar?.startTime;
      endTime = this.calendar?.endTime;
    }

    if (!startTime || !endTime) return;

    const [startHour, startMinute] = startTime.split(':').map(Number);
    const [endHour, endMinute] = endTime.split(':').map(Number);

    const start = new Date();
    start.setHours(startHour, startMinute, 0, 0);

    const end = new Date();
    end.setHours(endHour, endMinute, 0, 0);

    const current = new Date(start);

    while (current < end) {
      const time = current.toTimeString().slice(0, 5); // e.g., "09:30"
      this.timeSlots.push(time);
      current.setMinutes(current.getMinutes() + 30);
    }
  }
}
