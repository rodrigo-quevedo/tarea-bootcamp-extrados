CREATE DATABASE IF NOT EXISTS administracion_hotel;
USE administracion_hotel;

DROP TABLE IF EXISTS estacionamiento_clientes;
DROP TABLE IF EXISTS limpieza_cuartos;
DROP TABLE IF EXISTS reservaciones_de_cuarto;
DROP TABLE IF EXISTS acciones_usuario;
DROP TABLE IF EXISTS estacionamiento;
DROP TABLE IF EXISTS cuartos;
DROP TABLE IF EXISTS clientes;
DROP TABLE IF EXISTS usuarios;


-- TABLAS BÁSICAS
CREATE TABLE usuarios(
	id_usuario INTEGER PRIMARY KEY AUTO_INCREMENT,

	dni INTEGER UNIQUE NOT NULL,
	nombre VARCHAR(50) NOT NULL,
	
	nombre_usuario VARCHAR(25) NOT NULL,
	contraseña VARCHAR(25) NOT NULL,
	rol VARCHAR(20) NOT NULL,
		CHECK(rol IN('administrador', 'recepcionista')),
	
	-- formato fecha_alta y fecha_baja "YYYY-MM-DD"
	fecha_alta VARCHAR(10) NOT NULL,
		CHECK(LENGTH(fecha_alta) = 10),
		CHECK(fecha_alta REGEXP '^[0-9]{4}-[0-9]{2}-[0-9]{2}$'),
	-- fecha_baja IS NULL indica que el usuario sigue vigente
	fecha_baja VARCHAR(10) NULL,
		CHECK(LENGTH(fecha_baja) = 10 OR fecha_baja IS NULL),
		CHECK(fecha_baja REGEXP '^[0-9]{4}-[0-9]{2}-[0-9]{2}$' OR fecha_baja IS NULL),
	id_creador INTEGER NULL,
		FOREIGN KEY (id_creador) REFERENCES usuarios(id_usuario),
	-- id_creador admitirá NULL solamente para el administrador	
		CHECK(
			(rol = 'administrador' AND id_creador IS NULL) 
			OR 
			(rol = 'recepcionista' AND id_creador IS NOT NULL)
		)
);
 

CREATE TABLE clientes (
	dni INTEGER PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
	
	-- formato fecha_alta "YYYY-MM-DD"
	fecha_alta VARCHAR(10) NOT NULL,
		CHECK(LENGTH(fecha_alta) = 10),
		CHECK(fecha_alta REGEXP '^[0-9]{4}-[0-9]{2}-[0-9]{2}$'),
	id_creador INTEGER NOT NULL,
		FOREIGN KEY(id_creador) REFERENCES usuarios(id_usuario)
);


CREATE TABLE cuartos (
	numero INTEGER PRIMARY KEY,
	disponible BOOLEAN NOT NULL
);


CREATE TABLE estacionamiento (
	numero_lote INTEGER PRIMARY KEY
);


-- CONSIGNAS

-- Registrar las entradas y salidas de clientes a los cuartos, con fecha-hora(string):
CREATE TABLE reservaciones_de_cuarto (
	id_reservacion INTEGER PRIMARY KEY AUTO_INCREMENT,
	
	dni_cliente INTEGER NOT NULL,
		FOREIGN KEY(dni_cliente) REFERENCES clientes(dni),
	numero_cuarto INTEGER NOT NULL,
		FOREIGN KEY(numero_cuarto) REFERENCES cuartos(numero),
		
	-- formato fecha_hora_entrada y fecha_hora_salida: "YYYY-MM-DD HH:MM"
	fecha_hora_entrada VARCHAR(16) NOT NULL,
		CHECK(LENGTH(fecha_hora_entrada) = 16),
		CHECK(fecha_hora_entrada REGEXP 
			'^[0-9]{4}-[0-9]{2}-[0-9]{2} ([0,1][0-9]|[2][0-3]):[0-5][0-9]$'
		),
	fecha_hora_salida VARCHAR(16) NULL,
		CHECK(LENGTH(fecha_hora_salida) = 16 OR fecha_hora_salida IS NULL),
		CHECK(fecha_hora_salida REGEXP 
				'^[0-9]{4}-[0-9]{2}-[0-9]{2} ([0,1][0-9]|[2][0-3]):[0-5][0-9]$'
			OR fecha_hora_salida IS NULL
		)
);


-- Registrar las acciones de los administradores y recepcionistas (que crean, que eliminan, etc.) (tabla de logs - id_usuario, mensaje):
CREATE TABLE acciones_usuario(
	id_accion INTEGER PRIMARY KEY AUTO_INCREMENT,
	id_usuario INTEGER NOT NULL,
		FOREIGN KEY(id_usuario) REFERENCES usuarios(id_usuario),
	mensaje VARCHAR(80) NOT NULL
);


-- Registrar los tiempos de limpieza de los cuartos (luego de cada salida de un cliente, se debe tener en cuenta un tiempo de limpieza de cuarto, pedido por el recepcionista):
CREATE TABLE limpieza_cuartos (
	id_limpieza INTEGER PRIMARY KEY AUTO_INCREMENT,
	-- En lugar del numero de cuarto, se referencia a la reservación en particular, para poder usar la fecha_salida de la tabla reservaciones_de_cuarto.
	id_reservacion INTEGER NOT NULL,
		FOREIGN KEY(id_reservacion) REFERENCES reservaciones_de_cuarto(id_reservacion),
	tiempo_limpieza_enMinutos INTEGER NOT NULL,
	id_solicitante INTEGER NOT NULL,
		FOREIGN KEY(id_solicitante) REFERENCES usuarios(id_usuario)
);


