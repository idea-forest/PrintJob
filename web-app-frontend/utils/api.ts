import create from 'zustand';
import { ApiRoutes } from './api-route';
import { DeviceStore } from 'models/DeviceStore';
import { PrintJobStore } from 'models/PrintJobStore';
import { DashbaordStore } from 'models/DashboardStore';
import { PrinterStore } from 'models/PrinterStore';
import { SubmitPrintStore } from 'models';
import { ILoginAccess } from 'models';

export const useDeviceStore = create<DeviceStore>((set) => ({
    data: [],
    loading: false,
    error: null,
    fetchData: async (id?: number) => {
        try {
            set({ loading: true, error: null });
            const token: ILoginAccess = JSON.parse(localStorage.getItem("user") as string);
            const headers = token ? { Authorization: `Bearer ${token?.token}` } : {};
            const response = await fetch(`${ApiRoutes.apiUrl}/api/Device/DeviceList/${id}`, { headers });
            const data = await response.json();
            set({ data, loading: false });
        } catch (error: any) {
            set({ loading: false, error });
        }
    },
}));

export const usePrintJobStore = create<PrintJobStore>((set) => ({
    data: [],
    loading: false,
    error: null,
    fetchData: async (id?: number) => {
        try {
            set({ loading: true, error: null });
            const token: ILoginAccess = JSON.parse(localStorage.getItem("user") as string);
            const headers = token ? { Authorization: `Bearer ${token?.token}` } : {};
            const response = await fetch(`${ApiRoutes.apiUrl}/api/PrintJobControllerWeb/PrintJobList/${id}`, { headers });
            const data = await response.json();
            set({ data, loading: false });
        } catch (error: any) {
            set({ loading: false, error });
        }
    },
}));

export const useDashbaordStore = create<DashbaordStore>((set) => ({
    data: [],
    loading: false,
    error: null,
    fetchData: async (id?: number) => {
        try {
            set({ loading: true, error: null });
            const token: ILoginAccess = JSON.parse(localStorage.getItem("user") as string);
            const headers = token ? { Authorization: `Bearer ${token?.token}` } : {};
            const response = await fetch(`${ApiRoutes.apiUrl}/api/Dashboard/GetDashboard`, { headers });
            const data = await response.json();
            set({ data, loading: false });
        } catch (error: any) {
            set({ loading: false, error });
        }
    },
}));

export const usePrinterStore = create<PrinterStore>((set) => ({
    printer: [],
    printerloading: false,
    printererror: null,
    fetchPrinterData: async (deviceId?: string) => {
        try {
            set({ printerloading: true, printererror: null });
            const response = await fetch(`${ApiRoutes.apiUrl}/api/Printer/GetPrinterByDevice/${deviceId}`);
            const data = await response.json();
            set({ printer: data, printerloading: false });
        } catch (error: any) {
            set({ printerloading: false, printererror: error });
        }
    },
}));

export const usePostPrintJob = create<SubmitPrintStore>((set) => ({
    printstore: [],
    printstoreloading: false,
    printstoreerror: null,
    postPrintJob: async (printJobData) => {
        try {
            const user: ILoginAccess = JSON.parse(localStorage.getItem("user") as string);
            set({ printstoreloading: true, printstoreerror: null });
            let printJobBody = {
                Copies: "1",
                UserId: `${user.user?.id}`,
                EndPage: "0",
                PaperName: printJobData.paperName,
                DeviceId: printJobData.selectedDeviceIndex,
                StartPage: "1",
                PrinterName: printJobData.selectedPrinterName,
                FilePath: `https://api.printbloc.com/downloadfile/${printJobData.file}`,
            };
            const response = await fetch(`${ApiRoutes.apiUrl}/api/PrintJob/CreatePrintJob`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(printJobBody),
            });

            if (!response.ok) {
                throw new Error(`Failed to post print job. Status: ${response.status}`);
            }

            const responseData = await response.json(); // Await the response.json() method
            set({ printstoreloading: false, printstore: responseData });
        } catch (error: any) {
            set({ printstoreloading: false, printstoreerror: error });
        }
    },
    resetPrintstore: async () => {
        set({ printstore: [], printstoreloading: false, printstoreerror: null })
    }
}));