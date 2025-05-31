import { User } from "../services/auth.service";

export interface Calendar {
    id: number;
    name: string;
    type: CalendarType;
    users: User[];
    assets: Asset[];
    startDate: Date;
    endDate: Date;
    startTime: Date;
    endTime: Date;
    isOnSaturday: boolean;
    isOnSunday: boolean;
    saturdayStartTime: Date;
    saturdayEndTime: Date;
    sundayStartTime: Date;
    sundayEndTime: Date;
}

export enum CalendarType
{
    Tennis,
    Barber,
    CarRental
}

export interface Asset
{
    id: number;
    name: string;
    type: AssetType;
}

export enum AssetType
{
    TennisCourt,
    BarberSeat,
    Vehicle
}