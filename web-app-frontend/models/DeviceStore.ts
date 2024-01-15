import { Device } from "./device";

export interface DeviceStore {
    data: Device[];
    loading: boolean;
    error: Error | null;
    fetchData: () => Promise<void>;
}