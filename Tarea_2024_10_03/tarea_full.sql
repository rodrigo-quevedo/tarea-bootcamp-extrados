CREATE DATABASE IF NOT EXISTS tarea_vistas;

USE tarea_vistas;

DROP TABLE IF EXISTS ventas;
DROP TABLE IF EXISTS productos;
DROP TABLE IF EXISTS estacionamiento;
DROP TABLE IF EXISTS vehiculos;
DROP TABLE IF EXISTS empleados;
DROP TABLE IF EXISTS personas;


CREATE TABLE personas (
	dni INTEGER PRIMARY KEY,
	nombre VARCHAR(20) NOT NULL,
	apellido VARCHAR(20) NOT NULL,
	email VARCHAR(30) NOT NULL
);


CREATE TABLE empleados (
	id_empleado INTEGER PRIMARY KEY,
	dni INTEGER NOT NULL,
		FOREIGN KEY(dni) REFERENCES personas(dni),
	sueldo FLOAT NOT NULL,
	rol VARCHAR(10) NOT NULL,
		CHECK (rol IN ("vendedor","manager")),
	-- Formato de fecha_ingreso es "AAAA-MM-DD"
	fecha_ingreso VARCHAR(10) NOT NULL,
		CHECK(LENGTH(fecha_ingreso) = 10),
	-- Formato de fecha_egreso es "AAAA-MM-DD"
	fecha_egreso VARCHAR(10) NULL,
		CHECK(LENGTH(fecha_egreso) = 10),
);


CREATE TABLE vehiculos (
	patente VARCHAR(7) PRIMARY KEY,
		CHECK(LENGTH(patente) = 7),
	modelo VARCHAR(20) NOT NULL,
	color VARCHAR(15) NOT NULL,
	id_empleado INTEGER NOT NULL,
		FOREIGN KEY(id_empleado) REFERENCES empleados(id_empleado)
);


CREATE TABLE estacionamiento (
	patente VARCHAR(7) NOT NULL,
		FOREIGN KEY(patente) REFERENCES vehiculos(patente),
	fecha_ingreso VARCHAR(11) NOT NULL,
	lote INTEGER NOT NULL,
	PRIMARY KEY(patente, fecha_ingreso, lote)
);


CREATE TABLE productos (
	codigo_barra VARCHAR(8) PRIMARY KEY,
		CHECK(LENGTH(codigo_barra) = 8),
	descripcion VARCHAR(50) NULL,
	precio FLOAT NOT NULL
);

CREATE TABLE ventas (
	dni INTEGER NOT NULL,
		FOREIGN KEY(dni) REFERENCES personas(dni),
	id_empleado INTEGER NOT NULL,
		FOREIGN KEY(id_empleado) REFERENCES empleados(id_empleado),
	codigo_barra VARCHAR(8) NOT NULL,
		FOREIGN KEY(codigo_barra) REFERENCES productos(codigo_barra),
	-- Formato de fecha_hora es "AAAA-MM-DD HH:MM"
	fecha_hora VARCHAR(16) NOT NULL,
		CHECK(LENGTH(fecha_hora) = 16)
);