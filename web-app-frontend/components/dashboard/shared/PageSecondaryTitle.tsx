import { ColumnFlex, Desc } from 'components';
import { IProps } from 'models';
import { DEFAULT_STYLES } from 'styles';
import { Text, TextProps } from '@chakra-ui/react';

interface Props extends IProps, TextProps {}

export const PageSecondaryTitle = ({ title, desc, ...r }: Props) => {
  return (
    <ColumnFlex gap='0.5' px={DEFAULT_STYLES.mobilePx}>
      <Text textStyle='title'>{title}</Text>

      <Desc {...r}>{desc}</Desc>
    </ColumnFlex>
  );
};
