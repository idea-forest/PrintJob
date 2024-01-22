import React from "react";
import { Box, Flex, Text, Badge } from "@chakra-ui/react";

const DeviceRadioCard = ({ device, isSelected, onSelect }) => {
  return (
    <Box
      borderWidth="1px"
      borderRadius="xl"
      p={6}
      m={4}
      maxW="450px"
      position="relative"
      bg={isSelected ? "blue.200" : "white"}
      onClick={() => onSelect(!isSelected)}
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
        <Text fontWeight="bold" fontSize="2xl">
          {device.deviceId}
        </Text>
        {device.deviceStatus ? (
          <Badge
            mt={3}
            fontSize="xl"
            colorScheme="green"
            variant="subtle"
          >
            ONLINE
          </Badge>
        ) : (
          <Badge
            mt={3}
            fontSize="xl"
            colorScheme="red"
            variant="subtle"
          >
            OFFLINE
          </Badge>
        )}
        <Text mt={3} fontSize="sm" color="gray.600">
          {device.machineName}
        </Text>
      </Flex>
    </Box>
  );
};

export default DeviceRadioCard;
