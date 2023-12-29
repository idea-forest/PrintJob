import { Header } from 'components';
import { DEFAULT_STYLES } from 'styles';
import { Button, Text } from '@chakra-ui/react';
import { ChevronRightIcon } from '@chakra-ui/icons';
import { SharedBannerContainer } from './SharedBannerContainer';

export const NewBanner = () => {
  return (
    <SharedBannerContainer>
      <Header fontSize='1.3rem' mb='1' fontWeight={DEFAULT_STYLES.bold}>
        Supercharge your growth
      </Header>

      <Text
        maxW={{ base: DEFAULT_STYLES.fullWidth, lg: '450px' }}
        textAlign='center'
        color={DEFAULT_STYLES.primaryColor}
        opacity={0.4}
      >
        In order to complete your account setup and receive your funding offer,
        please connect your bank account, accounting platform, and payment
        platform.
      </Text>

      <Button mt='10' mb='3'>
        Connect your data
      </Button>

      <Button
        variant='text'
        rightIcon={<ChevronRightIcon fontSize='1.6rem' mt='0.5' />}
      >
        Talk to our team
      </Button>
    </SharedBannerContainer>
  );
};
