import Image from 'next/image';
import { DEFAULT_STYLES } from 'styles';
import { Flex, Center, Text } from '@chakra-ui/react';
import { ActionTitleModel } from 'models';

export const ActionTitle = ({
  headerColor,
  icon,
  header,
  textProps,
}: ActionTitleModel) => {
  return (
    <Flex align='center' gap='7px'>
      <Center bgColor={headerColor} borderRadius='full' w='37px' h='37px'>
        <Image src={icon} alt={header ?? 'Tanda'} />
      </Center>

      <Text
        fontWeight={DEFAULT_STYLES.mediumbold}
        textTransform='uppercase'
        letterSpacing='0.13px'
        color={headerColor}
        {...textProps}
      >
        {header}
      </Text>
    </Flex>
  );
};
