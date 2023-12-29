import Image from 'next/image';
import { ColumnFlex, Desc } from 'components';
import { NewPageItemsModel } from 'models';
import { DEFAULT_STYLES } from 'styles';
import { Text } from '@chakra-ui/react';
import { useSharedColor } from 'customHooks';

export const NewPageCardItem = ({ title, desc, icon }: NewPageItemsModel) => {
  const color = useSharedColor();

  return (
    <ColumnFlex
      minH='225px'
      border={DEFAULT_STYLES.border}
      rounded='10px'
      justify='center'
      px='4'
      py='3'
      gap='3'
    >
      <Image src={icon} alt={title ?? ''} />

      <Text textStyle='title' fontSize='1.1rem' color={color}>
        {title}
      </Text>

      <Desc color={DEFAULT_STYLES.darkGray}>{desc}</Desc>
    </ColumnFlex>
  );
};
