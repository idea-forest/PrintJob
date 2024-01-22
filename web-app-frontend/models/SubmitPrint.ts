export interface SubmitPrintStore {
    printstore: any[];
    printstoreloading: boolean;
    printstoreerror: any;
    postPrintJob: (printJobData: any) => Promise<void>;
    resetPrintstore: () => Promise<void>;
}