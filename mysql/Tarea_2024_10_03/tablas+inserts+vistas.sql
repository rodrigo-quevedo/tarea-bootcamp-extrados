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


DELETE FROM ventas;
DELETE FROM productos;
DELETE FROM estacionamiento;
DELETE FROM vehiculos;
DELETE FROM empleados;
DELETE FROM personas;

-- personas
INSERT INTO personas VALUES 
	(12332100, 'juan', 'rodriguez', 'juanrodriguez@mail.com'),
	(43212345, 'pedro', 'rodriguez', 'pedrorodriguez@mail.com'),
	(38290310, 'laura', 'gonzalez', 'lauragonzalez@mail.com'),
	(26305890, 'lucía', 'martinez', 'luciamartinez@mail.com'),
	(40048589, 'juan', 'garcía', 'juangarcia@mail.com'),
	(14423898, 'marta', 'lopez', 'martalopez@mail.com'),
	(39973920, 'felipe', 'gomez', 'felipegomez@mail.com'),
	(16666666, 'alonso', 'quijano', 'alonsoquijano@mail.com'),
	(15920939, 'pepe', 'argento', 'pepeargento@mail.com'),
	(41581851, 'maría', 'lópez', 'marialopez@mail.com'),
	(43908304, 'paula', 'díaz', 'pauladiaz@gmail.com'),
	(22930394, 'santiago', 'sanchez', 'santiagosanchez@mail.com'),
	(31939029, 'mario', 'castro', 'mariocastro@mail.com'),
	(36789111, 'alma', 'sosa', 'almasosa@mail.com')
;


-- empleados
INSERT INTO empleados(dni, sueldo, rol, fecha_ingreso, fecha_egreso) VALUES 
	(38290310, 500000, 'vendedor', '2018-02-15', NULL),
	(39973920, 500000, 'vendedor', '2018-06-21', NULL),
	(40048589, 480000, 'vendedor', '2018-06-21', '2018-09-21'),
	(15920939, 480000, 'vendedor', '2020-11-20', NULL),
	(26305890, 550000, 'manager', '2021-03-29', NULL)
;

-- SELECT * FROM empleados RIGHT JOIN personas ON empleados.dni = personas.dni;


-- vehiculos
INSERT INTO vehiculos VALUES
	('AB523FX', 'chevrolet corsa', 'blanco', 1),
	('AT981KL', 'volkswagen gol', 'gris', 2),
	('BS238WA', 'fiat palio', 'rojo', 1),
	('AB125AE', 'chevrolet chevy', 'verde', 4),
	('CA045OR', 'citroen c3', 'blanco', 5)
;
	
-- SELECT v.patente, v.modelo, v.color, v.id_empleado, 
-- datos_personales_empleados.dni, datos_personales_empleados.nombre, datos_personales_empleados.apellido, datos_personales_empleados.email,
-- datos_personales_empleados.rol, datos_personales_empleados.sueldo, datos_personales_empleados.fecha_ingreso, datos_personales_empleados.fecha_egreso
-- FROM vehiculos v 
-- JOIN 
-- 	(SELECT e.id_empleado, e.rol, e.sueldo, e.fecha_ingreso, e.fecha_egreso, p.dni, p.nombre, p.apellido, p.email
-- 	FROM empleados e JOIN personas p ON e.dni = p.dni
-- 	) AS datos_personales_empleados 
-- 	ON datos_personales_empleados.id_empleado = v.id_empleado
-- ;

-- estacionamiento
INSERT INTO estacionamiento VALUES
	('BS238WA', '2023-10-10', 1),
	('CA045OR', '2023-10-10', 2),
	('AT981KL', '2023-10-10', 4),
	('AB125AE', '2023-10-10', 5),
	
	('BS238WA', '2024-10-01', 1),
	('CA045OR', '2024-10-01', 2),
	('AT981KL', '2024-10-01', 4),
	('AB125AE', '2024-10-01', 5),
	
	('CA045OR', '2024-10-02', 1),
	('BS238WA', '2024-10-02', 2),
	('AB125AE', '2024-10-02', 4),
	('AT981KL', '2024-10-02', 5),
	
	('CA045OR', '2024-10-03', 1),
	('BS238WA', '2024-10-03', 2),
	('AT981KL', '2024-10-03', 4),
	
	('BS238WA', '2024-10-04', 1),
	('CA045OR', '2024-10-04', 2),
	('AT981KL', '2024-10-04', 4),
	('AB125AE', '2024-10-04', 5)
;

-- SELECT v.patente, est.fecha_ingreso AS fecha_estacionamiento, est.lote, e.id_empleado, p.nombre, p.apellido FROM estacionamiento est
-- RIGHT JOIN vehiculos v ON est.patente = v.patente
-- JOIN empleados e ON v.id_empleado = e.id_empleado
-- JOIN personas p ON e.dni = p.dni
-- ORDER BY fecha_estacionamiento, est.lote ASC
-- ;



