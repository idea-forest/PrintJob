import React from "react";
import { Box, Flex, Text, Badge } from "@chakra-ui/react";
import { IoColorPaletteOutline } from "react-icons/io5";

const PrinterColor = ({ name, color, isSelected, onSelect }) => {
  return (
    <Box
      borderWidth="1px"
      borderRadius="xl"
      p={6}
      m={4}
      maxW="450px"
      position="relative"
      bg={isSelected ? "blue.200" : "white"}
      onClick={() => onSelect(!isSelected, color)}
      cursor="pointer"
      boxShadow="lg"
      transition="transform 0.2s"
      _hover={{
        transform: "scale(1.05)",
      }}
    >
      <Flex
        direction="column"
        justifyContent="center"
        alignItems="center"
        textAlign="center"
      >
        <Box mb={3}>
          <IoColorPaletteOutline size={50} color="gray.500" />
        </Box>
        <Text fontWeight="bold" fontSize="1xl">{name}</Text>
      </Flex>
    </Box>
  );
};

export default PrinterColor;
