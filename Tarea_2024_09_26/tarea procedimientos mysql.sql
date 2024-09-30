CREATE TABLE IF NOT EXISTS personas (
	id INTEGER PRIMARY KEY,
	nombre VARCHAR(20) NOT NULL,
	email VARCHAR(50) NOT NULL
);

DROP TABLE IF EXISTS empleados;
CREATE TABLE IF NOT EXISTS empleados (
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
	posicion VARCHAR(30), 
		CHECK (posicion IN ('adm', 'op', 'tr') ),	
	id_persona INTEGER NOT NULL, 
		FOREIGN KEY(id_persona) REFERENCES personas(id),
	salario FLOAT NOT NULL,
	fecha_baja DATETIME NULL
);

DROP PROCEDURE IF EXISTS cargarDatos;
DELIMITER ~~
CREATE PROCEDURE cargarDatos()
BEGIN
	DECLARE id INT DEFAULT 0;
	DECLARE nombre VARCHAR(20);
	DECLARE email VARCHAR(50);
	
	DELETE FROM empleados;
	DELETE FROM personas;
	
	SET id = 0;
	miLoop : LOOP
		SET nombre = CONCAT('persona_', CAST(id AS CHAR(20)));
		SET email = CONCAT(nombre, '@gmail.com');
		
		INSERT INTO personas 
		VALUES (id, nombre, email);
		
		SET id = id + 1;
		
		IF id >= 10 
		THEN 
-- 			SELECT * FROM personas;
			LEAVE miLoop;
		END IF;
		
	END LOOP miLoop;
END~~
DELIMITER ;

CALL cargarDatos();

INSERT INTO empleados (posicion, id_persona, salario, fecha_baja )
	VALUES ("adm", 7, 4000, NULL);
INSERT INTO empleados (posicion, id_persona, salario, fecha_baja )
	VALUES ("tr", 4, 2000, NULL);
INSERT INTO empleados (posicion, id_persona, salario, fecha_baja )
	VALUES ("tr", 5, 2000, NULL);

SELECT * FROM empleados e
JOIN personas p
ON e.id_persona = p.id;
-- 
-- 
-- SELECT * FROM personas
-- WHERE id NOT IN (SELECT id_persona FROM empleados);
-- 
-- SELECT COUNT(personas_no_empleadas.id) 
-- FROM (
-- 	SELECT * FROM personas
-- 	WHERE id NOT IN (SELECT id_persona FROM empleados)
-- ) personas_no_empleadas;
	

-- DROP PROCEDURE IF EXISTS ejercicio1_SIN_CURSOR;
-- DELIMITER ~~~
-- CREATE PROCEDURE ejercicio1_SIN_CURSOR()
-- BEGIN
-- 	DECLARE cantPersonasAgregar INT;
-- 	DECLARE salarioBase INT;
-- 	DECLARE posicion VARCHAR(30);
-- 	DECLARE iteracion INT;
-- 		
-- 	
-- 	SET cantPersonasAgregar = (
-- 		SELECT COUNT(*) FROM (
-- 			SELECT id FROM personas
-- 			-- personas que no están en la tabla empleados:
-- 			WHERE id NOT IN (SELECT id_persona FROM empleados)
-- 		) AS personas_no_empleadas
-- 	);
-- 	SET salarioBase = 3000;
-- 	SET posicion = 'op';
-- 	
-- 	
-- 	SET iteracion = 1;
-- 	insertEmpleadosLoop : LOOP
-- 		IF (iteracion > cantPersonasAgregar)
-- 			LEAVE insertEmpleadosLoop;
-- 		END IF;
-- 		
-- 		-- No se puede iterar una columna sin usar cursores,
-- 		-- salvo que:
-- 			-- 1) se cree una tabla "indice_columna" con campos: indice PK, id_persona FK
-- 			-- 2) se haga un JOIN entre personas_no_empleadas y indice_columna
-- 			-- 3) se use el campo (tabla JOIN resultante).indice = iteracion dentro de un WHERE
-- 		-- Como es mucho más complejo, mejor usar un cursor.
-- 		
-- 		iteracion = iteracion + 1;
-- 	END LOOP insertEmpleadosLoop;
-- END
-- ~~~
-- DELIMITER ;
-- 
-- CALL ejercicio1_SIN_CURSOR();


DROP PROCEDURE IF EXISTS ejercicio1;
DELIMITER ~~~

