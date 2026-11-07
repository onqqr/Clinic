## Services API

Справочник специализаций и услуг клиники (consultations / diagnostics / analyses).

## структура

```
Services/Services/
  Services.sln
  Dockerfile
  src/
    Services.API/
    Services.Application/
    Services.Domain/
    Services.Infrastructure/
```

## DI

`DependencyInjection.cs` → `AddServicesModule`.  
`Program.cs` только pipeline + вызов DI.

## эндпоинты

| Метод | Путь | Назначение |
|---|---|---|
| GET | `/api/catalog` | страница услуг для пациента (3 вкладки, только Active) |
| GET | `/api/specializations` | список специализаций |
| GET | `/api/specializations/{id}` | детали + услуги |
| POST | `/api/specializations` | создать (имя + ≥1 услуга) |
| PUT | `/api/specializations/{id}` | обновить имя/статус |
| PATCH | `/api/specializations/{id}/status` | смена статуса (Inactive → cascade + `SpecializationDeactivated`; Active → услуги остаются Inactive и включаются вручную) |
| POST | `/api/specializations/{id}/services` | добавить услугу |
| GET | `/api/services/{id}` | детали услуги |
| PUT | `/api/services/{id}` | обновить услугу |
| PATCH | `/api/services/{id}/status` | смена статуса услуги (Active нельзя, если специализация Inactive → 409) |

## статусы

- Деактивация специализации каскадно переводит её услуги в `Inactive` и публикует `SpecializationDeactivated`.
- Активация специализации услуги **не** включает — их нужно активировать по одной.
- Услугу нельзя создать/активировать как `Active`, пока специализация `Inactive` (409).

## ран

```bash
dotnet run --project Services/Services/src/Services.API/Services.API.csproj --launch-profile http
```

http://localhost:5080/swagger
