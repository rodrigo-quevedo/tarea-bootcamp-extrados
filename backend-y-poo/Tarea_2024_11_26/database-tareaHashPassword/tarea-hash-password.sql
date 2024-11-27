-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=tarea_hashPassword_user;Password=123456;Database=tarea_hashPassword;"

DROP DATABASE IF EXISTS tarea_hashPassword;
CREATE DATABASE tarea_hashPassword;
USE tarea_hashPassword;

DROP USER IF EXISTS tarea_hashPassword_user;
CREATE USER tarea_hashPassword_user IDENTIFIED BY '123456';

DROP TABLE IF EXISTS Usuarios;
CREATE TABLE  Usuarios (
	mail VARCHAR(50) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
	edad INTEGER NOT NULL,
	
	username VARCHAR(50) NOT NULL UNIQUE,
	password VARCHAR(50) NOT NULL
);

GRANT INSERT, SELECT, UPDATE, DELETE
ON tarea_hashPassword.Usuarios
TO tarea_hashPassword_user;