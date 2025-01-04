-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=trabajo_final_backend_user;Password=123456;Database=trabajo_final_backend;"

DROP DATABASE IF EXISTS trabajo_final_backend;
CREATE DATABASE trabajo_final_backend;
USE trabajo_final_backend;

DROP USER IF EXISTS trabajo_final_backend_user;
CREATE USER trabajo_final_backend_user IDENTIFIED BY '123456';


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
	email VARCHAR(50) NOT NULL
);

-- Posiblemente 'admin' no necesite tabla (con los atributos de la tabla usuario puede hacer sus tareas)


DROP TABLE IF EXISTS organizadores;
CREATE TABLE organizadores(
	id_organizador INT PRIMARY KEY,
	FOREIGN KEY(id_usuario) REFERENCES usuarios(id),
	
	-- quién creó a este usuario
	id_usuario_creador INT UNIQUE NOT NULL,
	FOREIGN KEY(id_usuario_creador) REFERENCES usuarios(id)
	
	-- esto necesita su propia tabla, y que cada fila (torneo) referencie al ID organizador como FK
	-- en realidad tampoco necesita eso, basta con poner una columna "id_organizador" en la tabla Torneos.
	-- Para calcular los torneos organizados, hay que hacer SELECT * FROM torneos WHERE id_organizador=(el id del organizador del que se quiera obtener los torneos organizados)
-- 	torneos_organizados INT UNIQUE NOT NULL,
-- 	FOREIGN KEY(torneos_organizados) REFERENCES
);

DROP TABLE IF EXISTS jueces;
CREATE TABLE jueces(
	id_juez INT PRIMARY KEY,
	FOREIGN KEY(id_juez) REFERENCES usuarios(id),
	
	-- acá no conviene guardar la foto en sí, sino una URL o path/ruta/directorio
	-- el servidor lee esa URL o path y busca el archivo en el OS
	foto VARCHAR(200) NOT NULL,
	
	alias VARCHAR(25) NOT NULL 
	
	-- Torneos en que partició: pueden estar en una tabla "torneos" o "jueces de torneos"
		-- (ya que va a haber más de 1, conviene que "jueces de torneos" sea una tabla a la que "torneos" hace referencia con una id)
		-- Esto puede cambiar más adelante. 
		-- Lo que es seguro es que no va a acá porque sería duplicar información.
	
	-- quién creó a este usuario
	id_usuario_creador INT UNIQUE NOT NULL,
	FOREIGN KEY(id_usuario_creador) REFERENCES usuarios(id)
);


DROP TABLE IF EXISTS jugadores;
CREATE TABLE jugadores (
	id_jugador INT PRIMARY KEY,
	FOREIGN KEY (id_jugador) REFERENCES usuarios(id),

	foto VARCHAR(200) NOT NULL, -- URL/path del archivo
	
	alias VARCHAR(25) NOT NULL,
	
	id_usuario_creador INT UNIQUE NOT NULL,
	FOREIGN KEY(id_usuario_creador) REFERENCES usuarios(id),
	
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
	
	serie INT NOT NULL,
	FOREIGN KEY(serie) REFERENCES series(nombre);
);


-- Esta tabla implementa una relacion muchos a muchos, no se usa PK:
DROP TABLE IF EXISTS cartas_coleccionadas;
CREATE TABLE cartas_coleccionadas(
	id_carta INT NOT NULL,
	FOREIGN KEY (id_carta) REFERENCES cartas(id),
	
	id_usuario INT NOT NULL,
	FOREIGN KEY (id_usuario) REFERENCES usuarios(id)
);


