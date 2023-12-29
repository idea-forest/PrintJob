import { radioAnatomy } from '@chakra-ui/anatomy';
import { createMultiStyleConfigHelpers } from '@chakra-ui/react';
import { DEFAULT_STYLES } from '../globalStyles';

const { definePartsStyle, defineMultiStyleConfig } =
  createMultiStyleConfigHelpers(radioAnatomy.keys);

  const pink = definePartsStyle({
    control: {
      borderRadius: 'full',
      borderColor: DEFAULT_STYLES.pink,
      _checked: {
        bg: DEFAULT_STYLES.white,
        color: DEFAULT_STYLES.pink,
        borderColor: DEFAULT_STYLES.pink,
        _hover: {
          bg: DEFAULT_STYLES.white,
          color: DEFAULT_STYLES.pink,
          borderColor: DEFAULT_STYLES.pink,
        },
      },
    },
  });

  const baseStyle = definePartsStyle({
    control: {
      borderRadius: 'full',
      borderColor: DEFAULT_STYLES.lightPurple,
      _checked: {
        bg: DEFAULT_STYLES.white,
        color: DEFAULT_STYLES.lightPurple,
        borderColor: DEFAULT_STYLES.lightPurple,
        _hover: {
          bg: DEFAULT_STYLES.white,
          color: DEFAULT_STYLES.lightPurple,
          borderColor: DEFAULT_STYLES.lightPurple,
        },
      },
    },
  });

  export const radioTheme = defineMultiStyleConfig({
    baseStyle,
    variants: {
      pink,
    },
  });
