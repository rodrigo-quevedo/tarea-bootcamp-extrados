-- es buscado: ID 99
-- buscador: ID 100

SELECT * FROM perfil_usuarios 
WHERE id_usuario = 98
AND (
	id_usuario IN 
		(SELECT id_jugador_1 FROM partidas
		WHERE id_jugador_1 = 100 OR id_jugador_2 = 100)
	OR 
	id_usuario IN 
		(SELECT id_jugador_2 FROM partidas
		WHERE id_jugador_1 = 100 OR id_jugador_2 = 100)
	OR 
	id_usuario IN 
		(SELECT id_juez FROM partidas
		WHERE id_jugador_1 = 100 OR id_jugador_2 = 100)
) 
	 