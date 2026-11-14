CREATE TABLE patients (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    birth_date DATE NOT NULL,
    phone VARCHAR(20)
);

CREATE TABLE spravka_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price NUMERIC(10, 2) NOT NULL CHECK (price > 0),
    valid_days INT NOT NULL
);


CREATE TABLE patients (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    birth_date DATE NOT NULL,
    phone VARCHAR(20)
);

CREATE TABLE spravka_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price NUMERIC(10, 2) NOT NULL CHECK (price > 0),
    valid_days INT NOT NULL
);

CREATE TABLE spravki (
    id SERIAL PRIMARY KEY,
    patient_id INT NOT NULL REFERENCES patients(id),
    spravka_type_id INT NOT NULL REFERENCES spravka_types(id),
    price NUMERIC(10, 2) NOT NULL,
    issued_at DATE NOT NULL DEFAULT CURRENT_DATE
);

INSERT INTO patients (name, birth_date, phone) VALUES
    ('Айбек Асанов', '1998-03-12', '0555123456'),
    ('Нургуль Токтосунова', '2003-07-25', '0700987654'),
    ('Бакыт Жумабаев', '1985-11-02', NULL),
    ('Айпери Садыкова', '2001-01-30', '0772345678'),
    ('Тимур Ким', '2006-09-14', '0509111222');

INSERT INTO spravka_types (name, price, valid_days) VALUES
    ('Справка 083', 100, 365),
    ('Санитарная книжка', 750, 365),
    ('Справка 086', 300, 180),
    ('Справка в бассейн', 200, 90);

INSERT INTO spravki (patient_id, spravka_type_id, price, issued_at) VALUES
    (1, 1, 100, '2026-10-01'),
    (1, 4, 200, '2026-10-15'),
    (2, 2, 750, '2026-10-03'),
    (2, 3, 300, '2026-11-02'),
    (4, 3, 300, '2026-11-05'),
    (5, 4, 200, '2026-11-06'),
    (5, 1, 100, '2026-11-10');

INSERT INTO patients (name, birth_date, phone) VALUES
    ('Азамат Усенов', '1995-05-15', '0555987654'),
    ('Эльнура Камчыбекова', '2000-12-20', '0777123456'),
    ('Руслан Исаев', '1988-08-08', NULL);

INSERT INTO spravka_types (name, price, valid_days) VALUES
	('Справка в Военкомат', 1000, 30)

INSERT INTO spravki (patient_id, spravka_type_id, price, issued_at) VALUES
	(6, 5, 450, '2026-11-12'),
    (7, 1, 100, '2026-11-13'),
    (8, 3, 300, '2026-11-14'),
    (6, 4, 200, '2026-11-14'),
    (7, 5, 450, '2026-11-15');

SELECT spravki.id, patients.name AS patient, spravka_types.name AS spravka, spravki.price, spravki.issued_at
FROM spravki
JOIN patients ON spravki.patient_id = patients.id
JOIN spravka_types ON spravki.spravka_type_id = spravka_types.id
ORDER BY spravki.issued_at;

SELECT COUNT(*) AS spravki_count, SUM(price) AS total FROM spravki;

SELECT name from patients
order by patients.name

select name, birth_date from patients
where birth_date > '2000-01-01';

select name, phone from patients
where phone is NULL

SELECT * FROM patients 
WHERE name ILIKE '%ай%';

SELECT spravki.id, patients.name AS patient, spravka_types.name AS spravka, spravki.price, spravki.issued_at
FROM spravki
JOIN patients ON spravki.patient_id = patients.id
JOIN spravka_types ON spravki.spravka_type_id = spravka_types.id
where patients.id = 1

select * from spravka_types

INSERT INTO spravki (patient_id, spravka_type_id, price) VALUES (999, 1, 100);

INSERT INTO spravka_types (name, price, valid_days) VALUES ('Справка в лагерь', -500, 30);

INSERT INTO patients (birth_date) VALUES ('2000-01-01');

DELETE FROM patients WHERE id = 1;

INSERT INTO spravki (patient_id, spravka_type_id, price) VALUES (3, 2, 750);
SELECT * FROM spravki WHERE patient_id = 3;
