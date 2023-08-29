import React, { useRef } from "react";
import { createPortal } from "react-dom";

const Modal = ({ children }) => {
  const elRef = useRef(null);
  if (!elRef.current) {
    elRef.current = document.getElementById("modal");
  }
  return createPortal(<div>{children}</div>, elRef.current);
};

export default Modal;
