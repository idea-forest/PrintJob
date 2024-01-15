import { PrintJob } from "./printjob"

export interface Dashboard {
    totalDevice: number,
    totalActiveDevice: number,
    totalPrinter: number,
    printJob: PrintJob[]
}