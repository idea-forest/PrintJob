import create from 'zustand';
import { ApiRoutes } from './api-route';
import { DeviceStore } from 'models/DeviceStore';
import { PrintJobStore } from 'models/PrintJobStore';
import { DashbaordStore } from 'models/DashboardStore';
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