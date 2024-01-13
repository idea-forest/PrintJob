import { DEFAULT_STYLES } from "styles";
import React, { useMemo } from "react";
import { DashBoardLayout } from "layout";
import { Header } from "components";
import { DeviceListTable } from "components/dashboard/shared/DeviceListTable";
import { Grid, Card, CardHeader, CardBody, Heading, Stack, StackDivider } from "@chakra-ui/react";

interface DeviceListProps {}

const DeviceList: React.FC<DeviceListProps> = () => {
  const data: ConvertData[] = useMemo(
    () => [
      {
        fromUnit: "inches",
        toUnit: "millimetres (mm)",
        factor: 25.4,
      },
      {
        fromUnit: "feet",
        toUnit: "centimetres (cm)",
        factor: 30.48,
      },
      {
        fromUnit: "yards",
        toUnit: "metres (m)",
        factor: 0.91444,
      },
    ],
    []
  );

  const columns = useMemo(
    () => [
      {
        Header: "To convert",
        accessor: "fromUnit",
      },
      {
        Header: "Into",
        accessor: "toUnit",
      },
      {
        Header: "Multiply by",
        accessor: "factor",
        isNumeric: true,
      },
    ],
    []
  );

  const topbar = (
    <Header fontSize={{ base: "1.1rem", md: "1.5rem" }}>Device List</Header>
  );

  return (
    <DashBoardLayout header={topbar} showBckBtn>
      <Grid
        gap={{ base: "33px", lg: "10px", xl: "33px" }}
        templateColumns={{
          base: "1fr",
          md: "repeat(3,1fr)",
          lg: "repeat(2,600px 330px)",
          xl: "repeat(2,627px 340px)",
          "2xl": "repeat(2,727px 424px)",
        }}
        alignItems="flex-start"
        px={DEFAULT_STYLES.mobilePx}
        w={DEFAULT_STYLES.fullWidth}
      >
        <Card>
          <CardHeader>
            <Heading size="md">Device List</Heading>
          </CardHeader>

          <CardBody>
            <Stack divider={<StackDivider />} spacing="4">
              <DeviceListTable columns={columns} data={data}/>
            </Stack>
          </CardBody>
        </Card>
      </Grid>
    </DashBoardLayout>
  );
};

export default DeviceList;
