import {
  ReactNode,
  createContext,
  useContext,
  useEffect,
  useState,
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

export const UserProvider = ({ children }: { children: ReactNode }) => {
  const [userInfo, setUserInfo] = useState<ILoginAccess | null>(null);

  const updateUserInfo = (info: ILoginAccess) => {
    setUserInfo(info);
  };

  const value = { userInfo, updateUserInfo };

  useEffect(() => {
    const user = localStorage.getItem("user");
    if (!user) setUserInfo(null);
    setUserInfo(JSON.parse(user as string) as ILoginAccess);
  }, []);

  return (
    <UserContext.Provider value={value}>
      {children}
    </UserContext.Provider>
  );
};


export const useUserContext = () => {
  return useContext(UserContext);
};