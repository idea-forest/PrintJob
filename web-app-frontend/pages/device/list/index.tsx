import { DEFAULT_STYLES } from "styles";
import React, { useEffect, useState } from "react";
import { DashBoardLayout } from "layout";
import { Header } from "components";
import { DeviceListTable } from "components/dashboard/shared/DeviceListTable";
import {
  Card,
  CardHeader,
  CardBody,
  Heading,
  Stack,
  StackDivider,
  Alert,
  AlertIcon,
  AlertTitle,
  AlertDescription,
} from "@chakra-ui/react";
import { useDeviceStore } from "utils/api";
import { RingSpinnerOverlay } from "react-spinner-overlay";
import { ILoginAccess } from "models";

interface DeviceListProps {}

const DeviceList: React.FC<DeviceListProps> = () => {
  const { data, loading, error, fetchData } = useDeviceStore();
  const [errorAlert, setErrorAlert] = useState<string | null>(null);

  useEffect(() => {
    const userInfo: ILoginAccess = JSON.parse(localStorage.getItem("user") as string);
    fetchData(userInfo?.user?.teamId);
  }, []);

  useEffect(() => {
    if (error) {
      setErrorAlert("An error occurred while fetching data.");
    } else {
      setErrorAlert(null);
    }
  }, [error]);

  const topbar = (
    <Header fontSize={{ base: "1.1rem", md: "1.5rem" }}>Device List</Header>
  );

  return (
    <DashBoardLayout header={topbar} showBckBtn>
      {loading ? (
        <RingSpinnerOverlay loading={loading} size={50} />
      ) : (
        <Card w="100%">
          <CardBody>
            {errorAlert ? (
              <Alert status="error" mb="4">
                <AlertIcon />
                <AlertTitle mr={2}>Error!</AlertTitle>
                <AlertDescription>{errorAlert}</AlertDescription>
              </Alert>
            ) : (
              <Stack divider={<StackDivider />} spacing="4">
                <DeviceListTable dataRowList={data} />
              </Stack>
            )}
          </CardBody>
        </Card>
      )}
    </DashBoardLayout>
  );
};

export default DeviceList;
