import { LayoutProps } from 'models';
import { useRouter } from 'next/router';
import { createContext, useContext, useEffect, useState } from 'react';

interface MediaContextProps {
  mediaQuery: boolean;
  upDateMediaQuery: (b: boolean) => void;
}

const MediaContext = createContext<MediaContextProps>({
  mediaQuery: false,
  upDateMediaQuery: () => {},
});

export const MediaContextProvider = ({ children }: LayoutProps) => {
  const [mediaQuery, setMediaQuery] = useState(false);
  const { pathname } = useRouter();

  useEffect(() => {
    upDateMediaQuery(false);
  }, [pathname]);

  const upDateMediaQuery = (b: boolean) => {
    setMediaQuery(b);
  };

  return (
    <MediaContext.Provider value={{ mediaQuery, upDateMediaQuery }}>
      {children}
    </MediaContext.Provider>
  );
};

export const useCustomMediaQuery = () => {
  return useContext(MediaContext);
};
