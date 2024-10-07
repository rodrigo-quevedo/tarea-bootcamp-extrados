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