import { User } from "../services/user.service";

export interface CourtBooking {
    id: number;
    date: string;
    user: User;
}

export interface Court {
    id: number;
    name: string;
    description: string;
    bookings: CourtBooking[];
}