// components/TodoActions.tsx
import { FaCheck, FaStar, FaTrash } from "react-icons/fa";
import TodoActionsProps from "../interfaces/todo-actions";
import ActionButton from "./action-button";

export default function TodoActions({
  todo,
  loadingId,
  loadingAction,
  onComplete,
  onFavorite,
  onDelete,
}: TodoActionsProps) {
  const isLoading = (action: string) =>
    loadingId === todo.id && loadingAction === action;

  return (
    <div className="space-x-2 flex">
      <ActionButton
        onClick={() => onComplete(todo.id)}
        loading={isLoading("complete")}
        className={
          todo.isCompleted
            ? "bg-green-400 text-white hover:bg-green-500"
            : "bg-gray-300 text-black hover:bg-gray-400"
        }
      >
        <FaCheck className="w-4 h-4" />
      </ActionButton>

      <ActionButton
        onClick={() => onFavorite(todo.id)}
        loading={isLoading("favorite")}
        className={
          todo.isFavorite
            ? "bg-yellow-600 hover:bg-yellow-600"
            : "bg-gray-300 text-black hover:bg-gray-400"
        }
      >
        <FaStar className="w-4 h-4" />
      </ActionButton>

      <ActionButton
        onClick={() => onDelete(todo.id)}
        loading={isLoading("delete")}
        className="bg-red-500 hover:bg-red-800"
      >
        <FaTrash className="w-4 h-4" />
      </ActionButton>
    </div>
  );
}
