import { defineStyle, defineStyleConfig } from '@chakra-ui/react';

const xxl = defineStyle({
  height: 113,
  width: 113,
});

export const spinnerTheme = defineStyleConfig({
  sizes: { xxl },
  defaultProps: {
    size: 'xxl',
  },
});