-- productos
INSERT INTO productos VALUES 
	('00000023', "remera azul talle S", 13999.9),
	('00000024', "remera azul talle M", 13999.9),
	('00000025', "remera azul talle L", 13999.9),	

	('00000026', "remera roja talle S", 13999.9),
	('00000027', "remera roja talle M", 13999.9),
	('00000028', "remera roja talle L", 13999.9),	

	('00000029', "remera negra talle S", 14999.9),
	('00000030', "remera negra talle M", 14999.9),
	('00000031', "remera negra talle L", 14999.9),	
	
	('00000300', "camisa vestir celeste talle S", 21599.9),
	('00000301', "camisa vestir celeste talle M", 21599.9),
	('00000302', "camisa vestir celeste talle L", 21599.9),

	('00000303', "camisa vestir blanca talle S", 23599.9),
	('00000304', "camisa vestir blanca talle M", 23599.9),
	('00000305', "camisa vestir blanca talle L", 23599.9),

	('00004329', "buzo azul talle S", 30299.30),
	('00004330', "buzo azul talle M", 30299.30),
	('00004331', "buzo azul talle L", 30299.30),
	
	('00004332', "buzo negro talle S", 30299.30),
	('00004333', "buzo negro talle M", 30299.30),
	('00004334', "buzo negro talle L", 30299.30)
;

-- SELECT * FROM productos;


-- ventas
INSERT INTO ventas VALUES 
	('00000024',31939029, 1, "2024-10-01 11:06"),
	('00004333',31939029, 1, "2024-10-01 11:06"),
	('00000300',14423898, 2, "2024-10-01 12:18"),	
	('00000026',36789111, 1, "2024-10-01 14:53")
	
;

-- SELECT * FROM ventas v JOIN empleados e ON v.id_empleado=e.id_empleado
-- JOIN productos p ON p.codigo_barra = v.codigo_barra;


-- Generar un usuario nuevo para la BD que solo tenga acceso a las siguientes vistas:
-- 1) lista de empleados sin el sueldo
CREATE OR REPLACE VIEW vista_1
AS 
	SELECT 
		e.id_empleado AS id_empleado_from_view, 
		e.dni, 
		e.rol, 
		e.fecha_ingreso, 
		e.fecha_egreso 
	FROM empleados e
	WITH CHECK OPTION
;

SELECT * FROM vista_1;

-- 2) lista de los empelados vigentes (fecha_egreso = null) (sin el sueldo)
CREATE OR REPLACE VIEW vista_2
AS 
	SELECT * FROM vista_1 AS empleados_sin_sueldo 
	WHERE empleados_sin_sueldo.fecha_egreso IS NULL
	WITH CHECK OPTION
;

SELECT * FROM vista_2;

-- 3) lista de vehiculos con los datos del empleado al que pertence (sin el sueldo) (hacer join)
CREATE OR REPLACE VIEW vista_3
AS 
	SELECT * FROM vehiculos
	JOIN vista_1 AS empleados_sin_sueldo
	ON vehiculos.id_empleado = empleados_sin_sueldo.id_empleado_from_view
	WITH CHECK OPTION
;

SELECT * FROM vista_3;

-- 4) lista de personas que no sean empleados
CREATE OR REPLACE VIEW vista_4
AS 
	SELECT * FROM personas 
	WHERE	personas.dni NOT IN (SELECT dni FROM empleados)
	WITH CHECK OPTION
;

SELECT * FROM vista_4;

-- 5) lista de todos los empleados que hayan venido a trabajar el 10 OCT 2023 (tomar aquellos que usaron estacionamiento como los que se presentaron a trabajar)
CREATE OR REPLACE VIEW vista_5
AS 
	SELECT id_empleado FROM vehiculos v 
	WHERE v.patente IN (
			SELECT e.patente FROM estacionamiento e WHERE e.fecha_ingreso = '2023-10-10'
	)
	WITH CHECK OPTION
;


SELECT * FROM vista_5;

-- 6) lista de todos los productos comprados por la persona cuyo dni = 36789111
CREATE OR REPLACE VIEW vista_6
AS 
	SELECT * FROM productos p
	WHERE p.codigo_barra IN (
			SELECT v.codigo_barra FROM ventas v	WHERE v.dni = 36789111
	)
	WITH CHECK OPTION
;


SELECT * FROM vista_6;

-- 7) cantidad total de ventas en monto(plata), generada por el vendedor id = 2
CREATE OR REPLACE VIEW vista_7
AS
	SELECT SUM(p.precio) FROM productos p
	WHERE 
		p.codigo_barra IN (
			SELECT v.codigo_barra FROM ventas v WHERE v.id_empleado = 2
		)
;

SELECT * FROM vista_7;


-- Creación del usuario:
DROP USER IF EXISTS nuevo_usuario_vistas;

CREATE USER IF NOT EXISTS nuevo_usuario_vistas
IDENTIFIED BY 'pass1234';

-- Dar permisos al usuario:
GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_1
TO nuevo_usuario_vistas;

GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_2
TO nuevo_usuario_vistas;

GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_3
TO nuevo_usuario_vistas;

GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_4
TO nuevo_usuario_vistas;

GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_5
TO nuevo_usuario_vistas;

GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_6
TO nuevo_usuario_vistas;

GRANT SELECT, INSERT, UPDATE, DELETE
ON tarea_vistas.vista_7
TO nuevo_usuario_vistas;
