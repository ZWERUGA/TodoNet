import type { Metadata } from "next";
import { Roboto_Condensed } from "next/font/google";
import "./globals.css";

const robotoCondensed = Roboto_Condensed({
  variable: "--font-roboto-condensed",
  subsets: ["latin", "cyrillic"],
  weight: ["400", "700"],
  display: "swap",
});

export const metadata: Metadata = {
  title: "TodoNET",
  description: "Создание и ведение задач",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="ru" className={robotoCondensed.className}>
      <body className="antialiased">{children}</body>
    </html>
  );
}
