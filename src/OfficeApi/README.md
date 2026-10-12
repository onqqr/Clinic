## Шорт суммари

Веб-приложение для пациентов клиники: регистрация и личный кабинет, 
запись на приём к врачу или на услугу, просмотр медицинской истории 
и дополнительных возможностей, связанных с основными сценариями. 
Проект реализован как набор микросервисов на ASP.NET Core, 
общающихся между собой асинхронно через RabbitMQ.


## Доп Инфа:

__DdContext__ - объект EF Core, через который C# работает с базой данных.
EF Core > переводит LINQ/C# в SQL.
типа, _dbContext.Offices - дай доступ к таблице Offices.
DTO - объект, который используется для передачи данных между слоями/API.

## Отдельно:

Services:
1. RabiitMQ
2. MinIO

## Архитектура для каждого микросервиса:

Program.cs > DI > Controllers > Services > Repositories > EF Core > Database


                    ┌─────────────────────┐
                    │    OfficeApi        │
                    │  ASP.NET / HTTP      │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │  OfficeApi.Application│
                    │ Services / DTO /     │
                    │ interfaces           │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │   OfficeApi.Domain   │
                    │ Entities / Enums /   │
                    │ business rules       │
                    └─────────────────────┘
                               ▲
                               │
                    ┌──────────┴──────────┐
                    │ OfficeApi.Infrastructure│
                    │ EF / PostgreSQL /   │
                    │ Repository / S3 etc.│
                    └─────────────────────┘
## Nuget пакеты:

Microsoft.EntityFrameworkCore - сам EF Core<br>
Npgsql.EntityFrameworkCore.PostgreSQL - провайдер PostgreSQL<br>
Microsoft.EntityFrameworkCore.Design - для миграции


## Путь для данных:

Клиент > Сервер: 
`GET /api/offices/{id} > Controller > OfficeService > IOfficeRepository > OfficeRepository > OfficeDbContext > PostgreSQL`<br>
Сервер > Клиент:
`PostgreSQL > Office > OfficeService > OfficeResponse > JSON > Frontend`<br>
Если офиса нет:
`Database > null > Service > null > 404`

## Ответсвтенность

Repository - ищет данные.<br>
Service - выполняет сценарий.<br>
Controller - занимается HTTP.