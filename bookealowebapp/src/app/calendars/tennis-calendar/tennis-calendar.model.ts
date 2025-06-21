import { User } from "../../users/users.model";

export interface AssetBooking {
    id: number;
    date: string;
    user: User;
}

export interface SlotDetail {
    description: string;
    isBooked: boolean;
    isBlocked: boolean;
    isMyBooking: boolean;
}

export interface Court {
    id: number;
    name: string;
    description: string;
    bookings: AssetBooking[];
    blockings: AssetBooking[];
}