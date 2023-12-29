import { ColumnFlex } from 'components';
import { IProps } from 'models';
import { DEFAULT_STYLES } from 'styles';
import { Flex, Text } from '@chakra-ui/react';
import { GrayInfoCard } from './GrayInfoCard';

interface Props extends IProps {
  color?: string;
  exChange?: string;
}

export const ExchangeCard = ({ desc, exChange, title, color }: Props) => {
  return (
    <ColumnFlex w='full' gap='4'>
      <GrayInfoCard alignSelf='center'>{exChange}</GrayInfoCard>
      <Flex layerStyle='flex' justify='space-between' w='full'>
        <Text
          textStyle='desc'
          color={color}
          fontSize={DEFAULT_STYLES.textFontSize}
        >{`${title} Amount:`}</Text>
        <Text
          textStyle='desc'
          color={color}
          fontSize={DEFAULT_STYLES.textFontSize}
        >
          {desc}
        </Text>
      </Flex>
    </ColumnFlex>
  );
};
