import React from "react";
import Loader from "./loader";
import ActionButtonProps from "../interfaces/action-button";

export default function ActionButton({
  onClick,
  loading,
  disabled = false,
  className = "",
  children,
}: ActionButtonProps) {
  return (
    <button
      onClick={onClick}
      disabled={disabled || loading}
      className={`px-2 py-2 text-sm rounded cursor-pointer disabled:bg-gray-300 disabled:cursor-not-allowed flex items-center justify-center ${className}`}
    >
      {loading ? <Loader /> : children}
    </button>
  );
}
