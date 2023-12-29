import { useState } from "react";
import { Formik, Form, Field, FormikHelpers } from "formik";
import * as Yup from "yup";
import {
  Input,
  FormLabel,
  Button,
  Stack,
  FormControl,
  Text,
  Spinner,
  Box
} from "@chakra-ui/react";
import AuthComponent from "components/auth/AuthComponent";
import ReCAPTCHA from "react-google-recaptcha";
import { ColumnFlex } from "components";
import { Logo } from "components/dashboard/shared/nav/Logo";
import { UserService } from "models/UserService";
import { useUserStore } from "utils/fetch-utils";
import Link from "next/link";
import { DashRoutes } from "utils";
import Toast from "components/dashboard/shared/Toast";
import { useRouter } from "next/router";

interface RegisterFormValues {
  teamName: string;
  email: string;
  password: string;
  confirmPassword: string;
  recaptchaValue: string;
}

const validationSchema = Yup.object().shape({
  teamName: Yup.string().required("Team name is required"),
  email: Yup.string().email("Invalid email").required("Email is required"),
  password: Yup.string().required("Password is required"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("password"), null], "Passwords must match")
    .required("Confirm password is required"),
});

export default function Register() {
  const [isLoading, setIsLoading] = useState(false);
  const [isCaptchaValid, setIsCaptchaValid] = useState(false);
  const userStore = useUserStore((state: UserService) => state);
  const router = useRouter();

  const handleRegisterSubmit = async (
    values: RegisterFormValues,
    actions: FormikHelpers<RegisterFormValues>
  ) => {
    if (!isCaptchaValid) {
      actions.setFieldError("recaptchaValue", "Please complete the ReCAPTCHA");
      actions.setSubmitting(false);
      return;
    }

    setIsLoading(true);

    try {
      const response = await userStore.register(values.teamName, values.email, values.password);
      setTimeout(() => {
        setIsLoading(false);
        actions.setSubmitting(false);

        if(! response.success) {
          Toast(response.errors[0], 'error');
        } else {
          Toast("Registration Successful", 'success');
          router.push('/');
        }
      }, 2000);
    } catch (error: any) {
      setIsLoading(false);
      Toast(error, 'error');
    }
  };

  return (
    <AuthComponent>
      <Box
        w="100%"
        maxW={{ base: "90%", sm: "468px" }}
        py={{ base: "8", sm: "8" }}
        px={{ base: "4", sm: "10" }}
        bg={{ base: "transparent", sm: "bg.surface" }}
        boxShadow={{ base: "none", sm: "md" }}
        borderRadius={{ base: "none", sm: "xl" }}
        backgroundColor="whiteAlpha.900"
        mt="90px"
      >
        <ColumnFlex align="center" justify="center">
          <Stack spacing="8">
            <Logo />
            <Stack spacing={{ base: "2", md: "3" }} textAlign="center">
              <Text color="fg.muted">Sign up</Text>
            </Stack>
          </Stack>
        </ColumnFlex>
        <Formik
          initialValues={{
            teamName: "",
            email: "",
            password: "",
            confirmPassword: "",
            recaptchaValue: "",
          }}
          validationSchema={validationSchema}
          onSubmit={(values, actions) => handleRegisterSubmit(values, actions)}
        >
          {({ errors, touched, isSubmitting }) => (
            <Form>
              <Stack spacing="6" px={{ base: "4", sm: "0" }}>
                <Stack spacing="5">
                  <FormControl isInvalid={errors.teamName && touched.teamName}>
                    <FormLabel htmlFor="teamName">Team Name</FormLabel>
                    <Field
                      as={Input}
                      name="teamName"
                      placeholder="Enter Team Name"
                    />
                    {errors.teamName && touched.teamName && (
                      <Text color="red">{errors.teamName}</Text>
                    )}
                  </FormControl>
                </Stack>
                <Stack spacing="5">
                  <FormControl isInvalid={errors.email && touched.email}>
                    <FormLabel htmlFor="email">Email</FormLabel>
                    <Field
                      as={Input}
                      name="email"
                      type="email"
                      placeholder="Enter Email"
                    />
                    {errors.email && touched.email && (
                      <Text color="red">{errors.email}</Text>
                    )}
                  </FormControl>
                </Stack>
                <Stack spacing="5">
                  <FormControl isInvalid={errors.password && touched.password}>
                    <FormLabel htmlFor="password">Password</FormLabel>
                    <Field
                      as={Input}
                      name="password"
                      type="password"
                      placeholder="Enter Password"
                    />
                    {errors.password && touched.password && (
                      <Text color="red">{errors.password}</Text>
                    )}
                  </FormControl>
                </Stack>
                <Stack spacing="5">
                  <FormControl
                    isInvalid={
                      errors.confirmPassword && touched.confirmPassword
                    }
                  >
                    <FormLabel htmlFor="confirmPassword">
                      Confirm Password
                    </FormLabel>
                    <Field
                      as={Input}
                      name="confirmPassword"
                      type="password"
                      placeholder="Confirm Password"
                    />
                    {errors.confirmPassword && touched.confirmPassword && (
                      <Text color="red">{errors.confirmPassword}</Text>
                    )}
                  </FormControl>
                </Stack>
                <Stack spacing="5">
                  <ReCAPTCHA
                    sitekey="6LdMyS8pAAAAAGCFozA-wKJlUzlkS8LM98-hxq1-"
                    onChange={(value) => {
                      setIsCaptchaValid(true);
                    }}
                    onExpired={() => setIsCaptchaValid(false)}
                    onErrored={() => setIsCaptchaValid(false)}
                  />
                  {errors.recaptchaValue && touched.recaptchaValue && (
                    <Text color="red">{errors.recaptchaValue}</Text>
                  )}
                </Stack>
                <Stack spacing="5">
                  <Button type="submit" w="100%" isLoading={isLoading}>
                    {isLoading ? <Spinner size="sm" /> : "Register"}
                  </Button>
                </Stack>
              </Stack>
            </Form>
          )}
        </Formik>
        <Stack spacing="6">
          <ColumnFlex align="center" justify="center">
            <Text>
              <Link href={DashRoutes.login}>Go Back</Link>
            </Text>
          </ColumnFlex>
        </Stack>
      </Box>
    </AuthComponent>
  );
}
