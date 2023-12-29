import { useEffect } from "react";
import { useRouter } from "next/router";
import type { AppProps } from "next/app";
import { outFit, theme } from "styles";
import { ChakraProvider } from "@chakra-ui/react";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { GlobalLayout } from "layout";
import "styles/globals.css";

export default function App({ Component, pageProps }: AppProps) {
  const router = useRouter();
  // useEffect(() => {
  //   const user: string | null = localStorage.getItem("user");
  //   if (user !== null) {
  //     router.push("/dashboard");
  //   }
  // }, [router]);

  return (
    <ChakraProvider theme={theme}>
      <style jsx global>{`
        html,
        body,
        * {
          font-family: ${outFit.style.fontFamily};
        }
      `}</style>
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
    </ChakraProvider>
  );
}
