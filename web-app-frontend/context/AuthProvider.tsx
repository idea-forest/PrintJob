import React, { createContext, useContext, useEffect, useState } from 'react';
import { useRouter } from 'next/router';

interface AuthContextProps {
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);


export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const router = useRouter();
  const checkAuthentication = () => {
    const user = localStorage.getItem('user');
    setIsAuthenticated(!!user);
  };

  useEffect(() => {
    checkAuthentication();
    if (!isAuthenticated && !['/auth/login', '/auth/register', '/'].includes(router.pathname)) {
      router.push('/auth/login');
    } else if (isAuthenticated && ['/auth/login', '/auth/register', '/'].includes(router.pathname)) {
      router.push('/dashboard');
    }
  }, [isAuthenticated, router, router.pathname]);

  useEffect(() => {
    const handleRouteChange = () => {
      checkAuthentication();
    };

    router.events.on('routeChangeComplete', handleRouteChange);

    return () => {
      router.events.off('routeChangeComplete', handleRouteChange);
    };
  }, [router]);

  return <AuthContext.Provider value={{ isAuthenticated }}>{children}</AuthContext.Provider>;
};


export const useAuth = () => {
  return useContext(AuthContext);
};
