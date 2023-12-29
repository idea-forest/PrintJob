import Image from 'next/image';
import Link from 'next/link';
import { ColumnFlex } from 'components';
import { DEFAULT_STYLES } from 'styles';
import { Box, Button, HStack, Stack, Text } from '@chakra-ui/react';
import { TandaHDivider } from './TandaDivider';
import { ActionCardModel } from 'models';
import { ActionTitle } from './ActionTitle';

export const ActionCard = ({
  icon,
  header,
  headerColor,
  btnText,
  isShowTopBtn,
  onClick,
  title,
  desc,
  path,
  atEdge,
  gradientImg,
  leftIcon,
  btnBg,
  showTexts,
  details,
  info,
}: ActionCardModel) => {
  const buttonLink = (
    <Link
      href={path}
      style={{
        alignSelf: isShowTopBtn ? 'center' : 'flex-start',
      }}
    >
      <Button
        variant='noBg'
        bg={btnBg}
        border={btnBg ? 'none' : `1px solid ${DEFAULT_STYLES.darkGray}`}
        color={btnBg ? DEFAULT_STYLES.borderColor : DEFAULT_STYLES.darkGray}
        onClick={onClick}
        leftIcon={leftIcon}
        size={{ base: 'smPadding', lg: 'lgPadding' }}
      >
        {btnText}
      </Button>
    </Link>
  );

  return (
    <ColumnFlex
      minH='350px'
      layerStyle='card'
      align='center'
      pt='4'
      pb='5'
      position='relative'
      overflow='hidden'
    >
      <ColumnFlex width={{ base: '90%', lg: '85%' }} pb='4' gap='0.5'>
        <HStack align='center' justify='space-between'>
          <ActionTitle
            header={header}
            headerColor={headerColor}
            icon={icon}
            textProps={{
              fontSize: '1.3rem',
            }}
          />

          {isShowTopBtn && buttonLink}
        </HStack>

        <Text
          fontSize={DEFAULT_STYLES.fontSize}
          fontWeight={DEFAULT_STYLES.semibold}
          color={DEFAULT_STYLES.primaryColor}
          opacity={0.4}
        >
          {info}
        </Text>
      </ColumnFlex>

      <TandaHDivider />

      {showTexts && (
        <Stack width={{ base: '90%', lg: '85%' }} pt='4' flex='1' gap='2'>
          <Text textStyle='title' maxW='60%'>
            {title}
          </Text>

          <Text
            textStyle='bodyText'
            maxW='324px'
            color={DEFAULT_STYLES.primaryColor}
            opacity={0.4}
            flex='1'
          >
            {desc}
          </Text>
        </Stack>
      )}

      <Stack width={{ base: '90%', lg: '85%' }}>
        {!isShowTopBtn && buttonLink}
      </Stack>

      {details}

      {gradientImg && (
        <Box
          pos='absolute'
          right={atEdge ? 0 : '5'}
          bottom={atEdge ? 0 : '-5px'}
        >
          <Image src={gradientImg} alt='design' />
        </Box>
      )}
    </ColumnFlex>
  );
};
