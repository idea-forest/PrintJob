import Link from 'next/link';
import { ColumnFlex, Header } from 'components';
import { IProps } from 'models';
import { newPageItems } from 'store';
import { DEFAULT_STYLES } from 'styles';
import { Button, Grid, GridItem, Text } from '@chakra-ui/react';
import { NewPageCardItem } from './NewPageCardItem';

interface Props extends IProps {
  path?: string;
  onClick?: () => void;
}

export const NewPageCard = ({ desc, title, path, onClick }: Props) => {
  const renderNewPageItems = newPageItems.map((item) => (
    <NewPageCardItem key={item.id} {...item} />
  ));

  return (
    <GridItem colSpan={{ base: 1, lg: 2 }}>
      <ColumnFlex
        bg={DEFAULT_STYLES.white}
        border={DEFAULT_STYLES.border}
        rounded={DEFAULT_STYLES.borderRadius}
        minH='442px'
        pt='30px'
        pb='40px'
        px={{ base: '20px', xl: '30px' }}
        w={DEFAULT_STYLES.fullWidth}
      >
        <Header fontSize='1.5rem'>{`Welcome to Tanda ${title}`}</Header>

        <Text
          color={DEFAULT_STYLES.primaryColor}
          opacity={0.4}
          maxW={{ base: DEFAULT_STYLES.fullWidth, md: '600px' }}
        >
          {desc}
        </Text>

        {path && (
          <Link
            href={path}
            style={{ marginTop: '20px', alignSelf: 'flex-start' }}
          >
            <Button variant='dark' onClick={onClick}>
              Get started
            </Button>
          </Link>
        )}

        <Grid
          templateColumns={{
            base: 'repeat(1,1fr)',
            md: 'repeat(3,1fr)',
            '2xl': 'repeat(3,196px)',
          }}
          width={DEFAULT_STYLES.fullWidth}
          columnGap='35px'
          rowGap='5'
          mt={!path ? '45px' : '5'}
        >
          {renderNewPageItems}
        </Grid>
      </ColumnFlex>
    </GridItem>
  );
};
