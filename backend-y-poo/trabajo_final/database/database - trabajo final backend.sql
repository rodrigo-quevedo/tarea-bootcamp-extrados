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

DROP TABLE IF EXISTS jueces;
CREATE TABLE jueces(
	id_juez INT PRIMARY KEY,
	FOREIGN KEY(id_juez) REFERENCES usuarios(id),
	
	-- acá no conviene guardar la foto en sí, sino una URL o path/ruta/directorio
	-- el servidor lee esa URL o path y busca el archivo en el OS
	foto VARCHAR(200) NOT NULL,
	
	alias VARCHAR(25) NOT NULL
);


DROP TABLE IF EXISTS jugadores;
CREATE TABLE jugadores (
	id_jugador INT PRIMARY KEY,
	FOREIGN KEY(id_jugador) REFERENCES usuarios(id),

	foto VARCHAR(200) NOT NULL, -- URL/path del archivo
	
	alias VARCHAR(25) NOT NULL
	
	-- Estos van en sus propia tabla:
		-- cartas_coleccionadas
		-- descalificaciones
	
	-- Esto se puede sacar de otras tablas ya existentes:
		-- Juegos ganados
		-- Juegos perdidos
		-- Torneos ganados

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
	
	id_jugador INT NOT NULL,
	FOREIGN KEY (id_jugador) REFERENCES jugadores(id_jugador),
	
	PRIMARY KEY(id_carta, id_jugador)
);

DROP TABLE IF EXISTS torneos;
CREATE TABLE torneos(
	id INT PRIMARY KEY AUTO_INCREMENT,
	
	-- inicio y fin: cada torneo se realiza en el mismo dia, por lo que
	-- las horas disponibles para los juegos se calculan con horaIncio - horaFin
	fecha_hora_inicio DATETIME NOT NULL,	
	fecha_hora_fin DATETIME NOT NULL,	
	
	-- rondas: se calcula con lo de arriba (ya viene calculado desde el server)
	cantidad_rondas INT NOT NULL,

	pais VARCHAR(30) NOT NULL,
	
	fase VARCHAR(12) NOT NULL,
	CHECK (fase IN ('registro', 'torneo', 'finalizado')),
	
	id_organizador INT NOT NULL,
	FOREIGN KEY(id_organizador) REFERENCES usuarios(id),
	
	id_ganador INT NULL,
	FOREIGN KEY(id_ganador) REFERENCES jugadores(id_jugador)
	
	-- Ronda actual del torneo: 
	-- SELECT numero_ronda FROM juegos_de_ronda
		-- WHERE id_torneo=(id torneo) 
		-- ORDER BY numero_ronda DESC
		-- LIMIT 1;
);

-- Lista de espera: 
	-- los Jugadores se van registrando al torneo y entran a esta tabla
	-- luego, el Organizador acepta jugadores de esta lista y se agregan a la tabla "jugadores_inscriptos"
DROP TABLE IF EXISTS jugadores_a_inscribir;
CREATE TABLE jugadores_a_inscribir(
	id_jugador INT NOT NULL,
	FOREIGN KEY(id_jugador) REFERENCES jugadores(id_jugador),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),
	
	PRIMARY KEY(id_jugador, id_torneo)
);


DROP TABLE IF EXISTS jugadores_inscriptos;
CREATE TABLE jugadores_inscriptos(
	id_jugador INT NOT NULL,
	FOREIGN KEY(id_jugador) REFERENCES jugadores(id_jugador),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),
	
	PRIMARY KEY(id_jugador, id_torneo)
);


-- las cartas coleccionadas pueden ir cambiando
-- pero las cartas del mazo deben quedar registradas igualmente
-- por lo tanto, son colecciones independientes a nivel DB, es decir,
-- que el servidor verificará que las cartas del mazo estén en las cartas coleccionadas.
DROP TABLE IF EXISTS mazos;
CREATE TABLE mazos(
	id_jugador INT NOT NULL,
	FOREIGN KEY(id_jugador) REFERENCES jugadores(id_jugador),
	
	id_carta INT NOT NULL,
	FOREIGN KEY(id_carta) REFERENCES cartas(id),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),
	
	-- no puede haber cartas repetidas para un jugador de un torneo:
	PRIMARY KEY(id_torneo, id_jugador, id_carta) 
);


DROP TABLE IF EXISTS series_habilitadas;
CREATE TABLE series_habilitadas(
	nombre_serie VARCHAR(20) NOT NULL,
	FOREIGN KEY(nombre_serie) REFERENCES series(nombre),
	
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id)
);


DROP TABLE IF EXISTS jueces_torneo;
CREATE TABLE jueces_torneo (
	id_torneo INT NOT NULL,
	FOREIGN KEY(id_torneo) REFERENCES torneos(id),
	
	id_juez INT NOT NULL,
	FOREIGN KEY(id_juez) REFERENCES jueces(id_juez),
	
	PRIMARY KEY (id_torneo, id_juez)
);


-- RONDAS Y JUEGOS -- 

DROP TABLE IF EXISTS juegos_de_ronda;
CREATE TABLE juegos_de_ronda(
	id INT PRIMARY KEY AUTO_INCREMENT,

	numero_ronda INT NOT NULL,
	
	id_torneo INT NOT NULL,
	FOREIGN KEY (id_torneo) REFERENCES torneos(id),
	
	-- calcular datetime inicio y fin en el server (duracion 30min)
	fecha_hora_inicio DATETIME NOT NULL,
	fecha_hora_fin DATETIME NOT NULL,
	
	-- los 2 jugadores se asignan al crearse el juego
	id_jugador_1 INT NOT NULL,
	FOREIGN KEY(id_jugador_1) REFERENCES jugadores(id_jugador),
	id_jugador_2 INT NOT NULL,
	FOREIGN KEY(id_jugador_2) REFERENCES jugadores(id_jugador),

	id_ganador INT NULL,
	FOREIGN KEY(id_ganador) REFERENCES jugadores(id_jugador),
	CHECK(id_ganador IS NULL OR id_ganador = id_jugador_1 OR id_ganador = id_jugador_2),
	
	id_descalificado INT NULL,
	FOREIGN KEY (id_descalificado) REFERENCES jugadores(id_jugador),
	-- Obtener descalificaciones de un usuario:
		-- SELECT * from juegos_de_ronda
		-- WHERE id_descalificado = (id del usuario);
	
	
	id_juez INT NOT NULL,
	FOREIGN KEY(id_juez) REFERENCES jueces(id_juez)
	
);




-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=trabajo_final_backend_user;Password=123456;Database=trabajo_final_backend;"
DROP USER IF EXISTS trabajo_final_backend_user;
CREATE USER trabajo_final_backend_user IDENTIFIED BY '123456';
GRANT SELECT, INSERT, UPDATE ON trabajo_final_backend.* TO trabajo_final_backend_user;


-- Hardcodear admin
-- INSERT INTO usuarios(rol, pais, nombre_apellido, email, password)
-- VALUES ('admin', 'Argentina -03:00', 'juan gonzales', 'juangonzales@gmail.com', "adminpassword");


