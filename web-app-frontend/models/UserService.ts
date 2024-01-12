import { Team } from "./team";
import { ILoginAccess } from "./user";

export interface UserService {
    team: Team | null;
    isLoading: boolean;
    errors: any;
    success: boolean;
    fetchUserProfile: (accessToken: string) => any;
    LoginByGoogle: (teamid: string, username: string) => Promise<ILoginAccess | void>;
    login: (teamid: string, username: string, password: string) => Promise<ILoginAccess>;
    register: (teamname: string, email: string, password: string) => Promise<any>;
    checkTeamName: (teamName: string) => Promise<any>;
    logout: () => void;
}