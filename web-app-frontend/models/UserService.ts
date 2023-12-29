import { User } from "./user";
import { Team } from "./team";

export interface UserService {
    user: User | null;
    team: Team | null;
    isLoading: boolean;
    errors: any;
    success: boolean;
    fetchUserProfile: (accessToken: string) => any;
    LoginByGoogle: (teamid: string, username: string) => Promise<User | void>;
    login: (teamid: string, username: string, password: string) => Promise<User | void>;
    register: (teamname: string, email: string, password: string) => Promise<any>;
    checkTeamName: (teamName: string) => Promise<any>;
    logout: () => void;
}