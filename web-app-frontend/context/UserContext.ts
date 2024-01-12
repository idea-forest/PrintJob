import {
    createContext,
} from "react";
import { ILoginAccess } from "models";

interface IUserContext {
    userInfo: ILoginAccess | null;
    updateUserInfo: (v: ILoginAccess) => void;
  }

export const UserContext = createContext<IUserContext>({
    userInfo: null,
    updateUserInfo: (v: ILoginAccess) => { },
});