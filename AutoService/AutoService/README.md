# АвтоТранс — Система учёта заявок на ремонт автомобилей
### Версия с SQLite (база данных — файл внутри проекта)

## Стек технологий
- **Backend:** ASP.NET Core 8 MVC (C#)
- **СУБД:** SQLite (файл `autoservice.db` создаётся автоматически)
- **ORM:** Entity Framework Core
- **Frontend:** Bootstrap 5 + Bootstrap Icons

---

## Как запустить в Visual Studio 2022

### Шаг 1 — Создай проект
1. Visual Studio 2022 → **Создать новый проект**
2. Выбери **ASP.NET Core Web App (Model-View-Controller)** (C#)
3. Имя проекта: `AutoService`, Framework: **.NET 8.0**

### Шаг 2 — Замени файлы
1. Правой кнопкой на проект → **Открыть в проводнике**
2. Скопируй все папки и файлы из этого архива **с заменой**
3. Вернись в VS → **Перезагрузить всё**

### Шаг 3 — Установи NuGet-пакеты
Меню **Сервис → Диспетчер пакетов NuGet → Консоль диспетчера пакетов:**
```
Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 8.0.0
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.0
```

### Шаг 4 — Запусти
Нажми **F5** — база данных `autoservice.db` создастся автоматически в папке проекта и заполнится тестовыми данными. Никаких дополнительных настроек не нужно!

---

## Тестовые учётные записи

| Роль        | Логин   | Пароль |
|-------------|---------|--------|
| Менеджер    | login1  | pass1  |
| Автомеханик | login2  | pass2  |
| Автомеханик | login3  | pass3  |
| Оператор    | login4  | pass4  |
| Заказчик    | login12 | pass12 |
| Заказчик    | login13 | pass13 |

---

## Структура проекта
```
AutoService/
├── Controllers/
│   ├── AccountController.cs   # Авторизация
│   ├── UsersController.cs     # Управление пользователями (Менеджер)
│   ├── RequestsController.cs  # Заявки + комментарии
│   └── ImportController.cs    # Импорт из txt-файлов
├── Data/
│   ├── AppDbContext.cs        # EF Core контекст
│   └── DbSeeder.cs            # Автозаполнение тестовыми данными
├── Models/
│   ├── User.cs
│   ├── Request.cs
│   └── Comment.cs
├── Views/
│   ├── Account/Login.cshtml
│   ├── Users/
│   ├── Requests/
│   └── Import/
├── appsettings.json           # Строка подключения к SQLite
└── Program.cs
```

## Права по ролям

| Функция                        | Менеджер | Оператор | Автомеханик | Заказчик |
|-------------------------------|----------|----------|-------------|---------|
| Управление пользователями      | ✅       | ❌       | ❌          | ❌      |
| Все заявки                     | ✅       | ✅       | Свои        | Свои    |
| Создать заявку                 | ✅       | ✅       | ❌          | ✅      |
| Назначить механика             | ✅       | ✅       | ❌          | ❌      |
| Изменить статус заявки         | ✅       | ✅       | ✅          | ❌      |
| Добавить комментарий           | ✅       | ❌       | ✅          | ❌      |
| Импорт данных из файлов        | ✅       | ✅       | ❌          | ❌      |

## Стек технологий
- **Backend:** ASP.NET Core 8 MVC (C#)
- **СУБД:** MySQL
- **ORM:** Entity Framework Core (Pomelo.EntityFrameworkCore.MySql)
- **Frontend:** Bootstrap 5 + Bootstrap Icons

---

## Модуль 1 — Авторизация и управление пользователями
- Вход в систему по логину и паролю
- Ролевая модель: **Менеджер**, **Оператор**, **Автомеханик**, **Заказчик**
- Менеджер: CRUD пользователей (создание, редактирование, удаление)
- Разграниченный доступ к функционалу в зависимости от роли

## Модуль 2 — Управление заявками
- Создание заявок (Заказчик, Оператор, Менеджер)
- Просмотр списка с фильтрацией по статусу и поиском
- Просмотр карточки заявки с историей комментариев
- Редактирование заявки (права зависят от роли)
- Назначение механика на заявку (Оператор, Менеджер)
- Статусы: Новая заявка → В процессе ремонта → Ожидание автозапчастей → Готова к выдаче → Завершена
- Комментарии к заявке (Автомеханик, Менеджер)
- Импорт данных из txt-файлов (пользователи, заявки, комментарии)

---

## Установка и запуск

### 1. Требования
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MySQL Server 8.x

### 2. Настройка базы данных
```bash
# Войти в MySQL и выполнить скрипт
mysql -u root -p < init_db.sql
```

### 3. Настройка строки подключения
В файле `appsettings.json` укажите ваш пароль MySQL:
```json
"DefaultConnection": "Server=localhost;Database=autoservice_db;User=root;Password=ВАШ_ПАРОЛЬ;"
```

### 4. Запуск приложения
```bash
dotnet restore
dotnet run
```
Приложение запустится на `http://localhost:5000`

---

## Тестовые учётные записи

| Роль         | Логин    | Пароль  |
|--------------|----------|---------|
| Менеджер     | login1   | pass1   |
| Автомеханик  | login2   | pass2   |
| Автомеханик  | login3   | pass3   |
| Оператор     | login4   | pass4   |
| Оператор     | login5   | pass5   |
| Заказчик     | login11  | pass11  |
| Заказчик     | login12  | pass12  |
| Заказчик     | login13  | pass13  |

---

## Структура проекта
```
AutoService/
├── Controllers/
│   ├── AccountController.cs   # Авторизация
│   ├── UsersController.cs     # Управление пользователями
│   ├── RequestsController.cs  # Управление заявками + комментарии
│   └── ImportController.cs    # Импорт из txt
├── Models/
│   ├── User.cs
│   ├── Request.cs
│   └── Comment.cs
├── Data/
│   └── AppDbContext.cs        # EF Core контекст
├── Views/
│   ├── Account/Login.cshtml
│   ├── Users/ (Index, Create, Edit)
│   ├── Requests/ (Index, Details, Create, Edit)
│   └── Import/Index.cshtml
├── init_db.sql                # SQL-скрипт инициализации БД
├── appsettings.json
└── Program.cs
```
