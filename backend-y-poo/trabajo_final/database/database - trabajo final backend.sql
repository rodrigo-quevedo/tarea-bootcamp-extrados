DROP DATABASE IF EXISTS trabajo_final_backend;
CREATE DATABASE trabajo_final_backend;
USE trabajo_final_backend;


-- Creación de usuario: 
-- 1. El servidor obtiene los datos del usuario a crear.
-- 2. Se añade fila en "usuarios". Se obtiene la ID de esa fila.
-- 3. Se añade fila en tabla del rol correspondiente. Se agrega ID usuario a esa tabla como FK.
DROP TABLE IF EXISTS usuarios;
CREATE TABLE usuarios(
	id INT PRIMARY KEY AUTO_INCREMENT,
	rol VARCHAR(15) NOT NULL,
	CHECK(rol IN("admin", "organizador", "juez", "jugador")),

	pais VARCHAR(30) NOT NULL,
	
	nombre_apellido VARCHAR(60) NOT NULL,
	email VARCHAR(30) UNIQUE NOT NULL,
	
	password VARCHAR(255) NOT NULL,
	
	-- para borrado logico
	activo BOOL NOT NULL,
	
	-- usuario_creador: admite NULL porque jugador se puede auto-registrar
	id_usuario_creador INT NULL,
	FOREIGN KEY(id_usuario_creador) REFERENCES usuarios(id)
	
	-- refresh_token VARCHAR(300) NULL -->esto no permite multiples dispositivos
);

DROP TABLE IF EXISTS refresh_tokens;
CREATE TABLE refresh_tokens (
	refresh_token VARCHAR(300) PRIMARY KEY,
	
	id_usuario INT NOT NULL,
	FOREIGN KEY(id_usuario) REFERENCES usuarios(id),

	token_activo BOOL NOT NULL -- borrado logico
);

-- Posiblemente 'admin' no necesite tabla (con los atributos de la tabla usuario puede hacer sus tareas)
-- Lo mismo pasa con 'organizador'

DROP TABLE IF EXISTS perfil_usuarios;
CREATE TABLE perfil_usuarios(
	id_usuario INT PRIMARY KEY,
	FOREIGN KEY(id_usuario) REFERENCES usuarios(id),

	foto VARCHAR(200) NULL, -- URL/path del archivo
	
	alias VARCHAR(25) NULL
);


DROP TABLE IF EXISTS series;
CREATE TABLE series(
	nombre VARCHAR(20) PRIMARY KEY,
	fecha_salida DATETIME NOT NULL -- usar DateTime UTC (transformar datetime a UTC en el servidor)
	
	-- lista de cartas: SELECT * FROM cartas WHERE serie=(nombre de la serie)
);

DROP TABLE IF EXISTS cartas;
CREATE TABLE cartas(
	id INT PRIMARY KEY,
	ataque INT NOT NULL,
	defensa INT NOT NULL,
	ilustracion VARCHAR(200) NOT NULL  -- URL/path del archivo
);


DROP TABLE IF EXISTS series_de_cartas;
CREATE TABLE series_de_cartas(
	nombre_serie VARCHAR(20) NOT NULL,
	FOREIGN KEY(nombre_serie) REFERENCES series(nombre),
	
	id_carta INT NOT NULL,
	FOREIGN KEY(id_carta) REFERENCES cartas(id),
	
	PRIMARY KEY(nombre_serie, id_carta)
);


-- Esta tabla implementa una relacion muchos a muchos, se usa PK de 2 columnas:
DROP TABLE IF EXISTS cartas_coleccionadas;
CREATE TABLE cartas_coleccionadas(
	id_carta INT NOT NULL,
	FOREIGN KEY (id_carta) REFERENCES cartas(id),
	trabajo_final_backendtrabajo_final_backend
	id_jugador INT NOT NULL,
	FOREIGN KEY (id_jugador) REFERENCES usuarios(id),
	
	PRIMARY KEY(id_carta, id_jugador)
);



DROP TABLE IF EXISTS series_habilitadas;
DROP TABLE IF EXISTS jueces_torneo;
DROP TABLE IF EXISTS jugadores_inscriptos;
DROP TABLE IF EXISTS cartas_del_mazo;

DROP TABLE IF EXISTS torneos;
CREATE TABLE torneos(
	id INT PRIMARY KEY AUTO_INCREMENT,
	
	fecha_hora_inicio DATETIME NOT NULL,	
	fecha_hora_fin DATETIME NOT NULL,	

	horario_diario_inicio VARCHAR(5) NOT NULL, -- HH:MM
	horario_diario_fin VARCHAR(5) NOT NULL,
	
   -- la elige el organizador, esta limitada si hay fecha_hora_fin
	cantidad_rondas INT NOT NULL,

	pais VARCHAR(30) NOT NULL,
	
	fase VARCHAR(12) NOT NULL,
 	CHECK (fase IN ('registro', 'torneo', 'finalizado')),
	
	id_organizador INT NOT NULL,
	FOREIGN KEY(id_organizador) REFERENCES usuarios(id)
	
	-- Id ganador:
	-- SELECT id_ganador FROM partidas
	-- WHERE id_torneo = [id torneo]
	-- AND ronda = [cantidad_rondas];
   
	
	-- Ronda actual del torneo: 
	-- SELECT ronda FROM partidas
		-- WHERE id_torneo=(id torneo) 
		-- ORDER BY ronda DESC
		-- LIMIT 1;
);

