import { selectAnatomy } from '@chakra-ui/anatomy';
import {
  ComponentMultiStyleConfig,
  createMultiStyleConfigHelpers,
  defineStyle,
} from '@chakra-ui/react';
import { DEFAULT_STYLES } from '../globalStyles';

const { definePartsStyle, defineMultiStyleConfig } =
  createMultiStyleConfigHelpers(selectAnatomy.keys);

const tanda2 = definePartsStyle({
  field: {
    border: '1px solid',
    borderColor: 'rgba(25, 34, 76, 0.03);',
    background: 'rgba(25, 34, 76, 0.02)',
    borderRadius: '8px',
    _placeholder: {
      fontSize: DEFAULT_STYLES.textFontSize,
    },
  },
});

const tandaXl = defineStyle({
  fontSize: '0.9rem',
  px: '4',
});

const sizes = {
  xl: definePartsStyle({ field: tandaXl, addon: tandaXl }),
};

export const selectTheme: ComponentMultiStyleConfig = defineMultiStyleConfig({
  sizes,
  variants: { tanda2 },
  defaultProps: {
    size: 'xl',
    variant: 'tanda2',
  },
});
