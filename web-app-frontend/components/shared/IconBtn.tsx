import { ReactElement } from 'react';
import { IconButton, IconButtonProps } from '@chakra-ui/react';

interface Props extends IconButtonProps {
  icon: ReactElement;
  onClick?: () => void;
}

export const IconBtn = ({ icon, onClick, ...rest }: Props) => {
  return (
    <IconButton
      {...rest}
      icon={icon}
      color='#41485E'
      boxShadow='none'
      onClick={onClick}
    />
  );
};
