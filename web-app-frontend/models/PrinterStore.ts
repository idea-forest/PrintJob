import { Printer } from "typescript";

export interface PrinterStore {
    printer: Printer[];
    printerloading: boolean;
    printererror: Error | null;
    fetchPrinterData: () => Promise<void>;
}