CREATE PROCEDURE ejercicio1()
BEGIN
	-- variables del ejercicio
	DECLARE salarioBase INT DEFAULT 3000;
	DECLARE posicionEmp VARCHAR(30) DEFAULT 'op';

	-- variables del loop
	DECLARE terminado BOOL DEFAULT FALSE;

	-- variables del cursor
	DECLARE idPersonaNoEmpleada INT;
	DECLARE idPersonasNoEmpleadasCursor CURSOR FOR (
		SELECT id FROM personas 
		WHERE id NOT IN (SELECT id_persona FROM empleados)
	);
	DECLARE CONTINUE HANDLER FOR NOT FOUND
		FinalizarCursor: BEGIN  
			SET terminado = TRUE;
			CLOSE idPersonasNoEmpleadasCursor;
		END FinalizarCursor
	;
	
	-- abrir cursor
	OPEN idPersonasNoEmpleadasCursor;

	-- inicio loop: insertar personas no empleadas en tabla empleados
	LoopInserts : LOOP
		-- iterar el cursor (columna de IDs de las personas no empleadas).
		-- Cuando tira NOT FOUND al llegar al final, se activa el HANDLER, que setea terminado=true
		FETCH idPersonasNoEmpleadasCursor INTO idPersonaNoEmpleada;
	
		-- condición de corte, depende de la iteración del cursor así que debe ir después del FETCH
		IF (terminado)
			THEN LEAVE LoopInserts;
		END IF;
		
		
		INSERT INTO empleados (posicion, id_persona, salario) 
			VALUES (posicionEmp, idPersonaNoEmpleada, salarioBase)
		;

		SET salarioBase = salarioBase + 200;
		
	END LOOP LoopInserts;

	-- resultado ejercicio:
	SELECT * FROM empleados;
END

~~~
DELIMITER ;

CALL ejercicio1();


DROP PROCEDURE IF EXISTS ejercicio2;
DELIMITER ~~~

CREATE PROCEDURE ejercicio2( IN valor_a_superar INT )
BEGIN
	-- variables del ejercicio
 	DECLARE sumaSalarios INT DEFAULT 0;
 	DECLARE cantSalarios INT DEFAULT 0;
 	DECLARE respuestaString VARCHAR(100);

	-- variables del loop
	DECLARE terminado BOOL DEFAULT FALSE;
	
	-- variables del cursor
	DECLARE salarioEmpleado INT;
	DECLARE salarioEmpleadosMayorAMenorCursor CURSOR FOR (
		SELECT salario FROM empleados ORDER BY salario DESC
	);
	DECLARE CONTINUE HANDLER FOR NOT FOUND
		FinalizarCursor : BEGIN
			SET terminado = TRUE;
			CLOSE salarioEmpleadosMayorAMenorCursor;
		END FinalizarCursor
	;
	
	-- abrir cursor
	OPEN salarioEmpleadosMayorAMenorCursor;
	
	-- loop: averiguar cantidad de salarios de empleados cuya suma supere a valor_a_superar
	averiguarCantidadSalariosLoop : LOOP
		-- iterar cursor
		FETCH salarioEmpleadosMayorAMenorCursor INTO salarioEmpleado;
		
		-- cortar loop si el cursor llegó al final
		IF (terminado)
			THEN LEAVE averiguarCantidadSalariosLoop;
		END IF;
		
		
		SET sumaSalarios = sumaSalarios + salarioEmpleado;
		SET cantSalarios = cantSalarios + 1;
		
		IF (sumaSalarios > valor_a_superar)
			THEN LEAVE averiguarCantidadSalariosLoop;
		END IF;
		

	END LOOP averiguarCantidadSalariosLoop;


   -- definir resultado:
	IF (sumaSalarios > valor_a_superar)
		THEN 
			SET respuestaString =  CONCAT(
				'La cantidad de salarios para superar ',
				CAST(valor_a_superar AS CHAR(10)),
				' es : ',
				CAST(cantSalarios AS CHAR(10)),
				'. \nSu suma es:  ',
				CAST(sumaSalarios AS CHAR(10))
			);
			SELECT cantSalarios;
			SELECT respuestaString;
		
		ELSE 
			SET respuestaString = CONCAT(
				'La suma de los salarios de los empleados, ',
				CAST(sumaSalarios AS CHAR),
				', NO supera el valor ingresado: ',
				CAST(valor_a_superar AS CHAR)
			);
			SELECT cantSalarios;
			SELECT respuestaString;
			
	END IF;

END;

~~~
DELIMITER ;

CALL ejercicio2(10000);
CALL ejercicio2(999999);

