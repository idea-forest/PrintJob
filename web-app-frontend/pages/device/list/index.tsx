import { DEFAULT_STYLES } from "styles";
import React, { useMemo } from "react";
import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Box,
  VStack,
  TableContainer,
  Grid,
  chakra,
} from "@chakra-ui/react";
import { useTable, useSortBy } from "react-table";
import { TriangleDownIcon, TriangleUpIcon } from "@chakra-ui/icons";
import { DashBoardLayout } from "layout";
import { Header, ColumnFlex } from "components";
interface ConvertData {
  fromUnit: string;
  toUnit: string;
  factor: number;
}

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

  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
    useTable({ columns, data }, useSortBy);

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
          <Table variant='simple' {...getTableProps()}>
            <Thead>
              {headerGroups.map((headerGroup) => (
                <Tr {...headerGroup.getHeaderGroupProps()}>
                  {headerGroup.headers.map((column) => (
                    <Th
                      {...column.getHeaderProps(column.getSortByToggleProps())}
                      isNumeric={column.isNumeric}
                    >
                      {column.render("Header")}
                      <chakra.span pl="4">
                        {column.isSorted ? (
                          column.isSortedDesc ? (
                            <TriangleDownIcon aria-label="sorted descending" />
                          ) : (
                            <TriangleUpIcon aria-label="sorted ascending" />
                          )
                        ) : null}
                      </chakra.span>
                    </Th>
                  ))}
                </Tr>
              ))}
            </Thead>
            <Tbody {...getTableBodyProps()}>
              {rows.map((row) => {
                prepareRow(row);
                return (
                  <Tr {...row.getRowProps()}>
                    {row.cells.map((cell) => (
                      <Td
                        {...cell.getCellProps()}
                        isNumeric={cell.column.isNumeric}
                      >
                        {cell.render("Cell")}
                      </Td>
                    ))}
                  </Tr>
                );
              })}
            </Tbody>
          </Table>
      </Grid>
    </DashBoardLayout>
  );
};

export default DeviceList;
