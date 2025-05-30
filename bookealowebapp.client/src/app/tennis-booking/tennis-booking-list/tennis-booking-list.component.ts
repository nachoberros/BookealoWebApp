import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Court } from '../tennis-booking-model';

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

  timeSlots: string[] = [];

  
  ngOnInit(): void {
    const start = new Date();
    start.setHours(10, 0, 0, 0);

    for (let i = 0; i < 24; i++) {
      const slot = new Date(start.getTime() + i * 30 * 60000);
      this.timeSlots.push(slot.toTimeString().slice(0, 5)); // e.g. "10:00"
    }
  }

getBookingName(court: Court, time: string): string | null {
  const target = this.parseTimeToDate(time);

  const booking = court.bookings.find(b => {
    const bookingTime = new Date(b.date);
    return bookingTime.getHours() === target.getHours() &&
           bookingTime.getMinutes() === target.getMinutes();
  });

  return booking ? booking.userName : null;
}

private parseTimeToDate(time: string): Date {
  const [hours, minutes] = time.split(':').map(Number);
  const date = new Date();
  date.setHours(hours, minutes, 0, 0);
  return date;
}
}
