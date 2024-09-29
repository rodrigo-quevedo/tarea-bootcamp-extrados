-- CONSIGNAS

-- 1- obtener todas las locaciones de los estados unidos (ID de pais “US”)
SELECT * FROM locaciones WHERE ID_PAIS = "US"
ORDER BY ID_LOCACION;

-- 2- obtener todos los departamentos de los estado unidos
SELECT * FROM departamentos d
JOIN 
	(SELECT * FROM locaciones WHERE ID_PAIS = "US"
	ORDER BY ID_LOCACION) locaciones_US
ON d.ID_LOCACION = locaciones_US.ID_LOCACION
ORDER BY d.ID_DEPARTAMENTO;


-- 3- obtener todos los managers que reporten a un departamento de los estados unidos.

SELECT * FROM empleados e
JOIN 
	(SELECT d.ID_DEPARTAMENTO, d.NOMBRE, d.ID_MANAGER,
		 locaciones_US.ID_LOCACION, locaciones_US.DIRECCION, locaciones_US.CODIGO_POSTAL,
		 locaciones_US.CIUDAD, locaciones_US.ESTADO_PROVINCIA, ID_PAIS
	FROM departamentos d
	JOIN 
		(SELECT * FROM locaciones WHERE ID_PAIS = "US"
		ORDER BY ID_LOCACION) locaciones_US
	ON d.ID_LOCACION = locaciones_US.ID_LOCACION
	ORDER BY d.ID_DEPARTAMENTO) departamentos_US
ON e.ID_EMPLEADO = departamentos_US.ID_MANAGER;

-- 4- obtener todos los usuarios (empleados) que reporten a un manager que reporte a un departamento de los estados unidos.

SELECT * FROM empleados emp
JOIN (
	SELECT e.ID_EMPLEADO AS ID_MANAGER, e.NOMBRE AS NOMBRE_MANAGER, e.APELLIDO AS APELLIDO_MANAGER,
			departamentos_US.ID_DEPARTAMENTO, departamentos_US.NOMBRE AS NOMBRE_DEPARTAMENTO, departamentos_US.ID_LOCACION,
			departamentos_US.DIRECCION AS DIRECCION_DEPARTAMENTO, departamentos_US.CODIGO_POSTAL AS CODIGO_POSTAL_DEPARTAMENTO, departamentos_US.CIUDAD AS CIUDAD_DEPARTAMENTO,
			departamentos_US.ESTADO_PROVINCIA AS ESTADO_PROVINCIA_DEPARTAMENTO, departamentos_US.ID_PAIS AS ID_PAIS_DEPARTAMENTO
	FROM empleados e
	JOIN 
		(SELECT d.ID_DEPARTAMENTO, d.NOMBRE, d.ID_MANAGER,
			 locaciones_US.ID_LOCACION, locaciones_US.DIRECCION, locaciones_US.CODIGO_POSTAL,
			 locaciones_US.CIUDAD, locaciones_US.ESTADO_PROVINCIA, ID_PAIS
		FROM departamentos d
		JOIN 
			(SELECT * FROM locaciones WHERE ID_PAIS = "US"
			ORDER BY ID_LOCACION) locaciones_US
		ON d.ID_LOCACION = locaciones_US.ID_LOCACION
		ORDER BY d.ID_DEPARTAMENTO) departamentos_US
	ON e.ID_EMPLEADO = departamentos_US.ID_MANAGER) managers_US
ON emp.ID_MANAGER = managers_US.ID_MANAGER