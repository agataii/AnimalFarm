# AnimalFarm — фронтенд

Одностраничное приложение на React + TypeScript + Vite для учёта животных на ферме. Работает с бэкендом AnimalFarm API.

## Возможности

- **Аутентификация**: вход, регистрация, активация по ссылке из письма (в dev ссылка выводится в консоль бэкенда).
- **Роли**: обычный пользователь (User) и администратор (Admin). У админа доступен раздел «Пользователи».
- **Справочники и данные**: типы животных, породы, животные, взвешивания — списки и формы создания/редактирования с валидацией.

## Стек

- React 19, TypeScript
- Vite 7
- React Router 7
- TanStack React Query
- React Hook Form + Zod (валидация)
- Axios (HTTP)
- Tailwind CSS 4

## Требования

- Node.js 18+
- npm (или pnpm/yarn)

## Установка и запуск

1. Установите зависимости:

   ```bash
   npm install
   ```

2. В корне проекта создайте файл `.env` (если его ещё нет):

   ```
   VITE_API_URL=http://localhost:5000/api
   ```

3. Запустите бэкенд на порту 5000 (см. README в папке `backend`).

4. Запуск режима разработки:

   ```bash
   npm run dev
   ```

   Приложение откроется по адресу [http://localhost:5173](http://localhost:5173).

5. Сборка для продакшена:

   ```bash
   npm run build
   ```

   Результат в папке `dist/`.

## Вход в систему

- **Администратор** (создаётся при первом запуске бэкенда): логин `admin`, пароль `Admin123!`
- **Обычный пользователь**: зарегистрируйтесь через «Регистрация», затем перейдите по ссылке активации из консоли бэкенда и войдите с выбранным логином и паролем.

## Структура проекта

```
frontend/
├── public/
├── src/
│   ├── api/              # Запросы к API (auth, animalTypes, breeds, animals, weightings, users)
│   ├── components/
│   │   ├── common/       # ProtectedRoute, AdminRoute
│   │   └── layout/       # Navbar, Layout
│   ├── context/          # AuthContext
│   ├── hooks/            # useAuth
│   ├── lib/              # queryClient
│   ├── pages/
│   │   ├── auth/         # Login, Register, Activate
│   │   ├── dashboard/
│   │   ├── animalTypes/  # Список и форма
│   │   ├── breeds/
│   │   ├── animals/
│   │   ├── weightings/
│   │   └── users/        # Только для Admin
│   ├── types/            # Типы и DTO
│   ├── App.tsx
│   ├── main.tsx
│   ├── router.tsx
│   └── index.css
├── .env                  # VITE_API_URL
├── index.html
├── package.json
├── vite.config.ts
└── tailwind (PostCSS)
```

## Скрипты

| Команда        | Описание                    |
|----------------|-----------------------------|
| `npm run dev`  | Режим разработки с HMR      |
| `npm run build` | Сборка в `dist/`          |
| `npm run preview` | Просмотр собранного билда |
| `npm run lint` | Проверка ESLint              |

## CORS

В режиме разработки бэкенд настроен на разрешение запросов с любого источника; отдельный прокси во фронтенде не нужен.
