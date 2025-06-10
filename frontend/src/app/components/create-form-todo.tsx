"use client";

import { useState } from "react";
import Todo from "../interfaces/todo";
import { useApi } from "@/hooks/useApi";
import CreateTodoDto from "../interfaces/create-todo-dto";

interface Props {
  onTodoCreated?: (todo: Todo) => void;
}

export default function CreateTodoForm({ onTodoCreated }: Props) {
  const api = useApi();
  const [title, setTitle] = useState("");
  const [text, setText] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!title.trim() || !text.trim()) {
      setError("Заполните заголовок и текст задачи.");
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const newTodo = await api.todos.create({ title, text } as CreateTodoDto);
      setTitle("");
      setText("");
      if (onTodoCreated) {
        onTodoCreated(newTodo);
      }
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Ошибка при создании задачи."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="mb-6 space-y-4">
      <div>
        <label className="block mb-1 text-sm font-medium">Заголовок</label>
        <input
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="Например, Продукты"
          className="w-full px-4 py-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
        />
      </div>
      <div>
        <label className="block mb-1 text-sm font-medium">Текст задачи</label>
        <textarea
          value={text}
          onChange={(e) => setText(e.target.value)}
          placeholder="Например, Купить продукты в магазине Лента"
          className="w-full px-4 py-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
        />
      </div>
      <button
        type="submit"
        disabled={loading}
        className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded disabled:opacity-50 cursor-pointer"
      >
        {loading ? "Создание..." : "Добавить задачу"}
      </button>
      {error && <p className="text-red-500">{error}</p>}
    </form>
  );
}
