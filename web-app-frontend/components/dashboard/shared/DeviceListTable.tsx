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
  IconButton,
  Textarea,
  Box,
  Badge,
} from "@chakra-ui/react";
import { List } from "@refinedev/chakra-ui";
import { useForm } from "@refinedev/react-hook-form";
import { IconChevronDown, IconChevronRight } from "@tabler/icons";
import { ColumnFilter, ColumnSorter } from "components/table";
import { Pagination } from "components/pagination";
import { FilterElementProps } from "models/FilterElementProps";
import { Device } from "models/device";

interface DeviceListTableProps {
  dataRowList: any[];
}

export const DeviceListTable: React.FC<DeviceListTableProps> = ({
  dataRowList,
}) => {
  const {
    refineCore: { onFinish, id, setId },
    handleSubmit,
    register,
  } = useForm<Device>({
    refineCoreProps: {
      redirect: false,
      action: "edit",
    },
  });

  const columns = React.useMemo<ColumnDef<Device>[]>(
    () => [
      {
        id: "id",
        header: "",
        accessorKey: "id",
        enableColumnFilter: false,
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
        id: "machineName",
        header: "Machine Name",
        accessorKey: "machineName",
        enableColumnFilter: false,
      },
      {
        id: "ipAddress",
        header: "IP Address",
        accessorKey: "ipAddress",
        enableColumnFilter: false,
      },
      {
        id: "os",
        header: "OS",
        accessorKey: "os",
        enableColumnFilter: false,
      },
      {
        id: "deviceStatus",
        header: "Device Status",
        accessorKey: "deviceStatus",
        meta: {
          filterElement: function render(props: FilterElementProps) {
            return (
              <Select
                borderRadius="md"
                size="sm"
                placeholder="All Device Status"
                {...props}
              >
                <option value="online">Online</option>
                <option value="offline">Offline</option>
              </Select>
            );
          },
          filterOperator: "eq",
        },
      },
    ],
    []
  );

  const {
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

  const renderEditRow = useCallback((row: Row<Device>) => {
    const { deviceId, machineName, ipAddress, os, deviceStatus } = row.original;

    return (
      <React.Fragment key={id}>
        <Tr>
          <Td>
            <IconButton
              aria-label="Collapse / Expand"
              onClick={() => row.toggleExpanded()}
            >
              {row.getIsExpanded() ? <IconChevronDown /> : <IconChevronRight />}
            </IconButton>
          </Td>
          <Td>{deviceId}</Td>
          <Td>{machineName}</Td>
          <Td>{ipAddress}</Td>
          <Td>{os}</Td>
          <Td>
            {deviceStatus ? (
              <>
                <Badge colorScheme="green">Online</Badge>
              </>
            ) : (
              <>
                <Badge colorScheme="red">Offline</Badge>
              </>
            )}
          </Td>
        </Tr>
      </React.Fragment>
    );
  });

  return (
    <form onSubmit={handleSubmit(onFinish)}>
      <List>
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
                // if (id === (row.original as Device).id) {
                //   return renderEditRow(row);
                // } else
                //   return (
                //     <Fragment key={row.id}>
                //       <Tr>
                //         {row.getVisibleCells().map((cell: any) => {
                //           return (
                //             <Td key={cell.id}>
                //               {flexRender(
                //                 cell.column.columnDef.cell,
                //                 cell.getContext()
                //               )}
                //             </Td>
                //           );
                //         })}
                //       </Tr>
                //     </Fragment>
                //   );
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
