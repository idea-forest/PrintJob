import { BoxProps, Flex, FlexProps } from '@chakra-ui/react';

interface Props extends FlexProps, BoxProps {}

export const ColumnFlex = ({ children, ...rest }: Props) => {
  return (
    <Flex direction={rest.direction ?? 'column'} {...rest}>
      {children}
    </Flex>
  );
};
