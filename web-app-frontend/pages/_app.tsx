import type { AppProps } from "next/app";
import { outFit, theme } from "styles";
import { ChakraProvider } from "@chakra-ui/react";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { GlobalLayout } from "layout";
import "styles/globals.css";
import { UserProvider } from "context/user";
import { AuthProvider } from "context";

export default function App({ Component, pageProps }: AppProps) {
  return (
    <ChakraProvider theme={theme}>
      <style jsx global>{`
        html,
        body,
        * {
          font-family: ${outFit.style.fontFamily};
        }
      `}</style>
      <UserProvider>
        <AuthProvider>
          <GlobalLayout>
            <ToastContainer
              position="top-right"
              autoClose={5000}
              hideProgressBar={false}
              newestOnTop={false}
              closeOnClick
              rtl={false}
              pauseOnFocusLoss
              draggable
              pauseOnHover
              theme="light"
            />
            <Component {...pageProps} />
          </GlobalLayout>
        </AuthProvider>
      </UserProvider>
    </ChakraProvider>
  );
}
