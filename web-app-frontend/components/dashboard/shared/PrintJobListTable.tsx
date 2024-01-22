import React, { Fragment, useCallback } from "react";
import { useTable } from "@refinedev/react-table";
import { ColumnDef, flexRender, Row } from "@tanstack/react-table";
import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  TableContainer,
  HStack,
  Select,
  Textarea,
  IconButton,
  Checkbox,
  Button,
  Icon,
  Box,
  Badge,
} from "@chakra-ui/react";
import {
  List,
  EditButton,
  CreateButton,
  DeleteButton,
  DateField,
  SaveButton,
} from "@refinedev/chakra-ui";
import { useForm } from "@refinedev/react-hook-form";
import { IconChevronDown, IconChevronRight } from "@tabler/icons";
import { ColumnFilter, ColumnSorter } from "components/table";
import { Pagination } from "components/pagination";
import { FilterElementProps } from "models/FilterElementProps";
import { FaFilePdf } from "react-icons/fa";
import { BsFiletypeDocx } from "react-icons/bs";
import { PrintJob } from "models/printjob";
import { BiSolidFilePng } from "react-icons/bi";
import { AiOutlineFileJpg } from "react-icons/ai";
import { TbFileTypeXls } from "react-icons/tb";
import { useRouter } from 'next/router'
import { DashRoutes } from "utils";

interface PrintJobListTableProps {
  dataRowList: any[];
}

