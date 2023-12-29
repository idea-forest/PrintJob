import { DEFAULT_STYLES } from 'styles';
import { Center, CenterProps } from '@chakra-ui/react';

export const GrayInfoCard = ({ children, ...r }: CenterProps) => {
  return (
    <Center
      minW='133px'
      minH='50px'
      px='3'
      color='rgba(43, 43, 43, 0.6)'
      bg='rgba(99, 115, 129, 0.5)'
      rounded='10px'
      fontSize={DEFAULT_STYLES.textFontSize}
      fontWeight={DEFAULT_STYLES.mediumbold}
      opacity={DEFAULT_STYLES.opacity}
      {...r}
    >
      {children}
    </Center>
  );
};
