import React, { useEffect, useState } from "react";
import { Header } from "components";
import { DashBoardLayout } from "layout";
import { DEFAULT_STYLES } from "styles";
import {
  Grid,
  Box,
  Text,
  Icon,
  Flex,
  Card,
  CardBody,
  Stack,
  StackDivider,
  Alert,
  AlertIcon,
  AlertTitle,
  AlertDescription,
} from "@chakra-ui/react";
import { TbDeviceIpadShare } from "react-icons/tb";
import { TbPrinter } from "react-icons/tb";
import { TbDeviceMobileShare } from "react-icons/tb";
import { useDashbaordStore } from "utils/api";
import { PrintJobListTable } from "components";
import { RingSpinnerOverlay } from "react-spinner-overlay";

export default function Home() {
  const { data, loading, error, fetchData } = useDashbaordStore();
  const [errorAlert, setErrorAlert] = useState<string | null>(null);
  const topbar = <Header>Hello!</Header>;

  useEffect(() => {
    const userInfo: ILoginAccess = JSON.parse(
      localStorage.getItem("user") as string
    );
    fetchData(userInfo?.user?.teamId);
  }, []);

  useEffect(() => {
    if (error) {
      setErrorAlert("An error occurred while fetching data.");
    } else {
      setErrorAlert(null);
    }
  }, [error]);

  return (
    <DashBoardLayout header={topbar}>
      {loading ? (
        <RingSpinnerOverlay loading={loading} size={50} />
      ) : (
        <Grid gap={12}>
          <Grid
            gap={{ base: "33px", lg: "10px", xl: "33px" }}
            templateColumns={{
              base: "1fr",
              md: "repeat(1, 1fr)", // Single column on small screens
              lg: "repeat(3, 1fr)", // Three columns on large screens
              xl: "repeat(3, 1fr)", // Three columns on extra-large screens
              "2xl": "repeat(3, 1fr)", // Three columns on 2xl screens
            }}
            alignItems="flex-start"
            px={DEFAULT_STYLES.mobilePx}
            w={DEFAULT_STYLES.fullWidth}
          >
            <Box
              p={4}
              borderRadius="lg"
              boxShadow="md"
              textAlign="center"
              bgGradient="linear(to-r, purple.500, purple.300)"
              color="white"
            >
              <Flex align="center" justify="center" mb={2}>
                <Icon as={TbDeviceIpadShare} boxSize={6} mr={2} />
                <Text fontSize="xl">{data.totalDevice}</Text>
              </Flex>
              <Text>Total Device</Text>
            </Box>
            <Box
              p={4}
              borderRadius="lg"
              boxShadow="md"
              textAlign="center"
              bgGradient="linear(to-r, pink.500, pink.300)"
              color="white"
            >
              <Flex align="center" justify="center" mb={2}>
                <Icon as={TbDeviceMobileShare} boxSize={6} mr={2} />
                <Text fontSize="xl">{data.totalActiveDevice}</Text>
              </Flex>
              <Text>Active Device</Text>
            </Box>
            <Box
              p={4}
              borderRadius="lg"
              boxShadow="md"
              textAlign="center"
              bgGradient="linear(to-r, teal.500, blue.500)"
              color="white"
            >
              <Flex align="center" justify="center" mb={2}>
                <Icon as={TbPrinter} boxSize={6} mr={2} />
                <Text fontSize="xl">{data.totalPrinter}</Text>
              </Flex>
              <Text>Total Printer</Text>
            </Box>
          </Grid>
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
                  {data?.printJob && data.printJob.length > 0 ? (
                    <PrintJobListTable dataRowList={data.printJob} />
                  ) : (
                    <Alert status="error" mb="4">
                      <AlertIcon />
                      <AlertTitle mr={2}>Error!</AlertTitle>
                      <AlertDescription>No record found</AlertDescription>
                    </Alert>
                  )}
                </Stack>
              )}
            </CardBody>
          </Card>
        </Grid>
      )}
    </DashBoardLayout>
  );
}
