# AnimalFarm API

Веб-API на ASP.NET Core 8 для учёта животных на ферме: типы животных, породы, отдельные особи и взвешивания.

## Архитектура

Чистая слоистая архитектура из четырёх проектов:

- **AnimalFarm.API** — веб-API (контроллеры, middleware, настройка JWT)
- **AnimalFarm.Application** — бизнес-логика (сервисы, DTO, валидаторы FluentValidation)
- **AnimalFarm.Domain** — домен (сущности, перечисления, интерфейсы репозиториев)
- **AnimalFarm.Infrastructure** — доступ к данным (EF Core, PostgreSQL, Identity, репозитории, почта)

## Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (по умолчанию localhost:5432)
- EF Core CLI: `dotnet tool install --global dotnet-ef`

## Быстрый старт

1. **Настройте строку подключения** в `src/AnimalFarm.API/appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=AnimalFarmDb;Username=postgres;Password=postgres"
   }
   ```

2. **Создайте начальную миграцию** (если ещё не создана):

   ```bash
   dotnet ef migrations add InitialCreate --project src/AnimalFarm.Infrastructure --startup-project src/AnimalFarm.API --output-dir Data/Migrations
   ```

3. **Запустите приложение**:

   ```bash
   dotnet run --project src/AnimalFarm.API
   ```

   При первом запуске база мигрируется и заполняется начальными данными.

4. **Swagger UI**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Учётная запись администратора

При первом запуске автоматически создаётся администратор:

| Поле       | Значение                 |
|-----------|--------------------------|
| Логин     | `admin`                  |
| Email     | `admin@animalfarm.com`   |
| Пароль    | `Admin123!`              |

Параметры можно изменить в `appsettings.json` в секции `AdminUser`.

## Аутентификация

Используется JWT Bearer.

### Регистрация

```
POST /api/auth/register
{
  "userName": "ivan",
  "email": "ivan@example.com",
  "password": "Password1!"
}
```

Ссылка для активации выводится в консоль (режим без реальной отправки почты).

### Активация учётной записи

```
GET /api/auth/activate?userId={id}&token={token}
```

### Вход

```
POST /api/auth/login
{
  "userName": "admin",
  "password": "Admin123!"
}
```

В ответ приходит JWT. Его нужно передавать в заголовке запросов:

```
Authorization: Bearer <ваш-токен>
```

## Роли

| Роль   | Права |
|--------|--------|
| Admin  | Полный доступ ко всем ресурсам; просмотр пользователей, активация и деактивация учётных записей. |
| User   | CRUD по типам животных, породам и животным; взвешивания — только свои записи. |

## Эндпоинты API

### Аутентификация

| Метод | Путь                   | Доступ   |
|-------|------------------------|----------|
| POST  | `/api/auth/register`   | Гости    |
| POST  | `/api/auth/login`      | Гости    |
| GET   | `/api/auth/activate`  | Гости    |

### Пользователи (только Admin)

| Метод | Путь                             | Описание            |
|-------|-----------------------------------|---------------------|
| GET   | `/api/users`                     | Список пользователей |
| PUT   | `/api/users/{id}/activate`       | Активировать        |
| PUT   | `/api/users/{id}/deactivate`     | Деактивировать      |

### Типы животных

| Метод | Путь                       |
|-------|----------------------------|
| GET   | `/api/animaltypes`         |
| GET   | `/api/animaltypes/{id}`    |
| POST  | `/api/animaltypes`         |
| PUT   | `/api/animaltypes/{id}`    |
| DELETE| `/api/animaltypes/{id}`    |

### Породы

| Метод | Путь                                   |
|-------|----------------------------------------|
| GET   | `/api/breeds`                          |
| GET   | `/api/breeds/{id}`                     |
| GET   | `/api/breeds/by-animal-type/{typeId}`  |
| POST  | `/api/breeds`                          |
| PUT   | `/api/breeds/{id}`                     |
| DELETE| `/api/breeds/{id}`                     |

### Животные

| Метод | Путь                 |
|-------|----------------------|
| GET   | `/api/animals`       |
| GET   | `/api/animals/{id}`  |
| POST  | `/api/animals`       |
| PUT   | `/api/animals/{id}`  |
| DELETE| `/api/animals/{id}`  |

### Взвешивания

| Метод | Путь                     |
|-------|--------------------------|
| GET   | `/api/weightings`        |
| GET   | `/api/weightings/{id}`   |
| POST  | `/api/weightings`        |
| PUT   | `/api/weightings/{id}`   |
| DELETE| `/api/weightings/{id}`   |

## Миграции EF Core

```bash
# Добавить миграцию
dotnet ef migrations add <ИмяМиграции> --project src/AnimalFarm.Infrastructure --startup-project src/AnimalFarm.API --output-dir Data/Migrations

# Применить миграции к базе вручную
dotnet ef database update --project src/AnimalFarm.Infrastructure --startup-project src/AnimalFarm.API
```

## Структура решения

```
backend/
├── AnimalFarm.sln
├── README.md
└── src/
    ├── AnimalFarm.Domain/          # Домен
    │   ├── Entities/                # AnimalType, Breed, Animal, Weighting
    │   ├── Enums/                   # Gender
    │   └── Interfaces/              # Интерфейсы репозиториев
    ├── AnimalFarm.Application/      # Прикладной слой
    │   ├── DTOs/
    │   ├── Interfaces/
    │   ├── Services/
    │   └── Validators/              # FluentValidation
    ├── AnimalFarm.Infrastructure/   # Инфраструктура
    │   ├── Data/                    # DbContext, конфигурации, миграции, сидер
    │   ├── Identity/                # ApplicationUser
    │   ├── Repositories/
    │   └── Services/                # Auth, User, Email
    └── AnimalFarm.API/             # Точка входа
        ├── Controllers/
        ├── Middleware/
        └── Program.cs
```
