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
