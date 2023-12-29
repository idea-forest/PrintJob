import { ColumnFlex, IconBtn } from 'components';
import { useCustomMediaQuery } from 'context';
import { TopBarProps } from 'models';
import { DEFAULT_STYLES } from 'styles';
import { DashRoutes } from 'utils';
import { HamburgerIcon, ChevronLeftIcon } from '@chakra-ui/icons';
import { Center, Flex, HStack, Text } from '@chakra-ui/react';
import { useRouter } from 'next/router';

export const Topbar = ({ header, showBckBtn, otherBtn }: TopBarProps) => {
  const { upDateMediaQuery } = useCustomMediaQuery();
  const router = useRouter();

  const color =
    router.pathname === DashRoutes.expense
      ? DEFAULT_STYLES.pink
      : DEFAULT_STYLES.primaryHeaderColor;

  return (
    <Flex
      w={DEFAULT_STYLES.fullWidth}
      h={{ base: '80px', lg: '104px' }}
      position='fixed'
      bg={DEFAULT_STYLES.white}
      borderBottom='1px solid rgba(218, 220, 224, 0.83)'
      align='center'
      justify='center'
      zIndex={5}
      px={DEFAULT_STYLES.mobilePx}
    >
      <HStack layerStyle='container' justify='space-between'>
        <Flex align='center' gap='4'>
          {showBckBtn && (
            <Center
              border='1px solid #E8ECF4'
              rounded='4px'
              cursor='pointer'
              padding='4px 10px'
              onClick={() => {
                router.back();
              }}
              _hover={{
                transform: 'scale(0.99)',
              }}
            >
              <ChevronLeftIcon fontSize='1.3rem' color={color} />
              <Text color={color} fontWeight={DEFAULT_STYLES.semibold}>
                Back
              </Text>
            </Center>
          )}

          <ColumnFlex>{header}</ColumnFlex>
        </Flex>

        <Flex w='max-content' gap='4'>
          {!otherBtn && (
            <Center
              width='54px'
              height='54px'
              bgColor={DEFAULT_STYLES.lightPurple}
              borderRadius='full'
            >
              <Center
                width='16px'
                height='16px'
                bg={DEFAULT_STYLES.lightGray}
                borderRadius='full'
              />
            </Center>
          )}

          {otherBtn}
          <IconBtn
            aria-label='Close button'
            icon={<HamburgerIcon fontSize='1.8rem' />}
            display={{ base: 'flex', xl: 'none' }}
            bgColor='transparent'
            onClick={() => {
              upDateMediaQuery(true);
            }}
          />
        </Flex>
      </HStack>
    </Flex>
  );
};
