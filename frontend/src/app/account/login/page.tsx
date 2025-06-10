"use client";

import { useApi } from "@/hooks/useApi";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useState } from "react";

export default function LoginPage() {
  const api = useApi();
  const router = useRouter();

  const [formData, setFormData] = useState({
    userName: "",
    password: "",
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [responseMsg, setResponseMsg] = useState("");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    if (isSubmitting) return;

    e.preventDefault();
    setIsSubmitting(true);
    setResponseMsg("");

    try {
      await api.account.login(formData);
      router.push("/");
    } catch (error: any) {
      setResponseMsg(error.message || "Ошибка входа в систему");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100 dark:bg-gray-900 transition-colors">
      <form
        onSubmit={handleSubmit}
        className="bg-white dark:bg-gray-800 p-8 rounded shadow-md w-full max-w-md space-y-4"
      >
        <h1 className="text-2xl font-bold mb-4 text-center text-gray-800 dark:text-gray-100">
          Вход в систему
        </h1>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
            Имя пользователя
          </label>
          <input
            type="text"
            name="userName"
            value={formData.userName}
            onChange={handleChange}
            required
            autoFocus
            className="mt-1 w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-100 placeholder-gray-400 dark:placeholder-gray-400 focus:outline-none focus:ring focus:ring-blue-300 dark:focus:ring-blue-500"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
            Пароль
          </label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
            className="mt-1 w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-100 placeholder-gray-400 dark:placeholder-gray-400 focus:outline-none focus:ring focus:ring-blue-300 dark:focus:ring-blue-500"
          />
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded transition disabled:opacity-60 cursor-pointer"
        >
          {isSubmitting ? "Вход..." : "Войти"}
        </button>

        {responseMsg && (
          <p className="text-center text-sm mt-4 text-red-600 dark:text-red-400">
            {responseMsg}
          </p>
        )}

        {/* Ссылка на регистрацию */}
        <p className="text-center text-sm text-gray-600 dark:text-gray-300 mt-2">
          Нет аккаунта?{" "}
          <Link
            href="/account/register"
            className="text-blue-600 dark:text-blue-400 hover:underline"
          >
            Зарегистрироваться
          </Link>
        </p>
      </form>
    </div>
  );
}
