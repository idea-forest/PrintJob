import React, { useState, useRef, useEffect } from "react";
import { DashBoardLayout } from "layout";
import { Header } from "components";
import { useFormik } from "formik";
import * as Yup from "yup";
import {
  Card,
  CardBody,
  CardFooter,
  Flex,
  IconButton,
  Grid,
  Button,
  Spinner,
  Stack,
  StackDivider,
  Alert,
  AlertIcon,
  AlertTitle,
  AlertDescription,
} from "@chakra-ui/react";
import DeviceRadioCard from "components/dashboard/DeviceRadioCard";
import {
  Box,
  FormControl,
  FormErrorMessage,
  FormLabel,
  Icon,
  Image,
  Input,
  Select,
  Text,
} from "@chakra-ui/react";
import { ArrowLeftIcon, ArrowRightIcon } from "@chakra-ui/icons";
import { FaFileLines } from "react-icons/fa6";
import Upload from "rc-upload";
import { IPrintJob } from "models";
import PrinterCard from "components/dashboard/shared/PrinterRadioCard";
import PrinterColor from "components/dashboard/shared/PrinterColor";
import PaperName from "components/dashboard/shared/PaperType";
import { RingSpinnerOverlay } from "react-spinner-overlay";
import Toast from "components/dashboard/shared/Toast";
import pdfjs from "pdfjs-dist";
import { ApiRoutes } from "utils/api-route";
import { useDeviceStore, usePrinterStore, usePostPrintJob } from "utils/api";
import { useRouter } from "next/router";

