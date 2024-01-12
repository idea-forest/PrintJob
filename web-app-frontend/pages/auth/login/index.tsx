import { useState } from "react";
import { Formik, Form, Field, FormikHelpers } from "formik";
import { ColumnFlex } from "components";
import { Logo } from "components/dashboard/shared/nav/Logo";
import AuthComponent from "components/auth/AuthComponent";
import { GoogleOAuthProvider, GoogleLogin } from "@react-oauth/google";
import * as Yup from "yup";
import {
  Input,
  FormLabel,
  Button,
  Stack,
  FormControl,
  Text,
  Spinner,
  Box,
} from "@chakra-ui/react";
import { DashRoutes } from "utils/routes";
import Link from "next/link";
import { useUserStore } from "utils/fetch-utils";
import { UserService } from "models/UserService";
import Toast from "components/dashboard/shared/Toast";
import { useUserContext } from "context";
import { useRouter } from "next/router";

interface FormValues {
  teamname: string;
}

interface LoginValues {
  teamid: number;
  email: string;
  password: string;
}

const LoginSchema = Yup.object().shape({
  teamname: Yup.string().required("Team Name Is Required"),
});

const validationSchema = Yup.object().shape({
  email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  password: Yup.string()
    .required("Password is required")
    .min(8, "Password must be at least 8 characters")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()\-_=+{};:,<.>]).{8,}$/,
      "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character"
    ),
});

export default function Login(): JSX.Element {
  const [isLoading, setIsLoading] = useState(false);
  const userStore = useUserStore((state: UserService) => state);
  const [teamNameExists, setTeamNameExists] = useState(false);
  const [teamName, setTeamName] = useState("");
  const [teamId, setTeamId] = useState("");
  const { updateUserInfo } = useUserContext();
  const router = useRouter();

  const handleSubmit = async (
    values: FormValues,
    actions: FormikHelpers<FormValues>
  ): Promise<void> => {
    setIsLoading(true);

    try {
      const response = await userStore.checkTeamName(values.teamname);
      setTimeout(() => {
        setIsLoading(false);
        actions.setSubmitting(false);
        setTeamId(response.id);
        setTeamName(response.userName);
        setTeamNameExists(true);
      }, 1000);
    } catch (error) {
      Toast("Team name check failed", "error");
      setIsLoading(false);
    }
  };

  const handleLoginSubmit = async (
    values: LoginValues,
    actions: FormikHelpers<LoginValues>
  ) => {
    setIsLoading(true);

    try {
      const response = await userStore.login(teamId, values.email, values.password);
      setTimeout(() => {
        if (!response.success) {
          Toast(response?.error, 'error');
          throw new Error(response?.error); 
        }
        updateUserInfo(response);
        setIsLoading(false);
        actions.setSubmitting(false);
        Toast('Login Succesfull', 'success');
        router.push("/dashboard");
      }, 1000);
    } catch (error: any) {
      Toast(error, 'error');
      console.error("Login failed:", error);
      setIsLoading(false);
    }
  };
  return (
    <AuthComponent height="100vh">
      <Box
        w="100%"
        maxW={{ base: "90%", sm: "468px" }}
        py={{ base: "8", sm: "8" }}
        px={{ base: "4", sm: "10" }}
        bg={{ base: "transparent", sm: "bg.surface" }}
        boxShadow={{ base: "none", sm: "md" }}
        borderRadius={{ base: "none", sm: "xl" }}
        backgroundColor="whiteAlpha.900"
      >
        {teamNameExists ? (
          <>
            <ColumnFlex align="center" justify="center">
              <Stack spacing="8">
                <Logo />
                <Stack spacing={{ base: "2", md: "3" }} textAlign="center">
                  <Text color="fg.muted">Sign in to your team {teamName}</Text>
                </Stack>
              </Stack>
            </ColumnFlex>
            <Formik
              initialValues={{
                email: "",
                password: "",
              }}
              validationSchema={validationSchema}
              onSubmit={(values, actions) => handleLoginSubmit(values, actions)}
            >
              {({ errors, touched, isSubmitting }) => (
                <Form>
                  <Stack spacing="6" px={{ base: "4", sm: "0" }}>
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
                      <FormControl
                        isInvalid={errors.password && touched.password}
                      >
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
                    <Stack
                      spacing={{ base: "3", md: "5" }}
                      direction={{ base: "column", md: "row" }}
                      justify="center"
                      align="center"
                    >
                      <GoogleOAuthProvider clientId="796426914634-pe6h46j5tbvc4uq81i4or9kt652aodv0.apps.googleusercontent.com">
                        <GoogleLogin
                          onSuccess={(credentialResponse) => {
                            // Handle Google login success
                          }}
                          onError={() => {
                            // Handle Google login error
                          }}
                        />
                      </GoogleOAuthProvider>
                    </Stack>
                    <Stack spacing="5">
                      <Button type="submit" w="100%" isLoading={isLoading}>
                        {isLoading ? <Spinner size="sm" /> : "Login"}
                      </Button>
                    </Stack>
                  </Stack>
                </Form>
              )}
            </Formik>
          </>
        ) : (
          <>
            <ColumnFlex align="center" justify="center">
              <Stack spacing="8">
                <Logo />
                <Stack spacing={{ base: "2", md: "3" }} textAlign="center">
                  <Text color="fg.muted">Sign in to your team</Text>
                </Stack>
              </Stack>
            </ColumnFlex>
            <Formik
              initialValues={{
                teamname: "",
              }}
              validationSchema={LoginSchema}
              onSubmit={(values, actions) => handleSubmit(values, actions)}
            >
              {({ errors, touched, isSubmitting }) => (
                <Form>
                  <Stack spacing="6" px={{ base: "4", sm: "0" }}>
                    <Stack spacing="5">
                      <FormControl
                        isInvalid={errors.teamname && touched.teamname}
                      >
                        <FormLabel htmlFor="teamname">Team</FormLabel>
                        <Field
                          as={Input}
                          name="teamname"
                          type="text"
                          placeholder="Enter Team Name"
                        />
                        {errors.teamname && touched.teamname && (
                          <Text color="red">{errors.teamname}</Text>
                        )}
                      </FormControl>
                    </Stack>
                    <Stack spacing="5">
                      <Button type="submit" w="100%" isLoading={isLoading}>
                        {isLoading ? <Spinner size="sm" /> : "Continue"}
                      </Button>
                    </Stack>
                  </Stack>
                </Form>
              )}
            </Formik>
          </>
        )}
        <Stack spacing="6">
          <ColumnFlex align="center" justify="center">
            <Text>
              Don&#39;t have a team name?{" "}
              <Link href={DashRoutes.register}>Register</Link>
            </Text>
          </ColumnFlex>
        </Stack>
      </Box>
    </AuthComponent>
  );
}
