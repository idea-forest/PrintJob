import { useRef, useEffect, useState } from 'react';
import { createPortal } from 'react-dom';
import { LayoutProps } from 'models';
import { Flex } from '@chakra-ui/react';
import { DEFAULT_STYLES } from 'styles';

export const SharedPortal = ({ children }: LayoutProps) => {
  const ref = useRef<Element | null>(null);
  const [test] = useState(false);

  useEffect(() => {
    ref.current = document.querySelector<HTMLElement>('#portal');
  }, []);

  return test && ref.current
    ? createPortal(
        <Flex
          width={DEFAULT_STYLES.fullWidth}
          height={DEFAULT_STYLES.fullWidth}
          position='fixed'
          left={0}
          top={0}
          overflow='auto'
          bgColor='rgba(245, 245, 245, 0.4)'
          zIndex={2}
          align='center'
          justify='center'
        >
          {children}
        </Flex>,
        ref.current
      )
    : null;
};
