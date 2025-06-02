import { API_ENDPOINTS } from "../endpoints";
import CreateTodoDto from "../interfaces/createTodoDto";
import Todo from "../interfaces/todo";
import UpdateTodoDto from "../interfaces/updateTodoDto";

function getCsrfToken(): string | null {
  if (typeof document === "undefined") return null;

  const match = document.cookie
    .split("; ")
    .find((row) => row.startsWith("csrfToken="));

  return match?.split("=")[1] ?? null;
}

export function useApi() {
  const request = async <T>(url: string, options?: RequestInit): Promise<T> => {
    const csrfToken = getCsrfToken();

    const defaultHeaders: HeadersInit = {
      "Content-Type": "application/json",
    };

    if (
      options?.method &&
      ["POST", "PUT", "DELETE", "PATCH"].includes(options.method.toUpperCase())
    ) {
      if (csrfToken) {
        defaultHeaders["X-CSRF-TOKEN"] = csrfToken;
      }
    }

    const response = await fetch(url, {
      credentials: "include",
      headers: {
        ...defaultHeaders,
        ...(options?.headers ?? {}),
      },
      ...options,
    });

    if (response.status === 401) {
      window.location.href = "account/login";
      throw new Error("Сессия иссекла. Пожалуйста, войдите заново.");
    }

    if (!response.ok) throw new Error(await response.text());

    return response.json();
  };

  return {
    account: {
      register: (data: { userName: string; email: string; password: string }) =>
        request<{ userName: string; email: string; }>(
          API_ENDPOINTS.ACCOUNT.REGISTER,
          {
            method: "POST",
            body: JSON.stringify(data),
          }
        ),

      login: (data: { userName: string; password: string }) =>
        request<{ userName: string; email: string; }>(
          API_ENDPOINTS.ACCOUNT.LOGIN,
          {
            method: "POST",
            body: JSON.stringify(data),
          }
        ),
      logout: () =>
        request<{ message: string }>(API_ENDPOINTS.ACCOUNT.LOGOUT, {
          method: "POST",
        }),
    },

    todos: {
      getAll: () => request<Array<Todo>>(API_ENDPOINTS.TODO.GET_ALL),
      create: (data: CreateTodoDto) =>
        request<Todo>(API_ENDPOINTS.TODO.CREATE, {
          method: "POST",
          body: JSON.stringify(data),
        }),
      getById: (id: number) => request<Todo>(API_ENDPOINTS.TODO.GET_BY_ID(id)),
      update: (id: number, data: UpdateTodoDto) =>
        request<Todo>(API_ENDPOINTS.TODO.UPDATE(id), {
          method: "PUT",
          body: JSON.stringify(data),
        }),
      delete: (id: number) =>
        request<void>(API_ENDPOINTS.TODO.DELETE(id), {
          method: "DELETE",
        }),
    },
  };
}
