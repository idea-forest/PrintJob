import { DEFAULT_STYLES } from 'styles';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalCloseButton,
  ModalBody,
  Button,
  ButtonProps,
} from '@chakra-ui/react';
import { ReactNode } from 'react';

interface Props extends ButtonProps {
  isOpen: boolean;
  onClose: () => void;
  data: ReactNode;
  btnText?: string;
}

export const DrawModal = ({
  isOpen,
  onClose,
  data,
  btnText,
  ...restBtn
}: Props) => {
  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      isCentered
      motionPreset='slideInBottom'
      size='sm'
    >
      <ModalOverlay />
      <ModalContent>
        <ModalCloseButton
          border={DEFAULT_STYLES.border}
          rounded='full'
          fontSize='0.8rem'
          _hover={{
            bg: 'transparent',
          }}
        />
        <ModalBody
          display='flex'
          flexDirection='column'
          gap='3'
          minH='240px'
          pt='10'
          pb='7'
          justifyContent='center'
        >
          {data}

          {btnText && (
            <Button
              w={DEFAULT_STYLES.fullWidth}
              variant='dark'
              size='smPadding'
              {...restBtn}
            >
              {btnText}
            </Button>
          )}
        </ModalBody>
      </ModalContent>
    </Modal>
  );
};
