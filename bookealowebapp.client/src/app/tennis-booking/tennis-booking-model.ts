import { User } from "../users/users.model";

export interface CourtBooking {
    id: number;
    date: string;
    user: User;
}

export interface SlotDetail {
    description: string;
    isBooked: boolean;
    isBlocked: boolean;
}

export interface Court {
    id: number;
    name: string;
    description: string;
    bookings: CourtBooking[];
    blockings: CourtBooking[];
}