import React, { HTMLInputTypeAttribute } from 'react';
import { ReactNode } from 'react';
import {
  FormControl,
  FormControlProps,
  FormHelperText,
  FormLabel,
  Input,
  InputGroup,
  InputProps,
  InputRightElement,
  Text,
} from '@chakra-ui/react';
import { UseFormRegisterReturn } from 'react-hook-form';
import { DEFAULT_STYLES, textStyles } from 'styles';

interface Props {
  // Pass either label or placeholder for either one to show
  label?: string;
  placeholder?: string;
  errorMessage?: string;
  helperText?: string;

  id?: string;
  type?: HTMLInputTypeAttribute;
  value?: string;
  register?: UseFormRegisterReturn;
  // Use this when not using register
  onChange?: (e: any) => void;

  // For passing additional styles to elements
  inputStyle?: InputProps;
  formStyle?: FormControlProps;
  rightAddon?: ReactNode;
  // This is when I just wanted to use the form label and error of this inputcomp with select for example
  children?: ReactNode;
}

export const InputComp = ({
  label,
  placeholder,
  errorMessage,
  id,
  register,
  onChange,
  type,
  value,
  formStyle,
  inputStyle,
  rightAddon,
  children,
  helperText,
}: Props) => {
  // Shared input props
  const inputProps = {
    placeholder: placeholder,
    id: id,
    type: type,
    width: DEFAULT_STYLES.fullWidth,
    _placeholder: textStyles.placeholder,
    ...textStyles.bodyText,
  };

  return (
    <FormControl
      display='flex'
      justifyContent='center'
      alignItems='start'
      flexDirection='column'
      {...formStyle}
    >
      {label && (
        <FormLabel
          {...textStyles.label}
          width={DEFAULT_STYLES.fullWidth}
          pt={1}
          className='capitalize'
        >
          {label}
        </FormLabel>
      )}

      {!children && (
        <InputGroup>
          {register && (
            <Input
              value={value}
              {...inputProps}
              size='md'
              {...register}
              {...inputStyle}
            />
          )}
          {/* When register is not used */}
          {onChange && (
            <Input
              value={value}
              {...inputProps}
              size='md'
              {...inputStyle}
              onChange={(e) => onChange(e.target.value)}
            />
          )}
          {rightAddon && <InputRightElement>{rightAddon}</InputRightElement>}
        </InputGroup>
      )}

      {children}

      {errorMessage && (
        <Text
          alignSelf='start'
          textStyle='info'
          color={DEFAULT_STYLES.errorColor}
          pt={1}
          className='capitalize'
        >
          {errorMessage}
        </Text>
      )}

      {helperText && (
        <FormHelperText
          {...textStyles.info}
          color='rgba(0, 0, 0, 0.5)'
          alignSelf='flex-start'
        >
          {helperText}
        </FormHelperText>
      )}
    </FormControl>
  );
};
