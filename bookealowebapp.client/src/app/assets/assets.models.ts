import { AssetType } from "../calendars/calendars.model";

export interface Asset {
    id: number;
    name: string;
    description: string;
    type: AssetType;
}