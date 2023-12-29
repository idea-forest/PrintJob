import Link from 'next/link';
import Image from 'next/image';
import logo1 from 'assets/images/logo1.png';
import logo2 from 'assets/images/logo2.png';
import { Flex } from '@chakra-ui/react';
import { DashRoutes } from 'utils';
import { DEFAULT_STYLES } from 'styles';

export const Logo = () => {
  return (
    <Link href={DashRoutes.dashboard}>
      <Flex align='center' gap='1' pl={DEFAULT_STYLES.sidePl}>
        <Image src={logo1} alt='Printbloc' priority />
        <Image src={logo2} alt='Logo' priority />
      </Flex>
    </Link>
  );
};
