## шорт суммари

Микросервис профилей клиники.
Врачи, пациенты, ресепшионисты + локальный snapshot офисов.
ASP.NET Core + EF Core + PostgreSQL.

## структура

```
Services/Profile/
  Profile.sln
  src/
    Profile.API/              <- HTTP, Program.cs, DI
    Profile.Application/      <- сценарии, DTO, валидация, mapping, events
    Profile.Domain/           <- entities, enums, specifications, matching
    Profile.Infrastructure/   <- БД, EF, репозитории, event publisher stub
  tests/                      <- (юнит-тесты — следующий шаг)
```

Domain ничего не знает про HTTP/БД.
Application — что делать. Infrastructure — как достать из Postgres / залогировать событие.
API принимает запрос и отдаёт ответ.

## суммари слоёв

**Profile.API** — Controllers, Program.cs, DependencyInjection.cs, middleware.
Роуты: `api/doctors`, `api/patients`, `api/receptionists`, `api/office-snapshots`.

**Application**
- `Services/` — Doctor / Patient / Receptionist / OfficeSnapshot
- `DTOs/` — контракты API
- `Validators/` — FluentValidation
- `Events/` — DoctorProfileCreated, ReceptionistProfileCreated, *Deleted
- `Mapping/` — AutoMapper (в т.ч. Experience)

**Domain**
- `Entities/` — Doctor, Patient, Receptionist, OfficeSnapshot
- `Enums/` — DoctorStatus
- `Matching/` — весовое сопоставление профилей пациента
- `Specifications/` — фильтры списков

**Infrastructure**
- `Data/` — ProfilesDbContext + миграции
- `Repositories/`
- `Messaging/` — LoggingEventPublisher (временный stub вместо брокера)

## DI

Вся регистрация в `DependencyInjection.cs` → `AddProfileServices`.
`Program.cs` только поднимает pipeline и вызывает этот метод.

## цепочка

```
HTTP > Controller > *Service > *Repository > ProfilesDbContext > PostgreSQL
```

События пока: Service → IEventPublisher → лог.

## основные эндпоинты

| Метод | Путь | Назначение |
|---|---|---|
| GET/POST | `/api/doctors` | список (фильтры) / создать |
| GET/PUT/DELETE | `/api/doctors/{id:guid}` | детали / обновить / удалить |
| PATCH | `/api/doctors/{id:guid}/status` | смена статуса |
| GET/POST | `/api/patients` | список / создать (админ) |
| POST | `/api/patients/register` | регистрация + matching |
| POST | `/api/patients/confirm-match` | подтвердить/отклонить match |
| GET/POST | `/api/receptionists` | список / создать |
| POST/GET | `/api/office-snapshots` | upsert snapshot (заглушка синхронизации офисов) |

Фильтр врачей «только At work»: query `onlyAvailable=true`.

## ран

```
dotnet run --project Services/Profile/src/Profile.API/Profile.API.csproj --launch-profile http
```

http://localhost:5286  
Swagger (Development): http://localhost:5286/swagger
