import React from "react";
import { toast, ToastOptions } from "react-toastify";

type ToastType = "success" | "error" | "warning";

const Toast = (message: string, type: ToastType): void => {
  const options: ToastOptions = {
    position: toast.POSITION.TOP_RIGHT,
    autoClose: 3000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
    theme: "light",
  };

  switch (type) {
    case "success":
      toast.success(
        <div>
          <p>{message}</p>
        </div>,
        options
      );
      break;
    case "error":
      toast.error(
        <div>
          <p>{message}</p>
        </div>,
        options
      );
      break;
    case "warning":
      toast.warning(
        <div>
          <p>{message}</p>
        </div>,
        options
      );
      break;
    default:
      toast.warning(
        <div>
          <p>Toast not defined...</p>
        </div>,
        options
      );
      break;
  }
};

export default Toast;
