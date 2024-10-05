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
	(43908304, 'paula', 'díaz', 'pauladiaz@gmail.com');

SELECT * FROM personas;

-- empleados
INSERT INTO empleados(dni, sueldo, rol, fecha_ingreso, fecha_egreso) VALUES 
	(38290310, 500000, 'vendedor', '2018-02-15', NULL),
	(39973920, 500000, 'vendedor', '2018-06-21', NULL),
	(15920939, 480000, 'vendedor', '2020-11-20', NULL),
	(26305890, 550000, 'manager', '2021-03-29', NULL);

SELECT * FROM empleados;
SELECT * FROM empleados JOIN personas ON empleados.dni = personas.dni;

-- vehiculos


-- estacionamiento


-- productos


-- ventas