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