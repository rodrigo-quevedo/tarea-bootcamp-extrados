CREATE USER IF NOT EXISTS nuevo_usuario
IDENTIFIED BY 'pass1234';

-- GRANT SELECT
-- ON tarea_vistas.*
-- TO nuevo_usuario;
-- 
-- REVOKE SELECT 
-- ON tarea_vistas.*
-- FROM nuevo_usuario;

-- DROP USER nuevo_usuario;


-- 1) lista de empleados sin el sueldo
CREATE OR REPLACE VIEW vista_ejercicio1
AS 
	SELECT e.id_empleado AS id_empleado_from_view, e.dni, e.rol, e.fecha_ingreso, e.fecha_egreso 
	FROM empleados e;

SELECT * FROM vista_ejercicio1;

-- 2) lista de los empelados vigentes (fecha_egreso = null) (sin el sueldo)
CREATE OR REPLACE VIEW vista_ejercicio2
AS 
	SELECT * FROM vista_ejercicio1 WHERE fecha_egreso IS NULL;

SELECT * FROM vista_ejercicio2;

-- 3) lista de vehiculos con los datos del empleado al que pertence (sin el sueldo) (hacer join)
CREATE OR REPLACE VIEW vista_ejercicio3
AS 
	SELECT * FROM vehiculos
	JOIN vista_ejercicio1
	ON vehiculos.id_empleado = vista_ejercicio1.id_empleado_from_view;


SELECT * FROM vista_ejercicio3;

-- 4) lista de personas que no sean empleados


