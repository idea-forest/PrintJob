import { Text, TextProps } from '@chakra-ui/react';

export const Header = ({ children, ...r }: TextProps) => {
  return (
    <Text textStyle='header' {...r}>
      {children}
    </Text>
  );
};
