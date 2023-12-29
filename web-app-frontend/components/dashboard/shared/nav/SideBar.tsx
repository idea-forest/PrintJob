import { AiTwotoneHome } from 'react-icons/ai';
import { ColumnFlex, IconBtn } from 'components';
import { DEFAULT_STYLES } from 'styles';
import { Logo } from './Logo';
import { NavItem } from './NavItem';
import { NavCard } from './NavCard';
import { deviceItems } from 'store';
import { FiSettings } from 'react-icons/fi';
import { BiLogOut } from 'react-icons/bi';
import { DashRoutes } from 'utils';
import { useRouter } from 'next/router';
import { CloseIcon } from '@chakra-ui/icons';
import { useCustomMediaQuery } from 'context';
import { Flex } from '@chakra-ui/react';

// TODO: Remove nextjs data collection

export const SideBar = () => {
  const { push } = useRouter();
  const { mediaQuery, upDateMediaQuery } = useCustomMediaQuery();

  return (
    <Flex
      w={{ base: 'full', xl: 'max-content' }}
      bg='linear-gradient(180deg, rgba(23, 31, 52, 0.25) 0%, rgba(23, 31, 52, 0.25) 100%)'
      backdropFilter='blur(5px)'
      position='fixed'
      h={DEFAULT_STYLES.fullWidth}
      zIndex={10}
      transition={DEFAULT_STYLES.transition}
      transform={{
        base: `translate( ${mediaQuery ? '0%' : '-100%'})`,
        xl: 'translate(0px)',
      }}
      onClick={() => {
        upDateMediaQuery(false);
      }}
    >
      <ColumnFlex
        w='200px'
        h={DEFAULT_STYLES.fullWidth}
        position='relative'
        bg={DEFAULT_STYLES.white}
        shadow='0px 1px 12px rgba(195, 203, 222, 0.58806)'
        px='0.5'
        pt='40px'
        transition={DEFAULT_STYLES.transition}
      >
        <IconBtn
          aria-label='Close button'
          icon={<CloseIcon />}
          bgColor='transparent'
          display={{ base: 'flex', xl: 'none' }}
          position='absolute'
          right='5'
          top='4'
          onClick={() => {
            upDateMediaQuery(false);
          }}
        />

        <Logo />

        <ColumnFlex overflowY='auto' gap='1' flex='1' pb='20px' mt='30px'>
          <ColumnFlex gap='1' flex='1'>
            <NavItem
              icon={AiTwotoneHome}
              title='Home'
              path={DashRoutes.dashboard}
            />

            <NavCard header='Apps' list={deviceItems} />
          </ColumnFlex>

          <NavItem
            icon={FiSettings}
            title='Settings'
            path={DashRoutes.settings}
          />

          <NavItem
            icon={BiLogOut}
            title='Logout'
            onClick={() => {
              push(DashRoutes.home);
            }}
          />
        </ColumnFlex>
      </ColumnFlex>
    </Flex>
  );
};
