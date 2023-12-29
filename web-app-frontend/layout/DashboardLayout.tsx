import { ColumnFlex, SideBar, Topbar } from 'components';
import { LayoutProps, TopBarProps } from 'models';
import { DEFAULT_STYLES } from 'styles';
import { Flex } from '@chakra-ui/react';

interface Props extends LayoutProps, TopBarProps {}
// Look at the dashboardlayout

export const DashBoardLayout = ({
  children,
  header,
  showBckBtn,
  otherBtn,
}: Props) => {
  return (
    <Flex
      width={DEFAULT_STYLES.fullWidth}
      minH={DEFAULT_STYLES.fullHeight}
      bg={DEFAULT_STYLES.bgColor}
      position='relative'
    >
      <SideBar />

      {/* ?? When moving to app. I have to extract all code below to a separate component so I can still pass props. This should help fix the layout issues */}
      <ColumnFlex
        ml={{ base: '0', xl: '200px' }}
        width={DEFAULT_STYLES.fullWidth}
        align='center'
        position='relative'
      >
        <Topbar header={header} showBckBtn={showBckBtn} otherBtn={otherBtn} />

        <Flex
          layerStyle='container'
          maxW={DEFAULT_STYLES.fullWidth}
          mt={{ base: '120px', lg: '150px' }}
          mb={{ base: '35px', lg: '50px' }}
          justify='center'
        >
          {children}
        </Flex>
      </ColumnFlex>
    </Flex>
  );
};
