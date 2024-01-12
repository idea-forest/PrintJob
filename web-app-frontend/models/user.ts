export interface ILoginAccess{
    token:string;
    refreshToken:string;
    expires_in:number;
    success:boolean;
    user: [] | null;
    error: string;
}