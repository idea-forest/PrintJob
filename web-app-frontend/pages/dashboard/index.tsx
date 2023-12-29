import { ActionCard, Header } from 'components';
import { DashBoardLayout } from 'layout';
import { bankIcon, financeIcon } from 'store';
import { DEFAULT_STYLES } from 'styles';
import { DashRoutes } from 'utils';
import { Grid } from '@chakra-ui/react';

export default function Home() {
  const topbar = <Header>Hello!</Header>;

  return (
    <DashBoardLayout header={topbar}>
      <h1>Settings</h1>
    </DashBoardLayout>
  );
}
