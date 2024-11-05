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