CREATE TABLE series_habilitadas(
	nombre_serie VARCHAR(20) NOT NULL,
	FOREIGN KEY(nombre_serie) REFERENCES series(nombre),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),
	
	PRIMARY KEY(nombre_serie, id_torneo)
);


CREATE TABLE jueces_torneo (
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),

	id_juez INT NOT NULL,
	FOREIGN KEY(id_juez) REFERENCES usuarios(id),
	
	PRIMARY KEY (id_torneo, id_juez)
);


CREATE TABLE jugadores_inscriptos(
	id_jugador INT NOT NULL,
	FOREIGN KEY(id_jugador) REFERENCES usuarios(id),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),

	PRIMARY KEY (id_jugador, id_torneo),
	
	-- Esto me sirve para solucionar problemas de concurrencia:
	-- Puede haber un exceso de inscripciones a un torneo (supongamos que
	-- queda 1 solo lugar, pero 2 jugadores distintos leen fase=registro 
	-- al mismo tiempo y se inscriben ambos, excediendo en 1)
	
	-- Solucion: El organizador, al aceptar las inscripciones y cambiar
	-- de fase (registro ->torneo) lee los inscriptos al torneo con:
	-- ORDER BY id DESC LIMIT [cantidad_valida_jugadores], y a todos esos
	-- jugadores les asigna aceptado = true.
	orden INT NOT NULL UNIQUE AUTO_INCREMENT,
	aceptado BOOL NOT NULL	
);

-- Validar jueces sin SELECT:
-- INSERT INTO jueces_torneo
-- VALUES (
--   1,
--   (SELECT id FROM USUARIOS
--    WHERE id=94
--    AND rol = 'juez')
-- );

-- las cartas coleccionadas pueden ir cambiando
-- pero las cartas del mazo deben quedar registradas igualmente
-- por lo tanto, son colecciones independientes a nivel DB, es decir,
-- que el servidor verificará que las cartas del mazo estén en las cartas coleccionadas.

CREATE TABLE cartas_del_mazo(
	id_jugador INT NOT NULL,
	FOREIGN KEY(id_jugador) REFERENCES usuarios(id),
	
	id_carta INT NOT NULL,
	FOREIGN KEY(id_carta) REFERENCES cartas(id),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),
	
	-- no puede haber cartas repetidas para un jugador de un torneo:
	PRIMARY KEY(id_torneo, id_jugador, id_carta) 
);



-- RONDAS Y JUEGOS -- 

DROP TABLE IF EXISTS partidas;
CREATE TABLE partidas(
	id INT PRIMARY KEY AUTO_INCREMENT,

	ronda INT NOT NULL,
	
	id_torneo INT NOT NULL,
	FOREIGN KEY (id_torneo) REFERENCES torneos(id),
	
	-- calcular datetime inicio y fin en el server (duracion 30min)
	fecha_hora_inicio DATETIME NOT NULL,
	fecha_hora_fin DATETIME NOT NULL,
	
	-- los 2 jugadores se asignan al crearse el juego
	id_jugador_1 INT NOT NULL,
	FOREIGN KEY(id_jugador_1) REFERENCES usuarios(id),
	id_jugador_2 INT NOT NULL,
	FOREIGN KEY(id_jugador_2) REFERENCES usuarios(id),

	id_ganador INT NULL,
	FOREIGN KEY(id_ganador) REFERENCES usuarios(id),
	CHECK(id_ganador IS NULL OR id_ganador = id_jugador_1 OR id_ganador = id_jugador_2),
	
	id_descalificado INT NULL,
	FOREIGN KEY (id_descalificado) REFERENCES usuarios(idcartas_coleccionadas),
	-- Obtener descalificaciones de un usuario:
		-- SELECT * from juegos_de_ronda
		-- WHERE id_descalificado = (id del usuario);
	
	
	id_juez INT NOT NULL,
	FOREIGN KEY(id_juez) REFERENCES usuarios(id)
	
);




-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=trabajo_final_backend_user;Password=123456;Database=trabajo_final_backend;"
DROP USER IF EXISTS trabajo_final_backend_user;
CREATE USER trabajo_final_backend_user IDENTIFIED BY '123456';
GRANT SELECT, INSERT, UPDATE, DELETE ON trabajo_final_backend.* TO trabajo_final_backend_user;


-- Hardcodear admin
-- INSERT INTO usuarios(rol, pais, nombre_apellido, email, password)
-- VALUES ('admin', 'Argentina -03:00', 'juan gonzales', 'juangonzales@gmail.com', "adminpassword");


