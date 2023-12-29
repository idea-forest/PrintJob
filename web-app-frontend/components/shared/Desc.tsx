import { Text, TextProps } from '@chakra-ui/react';

export const Desc = ({ children, ...r }: TextProps) => {
  return (
    <Text color='rgba(99, 115, 129, 0.6)' textStyle='desc' {...r}>
      {children}
    </Text>
  );
};
