"use client";

import { useApi } from "@/app/hooks/useApi";
import { useRouter } from "next/navigation";
import { useState } from "react";

export default function RegisterPage() {
  const api = useApi();
  const router = useRouter();

  const [formData, setFormData] = useState({
    userName: "",
    email: "",
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
      await api.account.register(formData);
      router.push("/");
    } catch (error: any) {
      console.log(error);
      setResponseMsg(error.message || "Ошибка регистрации");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-900">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded shadow-md w-full max-w-md space-y-4"
      >
        <h1 className="text-2xl font-bold mb-4 text-center text-gray-700">
          Регистрация
        </h1>

        <div>
          <label className="block text-sm font-medium text-gray-700">
            Имя пользователя
          </label>
          <input
            type="text"
            name="userName"
            value={formData.userName}
            onChange={handleChange}
            required
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-200 text-gray-700"
            autoFocus
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">
            Email
          </label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-200 text-gray-700"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">
            Пароль
          </label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
            className="mt-1 w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-200 text-gray-700"
          />
        </div>

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition cursor-pointer"
        >
          {isSubmitting ? "Регистрация..." : "Зарегистрироваться"}
        </button>

        {responseMsg && (
          <p className="text-center text-sm mt-4 text-red-600">{responseMsg}</p>
        )}
      </form>
    </div>
  );
}