export const PrintJobListTable: React.FC<PrintJobListTableProps> = ({
  dataRowList,
}) => {
  const {
    refineCore: { onFinish, id, setId },
    handleSubmit,
    register,
  } = useForm<PrintJob>({
    refineCoreProps: {
      redirect: false,
      action: "edit",
    },
  });
  
  const router = useRouter()

  const rswitch = (param: any, cases: any) => {
    if (cases[param]) {
      return cases[param];
    } else {
      return cases.default;
    }
  };

  const reprintSelectedItems = (ids: number[]) => {};

  const columns = React.useMemo<ColumnDef<PrintJob>[]>(
    () => [
      {
        id: "userId",
        header: "Customer Name",
        accessorKey: "userId",
        enableColumnFilter: true,
      },
      {
        id: "deviceId",
        header: "Device Id",
        accessorKey: "deviceId",
        meta: {
          filterOperator: "contains",
        },
      },
      {
        id: "color",
        header: "Color",
        accessorKey: "color",
        enableColumnFilter: false,
      },
      {
        id: "page",
        header: "No of Pages",
        accessorKey: "page",
        enableColumnFilter: true,
      },
      {
        id: "copies",
        header: "Copies",
        accessorKey: "copies",
        enableColumnFilter: true,
      },
      {
        id: "printerName",
        header: "Printer Name",
        accessorKey: "printerName",
        enableColumnFilter: true,
      },
      {
        id: "type",
        header: "Type",
        accessorKey: "type",
        enableColumnFilter: true,
      },
      {
        id: "paperName",
        header: "Paper Name",
        accessorKey: "paperName",
        enableColumnFilter: true,
      },
      {
        id: "status",
        header: "Status",
        accessorKey: "status",
        enableColumnFilter: true,
      },
      {
        id: "createdAt",
        header: "Created At",
        accessorKey: "createdAt",
        cell: function render({ getValue }) {
          return <DateField value={getValue() as string} format="LLL" />;
        },
        enableColumnFilter: false,
      },
      {
        id: "actions",
        header: "Actions",
        accessorKey: "id",
        enableColumnFilter: false,
        enableSorting: false,
        cell: function render({ getValue }) {
          return (
            <HStack>
              <EditButton
                hideText
                size="sm"
                onClick={() => {
                  setId(getValue() as number);
                }}
              />
              <Button
                id="delete-selected"
                size="xs"
                color="blue"
                variant="outline"
                onClick={() =>
                  reprintSelectedItems(
                    table
                      .getSelectedRowModel()
                      .flatRows.map(({ original }) => original.id)
                  )
                }
              >
                Reprint
              </Button>
            </HStack>
          );
        },
      },
    ],
    []
  );

  const {
    getAllColumns,
    getHeaderGroups,
    getRowModel,
    refineCore: { setCurrent, pageCount, current, tableQueryResult },
  } = useTable({
    columns,
    data: dataRowList,
    refineCoreProps: {
      initialSorter: [
        {
          field: "id",
          order: "desc",
        },
      ],
    },
  });

  const renderEditRow = useCallback((row: Row<PrintJob>) => {
    const {
      deviceId,
      userId,
      color,
      page,
      copies,
      printerName,
      type,
      paperName,
      status,
      createdAt,
      message,
    } = row.original;

    return (
      <React.Fragment key={id}>
        <Tr>
          <Td>{userId}</Td>
          <Td>{deviceId}</Td>
          <Td>
            {color ? (
              <Badge colorScheme="black">Colored</Badge>
            ) : (
              <Badge colorScheme="blue">Black & White</Badge>
            )}
          </Td>
          <Td>{page}</Td>
          <Td>{copies}</Td>
          <Td>{printerName}</Td>
          <Td>
            {rswitch(type, {
              ".doc": <Icon as={BsFiletypeDocx} />,
              ".docx": <Icon as={BsFiletypeDocx} />,
              ".png": <Icon as={BiSolidFilePng} />,
              ".jpg": <Icon as={AiOutlineFileJpg} />,
              ".xls": <Icon as={TbFileTypeXls} />,
              ".xlsx": <Icon as={TbFileTypeXls} />,
              default: <Icon as={FaFilePdf} />,
            })}
          </Td>
          <Td>{paperName}</Td>
          <Td>
            {rswitch(status, {
              pending: <Badge colorScheme="black">Pending</Badge>,
              failed: <Badge colorScheme="red">Failed</Badge>,
              processing: <Badge colorScheme="blue">Processing</Badge>,
              default: <Badge colorScheme="green">Printed</Badge>,
            })}
          </Td>
          <Td>{createdAt}</Td>
          <Td>
            <HStack spacing={4}>
              <EditButton
                hideText
                size="sm"
                onClick={() => {

                }}
              />
              <Button
                id="delete-selected"
                size="xs"
                color="blue"
                variant="outline"
                onClick={() =>
                  alert("clicked")
                }
              >
                Reprint
              </Button>
            </HStack>
          </Td>
        </Tr>
        {row.getIsExpanded() && (
          <Tr id="expanded-row">
            <Td colSpan={getAllColumns().length}>
              <Textarea readOnly value={...register("message")} />
            </Td>
          </Tr>
        )}
      </React.Fragment>
    );
  });

  return (
    <form onSubmit={handleSubmit(onFinish)}>
      <List 
        headerButtons={<CreateButton onClick={() => router.push(DashRoutes.createPrintJob)}/>}
      >
        <TableContainer whiteSpace="pre-line">
          <Table variant="simple">
            <Thead>
              {getHeaderGroups().map((headerGroup: any) => (
                <Tr key={headerGroup.id}>
                  {headerGroup.headers.map((header: any) => (
                    <Th key={header.id}>
                      {!header.isPlaceholder && (
                        <HStack spacing="2">
                          <Box>
                            {flexRender(
                              header.column.columnDef.header,
                              header.getContext()
                            )}
                          </Box>
                          <HStack spacing="2">
                            <ColumnSorter column={header.column} />
                            <ColumnFilter column={header.column} />
                          </HStack>
                        </HStack>
                      )}
                    </Th>
                  ))}
                </Tr>
              ))}
            </Thead>
            <Tbody>
              {getRowModel().rows.map((row: any) => {
                return renderEditRow(row);
              })}
            </Tbody>
          </Table>
        </TableContainer>
        <Pagination
          current={current}
          pageCount={pageCount}
          setCurrent={setCurrent}
        />
      </List>
    </form>
  );
};
