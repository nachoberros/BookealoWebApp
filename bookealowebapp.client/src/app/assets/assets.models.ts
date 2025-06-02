import { AssetType } from "../calendars/candelars.model";

export interface Asset {
    id: number;
    name: string;
    description: string;
    type: AssetType;
}