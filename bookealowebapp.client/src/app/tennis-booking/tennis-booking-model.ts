export interface CourtBooking {
  date: string;
  userName: string;
}

export interface Court {
  name: string;
  description: string;
  bookings: CourtBooking[];
}