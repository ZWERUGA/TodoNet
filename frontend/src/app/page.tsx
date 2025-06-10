"use client";

import { useEffect, useState } from "react";
import { useApi } from "@/hooks/useApi";
import Todo from "./interfaces/todo";
import { useRouter } from "next/navigation";
import { useTheme } from "./providers/theme-provider";
import CreateTodoForm from "./components/create-form-todo";
import TodoActions from "./components/todo-actions";

export default function Home() {
  const api = useApi();
  const router = useRouter();
  const [todos, setTodos] = useState<Todo[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const { theme, toggleTheme } = useTheme();

  const [loadingId, setLoadingId] = useState<number | null>(null);
  const [loadingAction, setLoadingAction] = useState<string | null>(null);

  const handleToggleComplete = async (id: number) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;

    setLoadingId(id);
    setLoadingAction("complete");

    try {
      const updatedTodo = await api.todos.update(id, {
        isCompleted: !todo.isCompleted,
      });
      setTodos((prev) => prev.map((t) => (t.id === id ? updatedTodo : t)));
    } catch (err) {
      console.error(`Ошибка при обновлении задачи: ${err}`);
    } finally {
      setLoadingId(null);
      setLoadingAction(null);
    }
  };

  const handleToggleFavorite = async (id: number) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;

    setLoadingId(id);
    setLoadingAction("favorite");

    try {
      const updatedTodo = await api.todos.update(id, {
        isFavorite: !todo.isFavorite,
      });
      setTodos((prev) => prev.map((t) => (t.id === id ? updatedTodo : t)));
    } catch (err) {
      console.error(`Ошибка при обновлении задачи: ${err}`);
    } finally {
      setLoadingId(null);
      setLoadingAction(null);
    }
  };

  const handleDelete = async (id: number) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;

    setLoadingId(id);
    setLoadingAction("delete");

    try {
      await api.todos.delete(id);
      setTodos((prev) => prev.filter((t) => t.id !== id));
    } catch (err) {
      console.error(`Ошибка при удалении задачи: ${err}`);
    } finally {
      setLoadingId(null);
      setLoadingAction(null);
    }
  };

  const handleLogout = async () => {
    try {
      await api.account.logout();
      router.push("/account/login");
    } catch (error) {
      console.error("Ошибка при выходе из системы", error);
      router.push("/account/login");
    }
  };

  useEffect(() => {
    const fetchTodos = async () => {
      try {
        const data = await api.todos.getAll();
        setTodos(data);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Ошибка получения данных."
        );
      } finally {
        setLoading(false);
      }
    };

    fetchTodos();
  }, []);

  return (
    <main className="min-h-screen bg-gray-100 dark:bg-gray-900 text-gray-900 dark:text-gray-100 py-10 px-4 transition-colors">
      <div className="max-w-xl mx-auto bg-white dark:bg-gray-800 shadow-md rounded-lg p-6">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-semibold">Список задач</h1>
          <div className="space-x-2">
            <button
              onClick={toggleTheme}
              className="px-3 py-2 text-sm bg-indigo-600 hover:bg-indigo-700 text-white rounded-md cursor-pointer"
            >
              {theme === "dark" ? "Светлая тема" : "Тёмная тема"}
            </button>
            <button
              onClick={handleLogout}
              className="px-3 py-2 text-sm bg-red-500 hover:bg-red-600 text-white rounded-md cursor-pointer"
            >
              Выйти
            </button>
          </div>
        </div>

        {loading && <p className="text-center">Загрузка данных...</p>}
        {error && (
          <p className="text-center text-red-400">Ошибка получения данных...</p>
        )}

        {!loading && (
          <CreateTodoForm
            onTodoCreated={(newTodo) => setTodos((prev) => [newTodo, ...prev])}
          />
        )}

        {!loading && !error && (
          <ul className="space-y-2">
            {todos.length === 0 ? (
              <li>
                <p className="text-gray-500 dark:text-gray-400">
                  Задачи отсутствуют.
                </p>
              </li>
            ) : (
              todos.map((todo) => (
                <li
                  key={todo.id}
                  className="p-3 bg-gray-50 dark:bg-gray-700 border dark:border-gray-600 rounded hover:bg-gray-100 dark:hover:bg-gray-600 transition"
                >
                  <div className="flex justify-between items-start">
                    <div>
                      <h3
                        className={`text-lg font-semibold ${
                          todo.isCompleted
                            ? "line-through text-gray-400"
                            : "text-gray-800 dark:text-gray-200"
                        }`}
                      >
                        {todo.title}
                      </h3>
                      <p
                        className={
                          todo.isCompleted
                            ? "line-through text-gray-400"
                            : "text-gray-800 dark:text-gray-200"
                        }
                      >
                        {todo.text}
                      </p>
                    </div>
                    <div className="space-x-2">
                      <TodoActions
                        todo={todo}
                        loadingId={loadingId}
                        loadingAction={loadingAction}
                        onComplete={handleToggleComplete}
                        onFavorite={handleToggleFavorite}
                        onDelete={handleDelete}
                      />
                    </div>
                  </div>
                </li>
              ))
            )}
          </ul>
        )}
      </div>
    </main>
  );
}
