## шорт суммари

Микросервис офисов клиники.
CRUD по офисам + смена статуса.
ASP.NET Core + EF Core + PostgreSQL.

## структура

```
Services/Office/
  src/
    Clinic.Offices.API/                 <- HTTP слой (вход в приложение)
    Clinic.Offices.Application/     <- сценарии, DTO, валидация
    Clinic.Offices.Domain/          <- сущности и enums, без зависимостей
    Clinic.Offices.Infrastructure/  <- БД, EF, репозитории
  tests/
    Clinic.Offices.Tests/           <- юнит-тесты
```

**что за что отвечате** > каждый слой делает своё. Domain ничего не знает про HTTP/БД.
Application описывает что делать. Infrastructure — как достать из Postgres.
Api только принимает запрос и отдаёт ответ.

## суммари файлов

__Clinic.Offices.API__ — Controllers, Program.cs, DI.
Принимает HTTP, валидирует через FluentValidation, зовёт сервис, отдаёт JSON/статус-коды.

__Application__ — бизнес-сценарии.
- `Services/` — Create / Update / ChangeStatus / Delete и т.д.
- `DTOs/` — что приходит/уходит по API (с суффиксом Dto)
- `Interfaces/` — контракты сервиса и репозитория
- `Validators/` — правила валидации запросов

__Domain__ — ядро.
- `Entities/` — Office (город, улица, статус...)
- `Enums/` — OfficeStatus (Active / Inactive)
Без EF, без ASP.NET. Чистые модели.

__Infrastructure__ — техника.
- `Data/` — OfficeDbContext (доступ к таблицам)
- `Repositories/` — реализация IOfficeRepository
- `Migrations/` — схема БД

__tests__ — проверяем сервис и домен без поднятия всего API.

## сборка

`Program.cs` поднимает DI:
Application (сервисы + валидаторы) + Infrastructure (DbContext + репозиторий).

Цепочка:

```
HTTP > Controller > IOfficeService > IOfficeRepository > DbContext > PostgreSQL
```

клиент-сервер:
`GET /api/offices/{id} > OfficesController > OfficeService > OfficeRepository > OfficeDbContext > PostgreSQL`

сервер-клиент:
`PostgreSQL > Office > OfficeService > OfficeResponseDto > JSON`

Нет офиса: `null > 404`

## ответственность

Controller — HTTP (роуты, 200/400/404).<br>
Service — сценарий (собрать сущность, поменять статус, замапить в DTO).<br>
Repository — достать/сохранить/удалить в БД.<br>
DbContext — EF Core мост к PostgreSQL.<br>
DTO — контракт API, не тащим entity наружу.<br>
Validator — проверяет тело запроса до сервиса.

## Nuget

Microsoft.EntityFrameworkCore — EF Core<br>
Npgsql.EntityFrameworkCore.PostgreSQL — Postgres<br>
Microsoft.EntityFrameworkCore.Design — миграции<br>
FluentValidation — валидация DTO

## ран

```
dotnet run --project Services/Office/src/Clinic.Offices.API/Clinic.Offices.API.csproj --launch-profile http
```

http://localhost:5059

## доп инфа

Позже: RabbitMQ (события между сервисами), MinIO (фото офисов).