const CreatePrintJob = () => {
  const scrollContainerRef = useRef(null);
  const [selectedDeviceIndex, setSelectedDeviceIndex] = useState("");
  const [selectedPrinterName, setSelectedPrinterName] = useState("");
  const [selectedPrinterColor, setSelectedPrinterColor] = useState(false);
  const [mainPrinterColor, setMainPrinterColor] = useState(false);
  const { data, loading, error, fetchData } = useDeviceStore();
  const { printer, printerloading, printererror, fetchPrinterData } =
    usePrinterStore();
  const { printstore, printstoreloading, printstoreerror, postPrintJob, resetPrintstore } =
    usePostPrintJob();
  const [paperName, setPaperName] = useState("");
  const [fileUrl, setFileUrl] = useState("");
  const [fileBool, setFileBool] = useState(false);
  const router = useRouter();
  const handleSelect = (index) => {
    setSelectedDeviceIndex(index === selectedDeviceIndex ? null : index);
  };
  const handlePrinterSelect = (printerName) => {
    setSelectedPrinterName(printerName);
    formik.setFieldValue("selectedPrinterName", printerName);
  };
  const [uploading, setUploading] = useState(false);

  useEffect(() => {
    const userInfo: ILoginAccess = JSON.parse(
      localStorage.getItem("user") as string
    );
    fetchData(userInfo?.user?.teamId);
  }, []);

  useEffect(() => {
    fetchPrinterData(selectedDeviceIndex);
  }, [fetchPrinterData, selectedDeviceIndex]);

  useEffect(() => {
    if(printstoreerror) {
      Toast("Opps something went wrong", 'error');
    }
  }, [postPrintJob, printstoreerror]);

  useEffect(() => {
    if (printstore.status) {
      Toast(printstore?.message, 'success');
      resetPrintstore();
      router.push("/printjobs/list");
    } else if (printstore.status === false) {
      Toast(printstore.message ?? "Something went wrong", 'error');
      resetPrintstore();
    }
  }, [postPrintJob, printstore, resetPrintstore, router]);

  const paper = ["A4", "A3", "Letter", "Legal"];

  const scrollLeft = () => {
    if (scrollContainerRef.current) {
      scrollContainerRef.current.scrollLeft -= 200;
    }
  };

  const validationSchema = Yup.object().shape({
    selectedDeviceIndex: Yup.string().required(
      "Please select at least one device"
    ),
    selectedPrinterName: Yup.string().required("Please select a printer"),
    paperName: Yup.string().required("Please select a paper name"),
    file: Yup.string().required("Please upload a file"),
  });

  const formik = useFormik({
    initialValues: {
      selectedDeviceIndex: "",
      selectedPrinterName: "",
      mainPrinterColor: "",
      paperName: "",
      file: "",
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        await postPrintJob(values);
      } catch (error) {
        console.error("Error submitting print job", error);
      }
    },
  });

  const scrollRight = () => {
    if (scrollContainerRef.current) {
      scrollContainerRef.current.scrollLeft += 200;
    }
  };

  const handleUpload = (file) => {
    const reader = new FileReader();

    reader.onload = (event) => {
      const arrayBuffer = event.target.result;
      pdfjs.getDocument(arrayBuffer).promise.then((pdfDocument) => {
        const pageCount = pdfDocument.numPages;
        console.log(`Number of pages: ${pageCount}`);
      });
    };

    reader.readAsArrayBuffer(file);
  };

  const topbar = (
    <Header fontSize={{ base: "1.1rem", md: "1.5rem" }}>
      Create Print Job
    </Header>
  );

  return (
    <DashBoardLayout header={topbar} showBckBtn>
      <RingSpinnerOverlay loading={loading} size={30} />
      <RingSpinnerOverlay loading={printerloading} size={30} />
      <Flex
        direction={{ base: "column", md: "row" }}
        align="center"
        justify="center"
        w="100%"
      >
        <Card w="100%">
          <CardBody>
            <Box p={4} position="relative">
              <Text textAlign="center" fontWeight="bold" fontSize="xl">
                Available Devices
              </Text>
              <Flex
                justifyContent="space-between"
                alignItems="center"
                mx="auto"
              >
                <IconButton
                  icon={<ArrowLeftIcon boxSize={10} />} // Make the icon bigger
                  onClick={scrollLeft}
                  size="lg"
                />
                <Box overflowX="auto">
                  <Flex
                    ref={scrollContainerRef}
                    justifyContent="center"
                    alignItems="center"
                    overflowX="auto"
                    flexWrap="nowrap"
                    pb={4}
                  >
                    {data.map((device, index) => (
                      <DeviceRadioCard
                        key={index}
                        device={device}
                        isSelected={device.deviceId === selectedDeviceIndex}
                        onSelect={() => {
                          handleSelect(device.deviceId);
                          formik.setFieldValue(
                            "selectedDeviceIndex",
                            device.deviceId
                          );
                        }}
                      />
                    ))}
                  </Flex>
                </Box>
                <IconButton
                  icon={<ArrowRightIcon boxSize={10} />} // Make the icon bigger
                  onClick={scrollRight}
                  size="lg"
                />
              </Flex>
              {formik.touched.selectedDeviceIndex &&
                formik.errors.selectedDeviceIndex && (
                  <Text textAlign="center" fontWeight="bold" color="red.500">
                    {formik.errors.selectedDeviceIndex}
                  </Text>
                )}
            </Box>

            {Array.isArray(printer) && printer.length > 0 && (
              <Box mx="auto">
                <Box overflow="auto" mt={8}>
                  <Text textAlign="center" fontWeight="bold" fontSize="xl">
                    Printers
                  </Text>
                  <Grid
                    templateColumns={{ base: "1fr", md: "repeat(4, 1fr)" }}
                    gap={6}
                    justifyContent="center"
                  >
                    {printer.map((printers, index) => (
                      <PrinterCard
                        key={index}
                        printer={printers}
                        isSelected={printers.name === selectedPrinterName}
                        onSelect={() => {
                          handlePrinterSelect(printers.name);
                          setSelectedPrinterColor(
                            printers.printerColor == "Color" ? true : false
                          );
                        }}
                      />
                    ))}
                  </Grid>
                  {formik.touched.selectedPrinterName &&
                    formik.errors.selectedPrinterName && (
                      <Text
                        textAlign="center"
                        fontWeight="bold"
                        color="red.500"
                      >
                        {formik.errors.selectedPrinterName}
                      </Text>
                    )}
                </Box>
                <Box overflow="auto" mt={8}>
                  <Text textAlign="center" fontWeight="bold" fontSize="xl">
                    Printer Colors
                  </Text>
                  <Grid
                    templateColumns={{ base: "1fr", md: "repeat(2, 1fr)" }}
                    gap={6}
                    justifyContent="center"
                    alignItems="center"
                  >
                    <PrinterColor
                      name="Black"
                      color={mainPrinterColor}
                      isSelected={!mainPrinterColor}
                      onSelect={() => {
                        setMainPrinterColor(false);
                        formik.setFieldValue("mainPrinterColor", false);
                      }}
                    />
                    {selectedPrinterColor && (
                      <PrinterColor
                        name="Colored"
                        color={mainPrinterColor}
                        isSelected={mainPrinterColor}
                        onSelect={() => {
                          setMainPrinterColor(true);
                          formik.setFieldValue("mainPrinterColor", true);
                        }}
                      />
                    )}
                  </Grid>
                  {formik.touched.mainPrinterColor &&
                    formik.errors.mainPrinterColor && (
                      <Text
                        textAlign="center"
                        fontWeight="bold"
                        color="red.500"
                      >
                        {formik.errors.mainPrinterColor}
                      </Text>
                    )}
                </Box>
                <Box overflow="auto" mt={8}>
                  <Text textAlign="center" fontWeight="bold" fontSize="xl">
                    Paper Names
                  </Text>
                  <Grid
                    templateColumns={{ base: "1fr", md: "repeat(4, 1fr)" }}
                    gap={6}
                    justifyContent="center"
                  >
                    {paper.map((pap, index) => (
                      <PaperName
                        key={index}
                        name={pap}
                        isSelected={paperName === pap}
                        onSelect={() => {
                          setPaperName(pap);
                          formik.setFieldValue("paperName", pap);
                        }}
                      />
                    ))}
                  </Grid>
                  {formik.touched.paperName && formik.errors.paperName && (
                    <Text textAlign="center" fontWeight="bold" color="red.500">
                      {formik.errors.paperName}
                    </Text>
                  )}
                </Box>
              </Box>
            )}
            <FormControl mb="3">
              <Text mb="3" textAlign="center" fontWeight="bold" fontSize="xl">
                Upload Your Document
              </Text>
              <Upload
                disabled={fileBool}
                name="_IFormFile"
                onProgress={({ percent }) => {
                  setUploading(true);
                  setFileBool(true);
                  if (percent === 100) {
                    setFileBool(false);
                    setUploading(false);
                  }
                }}
                beforeUpload={(file, fileList) => {
                  const isPDF = file.type === "application/pdf";
                  if (!isPDF) {
                    message.error("You can only upload PDF files!");
                  }
                  return isPDF;
                }}
                onSuccess={(response, file) => {
                  const { name, size, type, lastModified } = file;
                  const images = [
                    {
                      name,
                      size,
                      type,
                      lastModified,
                      url: response.url,
                    },
                  ];
                  setFileBool(true);
                  setFileUrl(response.url);
                  formik.setFieldValue("file", response.url);
                }}
                action={ApiRoutes.uploadFile}
              >
                <Box
                  p="4"
                  bg="gray.100"
                  display="flex"
                  justifyContent="center"
                  alignItems="center"
                  flexDirection="column"
                  h="200px"
                >
                  {uploading ? (
                    <span>The file is uploading...</span>
                  ) : (
                    <>
                      <Icon
                        as={FaFileLines}
                        w={8}
                        h={8}
                        mb="3"
                        color="gray.600"
                      />
                      {fileBool ? (
                        <Text color="gray.700" fontWeight="semibold">
                          {fileUrl}
                        </Text>
                      ) : (
                        <Text color="gray.700" fontWeight="semibold">
                          Click or drag file to this area to upload
                        </Text>
                      )}
                    </>
                  )}
                </Box>
              </Upload>
              {formik.touched.file && formik.errors.file && (
                <Text textAlign="center" fontWeight="bold" color="red.500">
                  {formik.errors.file}
                </Text>
              )}
            </FormControl>
          </CardBody>
          <CardFooter>
            <Button
              colorScheme="teal"
              type="submit"
              onClick={formik.handleSubmit}
              disabled={
                formik.isSubmitting || (formik.isValidating && fileBool)
              }
              mb={{ base: 4, md: 0 }}
            >
              {printstoreloading ? (
                <Spinner size="sm" ml={2} />
              ) : (
                "Send Print Job"
              )}
            </Button>
          </CardFooter>
        </Card>
      </Flex>
    </DashBoardLayout>
  );
};

export default CreatePrintJob;
