-- GET /api/Patients
SELECT p."Id", p."Address", p."BirthDate", p."Name", p."PhoneNumber"
FROM "Patients" AS p

--POST /api/Patients
INSERT INTO "Patients" ("Id", "Address", "BirthDate", "Name", "PhoneNumber")
VALUES (@p0, @p1, @p2, @p3, @p4);

--GET /api/Patients/{id}
SELECT p."Id", p."Address", p."BirthDate", p."Name", p."PhoneNumber"
FROM "Patients" AS p
WHERE p."Id" = @id
LIMIT 1

--PUT /api/Patients/{id}
SELECT p."Id", p."Address", p."BirthDate", p."Name", p."PhoneNumber"
FROM "Patients" AS p
WHERE p."Id" = @id
LIMIT 1

UPDATE "Patients" SET "Address" = @p0, "BirthDate" = @p1, "Name" = @p2, "PhoneNumber" = @p3
WHERE "Id" = @p4;

--DELETE /api/Patients/{id}
 SELECT p."Id", p."Address", p."BirthDate", p."Name", p."PhoneNumber"
FROM "Patients" AS p
WHERE p."Id" = @id
LIMIT 1

DELETE FROM "Patients"
WHERE "Id" = @p0;


-- Q1: GET /api/patients возвращает всю таблицу (нет WHERE),
-- а GET /api/patients/{id} добавляет WHERE "Id" = @id и LIMIT 1,
-- то есть ищет одну запись по ключу.

-- Q2: id при POST база не генерирует: колонка "Id" есть в INSERT (@p0),
-- а RETURNING отсутствует. Id задаётся в приложении (например, Guid)
-- до вставки, поэтому приложение уже знает его.

-- Q3: Вместо значений в логе '?', потому что EF Core скрывает параметры
-- с персональными данными пациентов. Локально это можно отключить через
-- .EnableSensitiveDataLogging().

