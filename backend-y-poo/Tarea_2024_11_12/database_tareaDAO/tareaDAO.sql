-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;"

DROP DATABASE IF EXISTS tarea_dao;
CREATE DATABASE tarea_dao;
USE tarea_dao;

DROP USER IF EXISTS tareaDAO_user;
CREATE USER tareaDAO_user IDENTIFIED BY '123456';

DROP TABLE IF EXISTS Usuarios;
CREATE TABLE  Usuarios (
	Id INTEGER PRIMARY KEY,
	Nombre VARCHAR(50) NOT NULL,
	Edad INTEGER NOT NULL,
	-- Formato de Fecha_baja: "YYYY-MM-DD"
	Fecha_baja VARCHAR(10) NULL,
	CHECK(LENGTH(Fecha_baja) = 10 OR Fecha_baja IS NULL),
	CHECK(fecha_baja REGEXP '^[0-9]{4}-[0-9]{2}-[0-9]{2}$' OR fecha_baja IS NULL)
);

GRANT INSERT, SELECT, UPDATE, DELETE
ON tarea_dao.Usuarios
TO tareaDAO_user;