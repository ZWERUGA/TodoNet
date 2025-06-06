import { ReactNode } from "react";

interface ActionButtonProps {
  onClick: () => void;
  loading: boolean;
  disabled?: boolean;
  className?: string;
  children: ReactNode;
}

export default ActionButtonProps;
