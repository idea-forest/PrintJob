import React, { ReactNode } from "react";
import { Flex } from "@chakra-ui/react";

interface AuthComponentProps {
  children: ReactNode;
  height?: string | number;
}

const AuthComponent: React.FC<AuthComponentProps> = ({ children, height }) => {
  return (
    <Flex
      flexDirection="column"
      width="100wh"
      height={height !== undefined ? height : "auto"}
      bgGradient="linear(to-t, #818cf8, rgba(129, 140, 248, 0))"
      justifyContent="center"
      alignItems="center"
      pb="50px"
      px={{ base: "4", sm: "0" }}
    >
      {children}
    </Flex>
  );
};

export default AuthComponent;
