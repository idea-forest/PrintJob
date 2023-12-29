import { Header } from 'components';
import { DashBoardLayout } from 'layout';

const DashboardHelp = () => {
  const topbar = <Header>Help</Header>;

  return (
    <DashBoardLayout header={topbar}>
      <h1>You will be helped</h1>
    </DashBoardLayout>
  );
};

export default DashboardHelp;
