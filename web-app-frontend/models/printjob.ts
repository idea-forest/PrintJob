export interface PrintJob {
    id: number;
    filePath: string;
    color: boolean;
    startPage: number;
    page: number;
    endPage: number;
    copies: number;
    teamId: number;
    deviceId: string;
    printerName: string;
    type?: string | null;
    paperName: string;
    userId: string;
    status: string;
    message?: string | null;
    createdAt: Date;
    landScape: boolean;
}
