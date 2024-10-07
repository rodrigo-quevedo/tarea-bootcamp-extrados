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
	(22092321, 'alma', 'sosa', 'almasosa@mail.com')
;


-- empleados
INSERT INTO empleados(dni, sueldo, rol, fecha_ingreso, fecha_egreso) VALUES 
	(38290310, 500000, 'vendedor', '2018-02-15', NULL),
	(39973920, 500000, 'vendedor', '2018-06-21', NULL),
	(40048589, 480000, 'vendedor', '2018-06-21', '2018-09-21'),
	(15920939, 480000, 'vendedor', '2020-11-20', NULL),
	(26305890, 550000, 'manager', '2021-03-29', NULL)
;

SELECT * FROM empleados RIGHT JOIN personas ON empleados.dni = personas.dni;


-- vehiculos
INSERT INTO vehiculos VALUES
	('AB523FX', 'chevrolet corsa', 'blanco', 1),
	('AT981KL', 'volkswagen gol', 'gris', 2),
	('BS238WA', 'fiat palio', 'rojo', 1),
	('AB125AE', 'chevrolet chevy', 'verde', 4),
	('CA045OR', 'citroen c3', 'blanco', 5)
;
	
SELECT v.patente, v.modelo, v.color, v.id_empleado, 
datos_personales_empleados.dni, datos_personales_empleados.nombre, datos_personales_empleados.apellido, datos_personales_empleados.email,
datos_personales_empleados.rol, datos_personales_empleados.sueldo, datos_personales_empleados.fecha_ingreso, datos_personales_empleados.fecha_egreso
FROM vehiculos v 
JOIN 
	(SELECT e.id_empleado, e.rol, e.sueldo, e.fecha_ingreso, e.fecha_egreso, p.dni, p.nombre, p.apellido, p.email
	FROM empleados e JOIN personas p ON e.dni = p.dni
	) AS datos_personales_empleados 
	ON datos_personales_empleados.id_empleado = v.id_empleado
;

-- estacionamiento
INSERT INTO estacionamiento VALUES
	('BS238WA', '2024-10-01', 1),
	('CA045OR', '2024-10-01', 2),
	('AT981KL', '2024-10-01', 3),
	('AB125AE', '2024-10-01', 4),
	
	('CA045OR', '2024-10-02', 1),
	('BS238WA', '2024-10-02', 2),
	('AB125AE', '2024-10-02', 3),
	('AT981KL', '2024-10-02', 4),
	
	('CA045OR', '2024-10-03', 1),
	('BS238WA', '2024-10-03', 2),
	('AT981KL', '2024-10-03', 3),
	
	('BS238WA', '2024-10-04', 1),
	('CA045OR', '2024-10-04', 2),
	('AT981KL', '2024-10-04', 3),
	('AB125AE', '2024-10-04', 4)
;

SELECT v.patente, est.fecha_ingreso AS fecha_estacionamiento, est.lote, e.id_empleado, p.nombre, p.apellido FROM estacionamiento est
RIGHT JOIN vehiculos v ON est.patente = v.patente
JOIN empleados e ON v.id_empleado = e.id_empleado
JOIN personas p ON e.dni = p.dni
ORDER BY fecha_estacionamiento, est.lote ASC
;

-- SELECT v.patente, est.fecha_ingreso AS fecha_estacionamiento, est.lote, e.id_empleado, p.nombre, p.apellido FROM estacionamiento est
-- JOIN vehiculos v ON est.patente = v.patente
-- JOIN empleados e ON v.id_empleado = e.id_empleado
-- JOIN personas p ON e.dni = p.dni
-- -- LEFT JOIN estacionamiento est_l ON est_l.patente = v.patente
-- -- WHERE 
-- -- 	( 
-- -- 		(est.fecha_ingreso = '2024-10-03')
-- -- 	)
-- -- 	OR
-- -- 	( 
-- -- 	(est.fecha_ingreso IS NULL) 
-- -- 	)
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

SELECT * FROM productos;


-- ventas
INSERT INTO ventas VALUES 
	('00000024',31939029, 1, "2024-10-01 11:06"),
	('00004333',31939029, 1, "2024-10-01 11:06"),
	('00000300',14423898, 2, "2024-10-01 12:18"),	
	('00000026',22092321, 1, "2024-10-01 14:53")
	
;

SELECT * FROM ventas v JOIN empleados e ON v.id_empleado=e.id_empleado
JOIN productos p ON p.codigo_barra = v.codigo_barra;


