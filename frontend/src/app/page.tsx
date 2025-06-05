"use client";

import { useEffect, useState } from "react";
import { useApi } from "./hooks/useApi";
import Todo from "./interfaces/todo";
import { useRouter } from "next/navigation";
import { useTheme } from "./providers/theme-provider";

export default function Home() {
  const api = useApi();
  const router = useRouter();
  const [todos, setTodos] = useState<Todo[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const { theme, toggleTheme } = useTheme();

  const handleToggleComplete = async (id: number) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;

    try {
      const updatedTodo = await api.todos.update(id, {
        isCompleted: !todo.isCompleted,
      });
      setTodos((prev) => prev.map((t) => (t.id === id ? updatedTodo : t)));
    } catch (err) {
      console.error(`Ошибка при обновлении задачи: ${err}`);
    }
  };

  const handleToggleFavorite = async (id: number) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;

    try {
      const updatedTodo = await api.todos.update(id, {
        isFavorite: !todo.isFavorite,
      });
      setTodos((prev) => prev.map((t) => (t.id === id ? updatedTodo : t)));
    } catch (err) {
      console.error(`Ошибка при обновлении задачи: ${err}`);
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

  if (loading) return <div>Загрузка данных...</div>;
  if (error) return <div>Ошибка: {error}...</div>;

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

        {loading && <p>Загрузка данных...</p>}
        {error && <p className="text-red-400">Ошибка: {error}</p>}

        {!loading && !error && (
          <ul className="space-y-2">
            {todos.length === 0 ? (
              <p className="text-gray-500 dark:text-gray-400">
                Задачи отсутствуют.
              </p>
            ) : (
              todos.map((todo) => (
                <li
                  key={todo.id}
                  className="p-3 bg-gray-50 dark:bg-gray-700 border dark:border-gray-600 rounded hover:bg-gray-100 dark:hover:bg-gray-600 transition flex justify-between items-center"
                >
                  <span
                    className={
                      todo.isCompleted ? "line-through text-gray-400" : ""
                    }
                  >
                    {todo.text}
                  </span>
                  <div className="space-x-2">
                    <button
                      onClick={() => handleToggleComplete(todo.id)}
                      className={`px-2 py-1 text-sm rounded cursor-pointer ${
                        todo.isCompleted
                          ? "bg-green-400 text-white hover:bg-green-500"
                          : "bg-gray-300 text-black hover:bg-gray-400"
                      }`}
                    >
                      ✔
                    </button>
                    <button
                      onClick={() => handleToggleFavorite(todo.id)}
                      className={`px-2 py-1 text-sm rounded cursor-pointer ${
                        todo.isFavorite
                          ? "bg-yellow-400 text-black hover:bg-yellow-500"
                          : "bg-gray-300 text-black hover:bg-gray-400"
                      }`}
                    >
                      ⭐
                    </button>
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
