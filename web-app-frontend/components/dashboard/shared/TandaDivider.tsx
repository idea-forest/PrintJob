import { DEFAULT_STYLES } from 'styles';
import { Box, BoxProps } from '@chakra-ui/react';

export const TandaHDivider = ({ ...r }: BoxProps) => {
  return (
    <Box
      w={DEFAULT_STYLES.fullWidth}
      border={DEFAULT_STYLES.border}
      opacity={0.5}
      {...r}
    />
  );
};

export const TandaVDivider = ({ ...r }: BoxProps) => {
  return (
    <Box border={DEFAULT_STYLES.border} opacity={0.5} height='100%' {...r} />
  );
};
