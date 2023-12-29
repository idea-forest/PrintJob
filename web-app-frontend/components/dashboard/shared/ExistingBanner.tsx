import Image from 'next/image';
import loader from 'assets/images/loader.png';
import { DEFAULT_STYLES } from 'styles';
import { Flex, Text } from '@chakra-ui/react';
import { Header } from './Header';
import { SharedBannerContainer } from './SharedBannerContainer';

export const ExistingBanner = () => {
  return (
    <SharedBannerContainer>
      <Flex align='center' gap='2' direction={{ base: 'column', md: 'row' }}>
        <Image src={loader} alt='Loader' />

        <Header fontSize='1.3rem' mb='1' fontWeight={DEFAULT_STYLES.bold}>
          We’re evaluating your company to get you the best financing
        </Header>
      </Flex>

      <Text
        maxW={{ base: DEFAULT_STYLES.fullWidth, lg: '555px' }}
        textAlign='center'
        color={DEFAULT_STYLES.primaryColor}
        opacity={0.4}
      >
        We’re currently performing our data-driven analysis on your business
        health. You’ll have your funding overview in less than 48 hours, in the
        meantime, explore the rest of Tanda.
      </Text>
    </SharedBannerContainer>
  );
};
