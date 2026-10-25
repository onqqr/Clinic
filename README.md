# ElPixel Clinic

микросервисы клиники на ASP.NET Core. 
Каждый сервис - отдельный solution и своя БД.

## сервисы

| Сервис | Solution | HTTP | Swagger |
|---|---|---|---|
| Office API | `Services/Office/Office.sln` | http://localhost:5059 | `/swagger` |
| Profiles API | `Services/Profile/Profile.sln` | http://localhost:5286 | `/swagger` |
| Services API | `Services/Services/Services.sln` | http://localhost:5080 | `/swagger` |

Инфраструктура в Docker: PostgreSQL (отдельная БД на каждый сервис).

## ран

```bash
git clone <repo-url>
cd Clinic
docker compose up --build
```

Проверка:

- Office: http://localhost:5059/swagger  
- Profiles: http://localhost:5286/swagger  
- Services: http://localhost:5080/swagger  

Остановка: `docker compose down`  
Стереть данные БД: `docker compose down -v`

## docker

- **`docker-compose.yml`** - в корне весь стек одной командой.
- **`Services/<Name>/Dockerfile`** - multi-stage сборка одного микросервиса.
- Connection strings и порты - через environment в compose.
- При старте API сами применяют EF-миграции.