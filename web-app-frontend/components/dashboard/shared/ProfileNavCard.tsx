import { ColumnFlex, Desc } from 'components';
import { Header } from './Header';
import { DEFAULT_STYLES } from 'styles';
import { IProps } from 'models';
import { Box } from '@chakra-ui/react';

interface Props extends IProps {
  showOnMobile?: boolean;
}

export const ProfileNavCard = ({ title, desc, showOnMobile }: Props) => {
  return (
    <ColumnFlex>
      <Header>{title}</Header>
      <Box display={{ base: showOnMobile ? 'block' : 'none', md: 'block' }}>
        <Desc
          fontWeight={DEFAULT_STYLES.semibold}
          color={DEFAULT_STYLES.darkGray}
        >
          {desc}
        </Desc>
      </Box>
    </ColumnFlex>
  );
};
