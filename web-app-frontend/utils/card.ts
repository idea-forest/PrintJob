import { CardStatus, CardType } from 'models';
import { DEFAULT_STYLES } from 'styles';

export const cardColorHandler = (type: CardType) => {
  if (type === CardType.Physical) {
    return DEFAULT_STYLES.redColor;
  } else {
    return DEFAULT_STYLES.lightGreen;
  }
};

export const cardTableColorHandler = (type: CardType) => {
  if (type === CardType.Physical) {
    return {
      color: DEFAULT_STYLES.lightGreen,
      bg: DEFAULT_STYLES.lightGreenBg,
    };
  } else {
    return {
      color: DEFAULT_STYLES.orange,
      bg: DEFAULT_STYLES.lightOrangeBg,
    };
  }
};

export const cardStatusColor = (status: CardStatus) => {
  if (status === CardStatus.Active) {
    return {
      color: DEFAULT_STYLES.redColor,
      bg: DEFAULT_STYLES.lightErrorBg,
    };
  } else {
    return {
      color: DEFAULT_STYLES.lightPurple,
      bg: DEFAULT_STYLES.lightPurpleBg,
    };
  }
};

export const hidePin = (pin: string | number, len: number = 4) => {
  let newPin = `${pin}`;
  const length = newPin.length;

  const ans = newPin
    .split('')
    .fill('*', 0, length - len)
    .reduce((prev, cur, index) => {
      if (index !== 0 && !(index % 4)) {
        prev += ' ';
      }
      return prev + cur;
    }, '');

  return ans;
};
