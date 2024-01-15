import { PrintJob } from "./printjob";

export interface PrintJobStore {
    data: PrintJob[];
    loading: boolean;
    error: Error | null;
    fetchData: () => Promise<void>;
}