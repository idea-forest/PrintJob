import { checkboxAnatomy } from '@chakra-ui/anatomy';
import { createMultiStyleConfigHelpers } from '@chakra-ui/react';
import { DEFAULT_STYLES } from '../globalStyles';

const { definePartsStyle, defineMultiStyleConfig } =
  createMultiStyleConfigHelpers(checkboxAnatomy.keys);

const pink = definePartsStyle({
  control: {
    borderColor: DEFAULT_STYLES.pink,
    _checked: {
      bg: DEFAULT_STYLES.pink,
      color: DEFAULT_STYLES.white,
      borderColor: DEFAULT_STYLES.pink,
      _hover: {
        bg: DEFAULT_STYLES.pink,
        color: DEFAULT_STYLES.white,
        borderColor: DEFAULT_STYLES.pink,
      },
    },
  },
});

const dark = definePartsStyle({
  control: {
    borderColor: DEFAULT_STYLES.darkColor,
    _checked: {
      bg: DEFAULT_STYLES.darkColor,
      color: DEFAULT_STYLES.white,
      borderColor: DEFAULT_STYLES.darkColor,
      _hover: {
        bg: DEFAULT_STYLES.darkColor,
        color: DEFAULT_STYLES.white,
        borderColor: DEFAULT_STYLES.darkColor,
      },
    },
  },
  label: {
    fontSize: DEFAULT_STYLES.textFontSize,
  },
});

const baseStyle = definePartsStyle({
  control: {
    borderColor: DEFAULT_STYLES.lightPurple,
    _checked: {
      bg: DEFAULT_STYLES.lightPurple,
      color: DEFAULT_STYLES.white,
      _hover: {
        bg: DEFAULT_STYLES.lightPurple,
        color: DEFAULT_STYLES.white,
      },
    },
  },
  label: {
    fontSize: DEFAULT_STYLES.textFontSize,
  },
});

export const checkboxTheme = defineMultiStyleConfig({
  baseStyle,
  variants: {
    pink,
    dark,
  },
});
