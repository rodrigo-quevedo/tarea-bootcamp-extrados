-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=tarea_datetime_user;Password=123456;Database=tarea_datetime;"

DROP DATABASE IF EXISTS tarea_datetime;
CREATE DATABASE tarea_datetime;
USE tarea_datetime;

DROP USER IF EXISTS tarea_datetime_user;
CREATE USER tarea_datetime_user IDENTIFIED BY '123456';
	
DROP TABLE IF EXISTS Usuarios;
CREATE TABLE  Usuarios (
	mail VARCHAR(50) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
	edad INTEGER NOT NULL,
	
	username VARCHAR(50) NOT NULL UNIQUE,
	password VARCHAR(255) NOT NULL,
	
	role VARCHAR(20) NOT NULL,
	CHECK (role IN ("usuario", "admin"))
);

DROP TABLE IF EXISTS Libros;
CREATE TABLE Libros (
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
	titulo VARCHAR(100) NOT NULL,
	-- los DATETIME se deben insertar en UTC para que sean iguales
	fechaHora_prestamo DATETIME NULL,
	fechaHora_vencimiento DATETIME NULL,
	username_prestatario VARCHAR(50) NULL,
	-- Si hay fecha de prestamo, debe haber fecha vencimiento y username_prestatario al que se le presto.
	-- Si no hay fecha prestamo, el libro esta disponible, por lo tanto tampoco tiene fecha de vencimiento
	CHECK (
		fechaHora_prestamo IS NULL AND fechaHora_vencimiento IS NULL AND username_prestatario IS NULL
		OR
		fechaHora_prestamo IS NOT NULL AND fechaHora_vencimiento IS NOT NULL AND username_prestatario IS NOT NULL
	)
);

GRANT INSERT, SELECT, UPDATE, DELETE
ON tarea_datetime.Usuarios
TO tarea_datetime_user;

GRANT INSERT, SELECT, UPDATE, DELETE
ON tarea_datetime.Libros
TO tarea_datetime_user;


-- libros (para esta tarea solo se pide prestar libros, no registrarlos)
INSERT INTO libros (titulo) VALUES
	("Robin Hood"),
	("Don Quijote de la Mancha"),
	("Cuentos de la selva"),
	("Diccionario español inglés"),
	("Algebra I"),
	("Arquitectura del computador")
;