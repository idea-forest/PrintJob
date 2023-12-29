import { inputAnatomy } from '@chakra-ui/anatomy';
import {
  ComponentMultiStyleConfig,
  createMultiStyleConfigHelpers,
  defineStyle,
} from '@chakra-ui/react';
import { DEFAULT_STYLES } from '../globalStyles';

const { definePartsStyle, defineMultiStyleConfig } =
  createMultiStyleConfigHelpers(inputAnatomy.keys);

const baseStyle = definePartsStyle({
  field: {
    borderRadius: '4px',
    _invalid: {
      border: '2px solid',
      borderColor: DEFAULT_STYLES.errorColor,
    },
    _focus: {
      border: '2px solid',
      borderColor: DEFAULT_STYLES.lightPurple,
    },
  },
});

// Variants
;

const tandaXs = definePartsStyle({
  field: {
    border: '2px solid',
    borderColor: '#DEE5E9',
    background: 'transparent',
    borderRadius: '4px',
    transition: DEFAULT_STYLES.transition,
    h: '37px',
    w: '123px',
  },
});

// How to define sizes
const tandaXl = defineStyle({
  px: '4',
  h: '56px',
});

const sizes = {
  xl: definePartsStyle({ field: tandaXl }),
};

export const inputTheme: ComponentMultiStyleConfig = defineMultiStyleConfig({
  variants: {  tandaXs },
  baseStyle,
  sizes,
  defaultProps: {
    size: 'xl',
  },
});
