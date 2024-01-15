import { Dashboard
 } from "./dashboard";
 
export interface DashbaordStore {
    data: Dashboard[];
    loading: boolean;
    error: Error | null;
    fetchData: () => Promise<void>;
}