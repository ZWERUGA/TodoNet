const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5289";
const API_ACCOUNT_URL = "api/account";
const API_TODO_URL = "api/todos";

export const API_ENDPOINTS = {
  ACCOUNT: {
    REGISTER: `${API_BASE_URL}/${API_ACCOUNT_URL}/register`,
    LOGIN: `${API_BASE_URL}/${API_ACCOUNT_URL}/login`,
    LOGOUT: `${API_BASE_URL}/${API_ACCOUNT_URL}/logout`,
  },
  TODO: {
    GET_ALL: `${API_BASE_URL}/${API_TODO_URL}`,
    CREATE: `${API_BASE_URL}/${API_TODO_URL}`,
    GET_BY_ID: (id: number) => `${API_BASE_URL}/${API_TODO_URL}/${id}`,
    UPDATE: (id: number) => `${API_BASE_URL}/${API_TODO_URL}/${id}`,
    DELETE: (id: number) => `${API_BASE_URL}/${API_TODO_URL}/${id}`,
  },
} as const;

export type ApiEndpoints = typeof API_ENDPOINTS;
