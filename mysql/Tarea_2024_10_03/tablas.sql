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
	id_empleado INTEGER PRIMARY KEY AUTO_INCREMENT,
	dni INTEGER NOT NULL,
		FOREIGN KEY(dni) REFERENCES personas(dni),
	sueldo FLOAT NOT NULL,
	rol VARCHAR(10) NOT NULL,
		CHECK (rol IN ("vendedor","manager")),
	-- Formato de fecha_ingreso es "AAAA-MM-DD"
	fecha_ingreso VARCHAR(10) NOT NULL,
		CHECK(LENGTH(fecha_ingreso) = 10),
		CHECK(fecha_ingreso REGEXP '^20([01][0-9]|2[0-4])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$'),
	-- Formato de fecha_egreso es "AAAA-MM-DD"
	fecha_egreso VARCHAR(10) NULL,
		CHECK(fecha_egreso = NULL OR LENGTH(fecha_egreso) = 10),
		CHECK(fecha_egreso = NULL OR fecha_egreso REGEXP '^20([01][0-9]|2[0-4])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$')
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
		CHECK(fecha_ingreso REGEXP '^20([01]\d|2[0-4])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$'),
	lote INTEGER NOT NULL,
	PRIMARY KEY(patente, fecha_ingreso, lote)
);


CREATE TABLE productos (
   -- Al no haber cantidad, el código de barra representa la unidad de producto (el item), no el producto.
   -- ej. 2 paquetes de fideos marca "M" de 1kg, son idénticos porque el producto es el mismo, sin embargo tendrán código de barra distinto.
   --   ->En cambio, si hubiera cantidad, se puede usar el mismo código de barra para todos los items "fideos marca M de 1kg".
	codigo_barra VARCHAR(8) PRIMARY KEY,
		CHECK(LENGTH(codigo_barra) = 8),
		CHECK(codigo_barra REGEXP '^[0-9]{8}$'),
	descripcion VARCHAR(50) NOT NULL,
	precio FLOAT NOT NULL
);

CREATE TABLE ventas (
	-- El código de barra representa la unidad del producto, por lo tanto un item (que lleva asociado un codigo_barra único) solo se puede vender 1 sola vez.
	-- Por eso codigo_barra también puede ser la PK de ventas.
	codigo_barra VARCHAR(8) NOT NULL PRIMARY KEY,
		FOREIGN KEY(codigo_barra) REFERENCES productos(codigo_barra),
	dni INTEGER NOT NULL,
		FOREIGN KEY(dni) REFERENCES personas(dni),
	id_empleado INTEGER NOT NULL,
		FOREIGN KEY(id_empleado) REFERENCES empleados(id_empleado),
	-- Formato de fecha_hora es "AAAA-MM-DD HH:MM"
	fecha_hora VARCHAR(16) NOT NULL,
		CHECK(LENGTH(fecha_hora) = 16),
		CHECK(fecha_hora REGEXP '^20([01][0-9]|2[0-4])-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01]) ([01][0-9]|2[0-3]):([0-5][0-9])$')
);
