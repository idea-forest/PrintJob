import { DEFAULT_STYLES } from 'styles';
import { AlertStatus } from '@chakra-ui/react';
import { IconType } from 'react-icons';
import { AiFillInfoCircle } from 'react-icons/ai';
import { MdWarning } from 'react-icons/md';

export const alertStatusHandler = (status: AlertStatus) => {
  let color: string = '';
  let bg: string = '';
  let icon: IconType | undefined = undefined;

  switch (status) {
    case 'warning':
      color = DEFAULT_STYLES.errorColor;
      bg = DEFAULT_STYLES.lightErrorBg;
      icon = MdWarning;

      break;

    case 'info':
      color = DEFAULT_STYLES.lightPurple;
      bg = DEFAULT_STYLES.lightPurpleBg;
      icon = AiFillInfoCircle;
      break;
  }

  return { color, bg, icon };
};