-- Qué estacionamiento utilizo X cliente cuando visito las instalaciones:
CREATE TABLE estacionamiento_clientes (
	id_estacionamiento_cliente INTEGER PRIMARY KEY AUTO_INCREMENT,
	-- Me parece más correcto NULL, ya que el estacionamiento es opcional, por ejemplo si el cliente fue en transporte público o taxi. NOT NULL sería correcto si todos los clientes usan estacionamiento.
	numero_lote INTEGER NULL,
		FOREIGN KEY(numero_lote) REFERENCES estacionamiento(numero_lote),
	dni_cliente INTEGER NOT NULL,
		FOREIGN KEY(dni_cliente) REFERENCES clientes(dni),
	id_reservacion INTEGER NOT NULL,
		FOREIGN KEY(id_reservacion) REFERENCES reservaciones_de_cuarto(id_reservacion)
);


DELETE FROM acciones_usuario;
DELETE FROM reservaciones_de_cuarto;
DELETE FROM estacionamiento;
DELETE FROM cuartos;
DELETE FROM clientes;
DELETE FROM usuarios;


INSERT INTO usuarios (dni, nombre, nombre_usuario, contraseña, rol, fecha_alta, fecha_baja, id_creador)
VALUES
	(1, 'usuario1', 'nombre_usuario1', 'pass123', 'administrador', '2020-10-20', NULL, NULL),
	(2, 'usuario2', 'nombre_usuario2', 'pass123456', 'recepcionista', '2020-10-20', NULL, 1)
;


INSERT INTO clientes VALUES
	(1111, 'cliente1', '2020-12-20', 2),
	(2222, 'cliente2', '2020-12-21', 2),
	(3333, 'cliente3', '2020-12-22', 2),
	(4444, 'cliente4', '2020-12-23', 2),
	(5555, 'cliente5', '2020-12-24', 2)
;

INSERT INTO cuartos VALUES
	(1, FALSE),
	(2, TRUE),	
	(3, FALSE),
	(4, TRUE),
	(5, TRUE),
	(6, TRUE),	
	(7, TRUE),
	(8, TRUE)
;

INSERT INTO estacionamiento VALUES
	(1),(2),(3),(4),(5),(6),(7),(8)
;

INSERT INTO reservaciones_de_cuarto (dni_cliente, numero_cuarto, fecha_hora_entrada, fecha_hora_salida) VALUES
	(1111, 1, '2020-12-20 10:15', '2020-12-21 10:30'),
	(2222, 1, '2020-12-21 13:30', NULL),
	(1111, 8, '2020-12-25 10:00', NULL),
	(3333, 3, '2020-12-23 18:00', NULL),
	(4444, 4, '2020-12-24 23:00', NULL),
	(5555, 3, '2021-01-01 18:00', '2021-01-01 22:00')
;

INSERT INTO acciones_usuario (id_usuario, mensaje) VALUES
	(1, 'Creó al usuario 2 con rol recepcionista'),
	(2, 'Registró al cliente dni: 1111 el día 2020-12-20'),
	(2, 'Registró al cliente dni: 2222 el día 2020-12-21'),
	(2, 'Registró al cliente dni: 3333 el día 2020-12-22'),
	(2, 'Registró al cliente dni: 4444 el día 2020-12-23'),
	(2, 'Registró al cliente dni: 5555 el día 2020-12-24')
;

INSERT INTO limpieza_cuartos (id_reservacion, tiempo_limpieza_enMinutos, id_solicitante) VALUES
	(1, 90, 2),
	(6, 60, 2)
;

INSERT INTO estacionamiento_clientes (numero_lote, dni_cliente, id_reservacion) VALUES
	(4, 1111, 1),
	(NULL, 2222, 2),
	(6, 1111, 3),
	(1, 3333, 4),
	(2, 4444, 5),
	(8, 5555, 6)
;

-- reservaciones vigentes + datos estacionamiento + datos cliente
SELECT * FROM reservaciones_de_cuarto r 
JOIN estacionamiento_clientes e ON r.id_reservacion = e.id_reservacion
JOIN clientes c ON r.dni_cliente = c.dni
WHERE fecha_hora_salida IS NULL
;

-- reservaciones finalizadas + datos estacionamiento + datos cliente
SELECT * FROM reservaciones_de_cuarto r 
JOIN estacionamiento_clientes e ON r.id_reservacion = e.id_reservacion
JOIN clientes c ON r.dni_cliente = c.dni
WHERE fecha_hora_salida IS NOT NULL
;


-- datos para calcular cuándo van a estar disponibles los cuartos luego de limpiarlos:
SELECT *
FROM limpieza_cuartos l
JOIN reservaciones_de_cuarto r
ON l.id_reservacion = r.id_reservacion;

-- cuartos disponibles
SELECT * FROM cuartos WHERE disponible = TRUE;

-- estacionamientos libres
SELECT e.numero_lote AS lotes_libres FROM estacionamiento e
WHERE e.numero_lote NOT IN (
	SELECT numero_lote FROM estacionamiento_clientes 
	WHERE 
		id_reservacion IN(
			(SELECT id_reservacion FROM reservaciones_de_cuarto WHERE fecha_hora_salida IS NULL)
		)
	AND 
		numero_lote IS NOT NULL
);

-- estacionamientos ocupados + datos reservacion
SELECT * FROM estacionamiento e
JOIN ( 
	SELECT ec.numero_lote, ec.dni_cliente, ec.id_reservacion, r.numero_cuarto, r.fecha_hora_entrada, r.fecha_hora_salida
	FROM estacionamiento_clientes ec 
	JOIN reservaciones_de_cuarto r ON r.id_reservacion = ec.id_reservacion
	WHERE r.fecha_hora_salida IS NULL
) AS tabla_join
ON e.numero_lote = tabla_join.numero_lote
ORDER BY e.numero_lote
;