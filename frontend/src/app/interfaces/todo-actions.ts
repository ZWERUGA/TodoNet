import Todo from "./todo";

interface TodoActionsProps {
  todo: Todo;
  loadingId: number | null;
  loadingAction: string | null;
  onComplete: (id: number) => void;
  onFavorite: (id: number) => void;
  onDelete: (id: number) => void;
}

export default TodoActionsProps;
