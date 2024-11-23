-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=tarea_apiWebREST_user;Password=123456;Database=tarea_apiWebREST;"

DROP DATABASE IF EXISTS tarea_apiWebREST;
CREATE DATABASE tarea_apiWebREST;
USE tarea_apiWebREST;

DROP USER IF EXISTS tarea_apiWebREST_user;
CREATE USER tarea_apiWebREST_user IDENTIFIED BY '123456';

DROP TABLE IF EXISTS Usuarios;
CREATE TABLE  Usuarios (
	mail VARCHAR(50) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
	edad INTEGER NOT NULL
);

GRANT INSERT, SELECT, UPDATE, DELETE
ON tarea_apiWebREST.Usuarios
TO tarea_apiWebREST_user;