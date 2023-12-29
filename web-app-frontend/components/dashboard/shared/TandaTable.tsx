import { DEFAULT_STYLES } from 'styles';
import {
  Table,
  TableContainer,
  TableProps,
  TableRowProps,
  Tbody,
  Text,
  Th,
  Thead,
  Tr,
} from '@chakra-ui/react';

interface Props extends TableProps {
  tableHeaders: string[];
}

export const TandaTable = ({ tableHeaders, children, ...t }: Props) => {
  const renderTableHeaders = tableHeaders.map((h, i, arr) => {
    const isNumeric = i === arr.length - 1;

    return (
      <Th
        key={`${h}_${i}`}
        isNumeric={isNumeric}
        color={DEFAULT_STYLES.primaryHeaderColor}
        opacity={DEFAULT_STYLES.opacity}
      >
        <Text>{h}</Text>
      </Th>
    );
  });

  return (
    <TableContainer minW={DEFAULT_STYLES.fullWidth}>
      <Table variant='simple' {...t}>
        <Thead>
          <Tr>{renderTableHeaders}</Tr>
        </Thead>

        <Tbody>{children}</Tbody>
      </Table>
    </TableContainer>
  );
};

export const TandaTableRow = ({ ...r }: TableRowProps) => (
  <Tr textStyle='bolderBody' {...r}>
    {r.children}
  </Tr>
);
