import { DashBoardLayout } from 'layout';
import { LayoutProps, TopBarProps } from 'models';
import { DEFAULT_STYLES } from 'styles';
import { Grid } from '@chakra-ui/react';

interface Props extends LayoutProps, TopBarProps {}

export const SharedExpensesAndDraw = ({
  header,
  showBckBtn,
  children,
}: Props) => {
  return (
    <DashBoardLayout header={header} showBckBtn={showBckBtn}>
      <Grid
        gap={{ base: '33px', lg: '10px', xl: '33px' }}
        templateColumns={{
          base: '1fr',
          md: 'repeat(3,1fr)',
          lg: 'repeat(2,600px 330px)',
          xl: 'repeat(2,627px 340px)',
          '2xl': 'repeat(2,727px 424px)',
        }}
        alignItems='flex-start'
        px={DEFAULT_STYLES.mobilePx}
        w={DEFAULT_STYLES.fullWidth}
      >
        {children}
      </Grid>
    </DashBoardLayout>
  );
};
