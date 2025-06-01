"use client";

import { useEffect } from "react";
import { useApi } from "@/app/hooks/useApi";
import { useRouter } from "next/navigation";

export default function LogoutPage() {
  const api = useApi();
  const router = useRouter();

  useEffect(() => {
    const logout = async () => {
      try {
        await api.account.logout();
        router.push("/account/login");
      } catch (error) {
        console.error("Ошибка при выходе из системы", error);
        router.push("/account/login");
      }
    };
    logout();
  }, [api, router]);

  return <div>Выход из системы...</div>;
}
