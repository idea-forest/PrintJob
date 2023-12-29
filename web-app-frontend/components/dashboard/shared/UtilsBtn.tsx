import { DEFAULT_STYLES } from 'styles';
import { Button } from '@chakra-ui/react';

interface Props {
  btnText: string;
  onClick?: () => void;
}

export const UtilsBtn = ({ btnText, onClick }: Props) => {
  return (
    <Button
      width={DEFAULT_STYLES.fullWidth}
      variant='dark'
      rounded={8}
      onClick={() => {
        onClick?.();
      }}
      size='smPadding'
    >
      {btnText}
    </Button>
  );
};
