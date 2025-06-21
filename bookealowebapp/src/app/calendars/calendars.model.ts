import { User } from "../users/users.model";
import { AssetBooking } from "./tennis-calendar/tennis-calendar.model";

export interface Calendar {
    id: number;
    name: string;
    type: CalendarType;
    users: User[];
    assets: Asset[];
    startDate?: string;
    endDate?: string;
    startTime?: string;
    endTime?: string;
    isOnSaturday: boolean;
    isOnSunday: boolean;
    saturdayStartTime?: string;
    saturdayEndTime?: string;
    sundayStartTime?: string;
    sundayEndTime?: string;
}

export enum CalendarType {
    Tennis,
    Barber,
    CarRental
}

export interface Asset {
    id: number;
    name: string;
    type: AssetType;
    description: string;
    bookings: AssetBooking[];
    blockings: AssetBooking[];
}

export enum AssetType {
    TennisCourt,
    BarberSeat,
    Vehicle
}