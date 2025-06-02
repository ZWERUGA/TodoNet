"use client";

import { useEffect, useState } from "react";
import { useApi } from "./hooks/useApi";
import Todo from "./interfaces/todo";
import { useRouter } from "next/navigation";

export default function Home() {
  const api = useApi();
  const router = useRouter();
  const [todos, setTodos] = useState<Todo[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

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
    <div>
      <h1>Задачи</h1>
      <ul>
        {todos.length == 0 ? (
          <p>Задач нет...</p>
        ) : (
          todos.map((todo) => <li key={todo.id}>{todo.text}</li>)
        )}
      </ul>
      <button
        onClick={handleLogout}
        className="p-2 bg-gray-600 rounded-md cursor-pointer"
      >
        Выйти
      </button>
    </div>
  );
}
