import { User } from "../services/auth.service";

export interface Calendar {
  id: number;
  name: string;
  type: CalendarType;
  users: User[];
  assets: Asset[];
  startDate?: Date;
  endDate?: Date;
  startTime?: string;
  endTime?: string;
  isOnSaturday: boolean;
  isOnSunday: boolean;
  saturdayStartTime?: string;
  saturdayEndTime?: string;
  sundayStartTime?: string;
  sundayEndTime?: string;
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