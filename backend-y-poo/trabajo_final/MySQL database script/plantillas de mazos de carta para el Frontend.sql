
DROP TABLE IF EXISTS mazos_de_jugador;

CREATE TABLE mazos_de_jugador( 
	id_mazo INT NOT NULL,

	id_jugador INT NOT NULL,
	FOREIGN KEY(id_jugador) REFERENCES usuarios(id),
	
	id_carta INT NOT NULL,
	FOREIGN KEY(id_carta) REFERENCES cartas(id),
	
	PRIMARY KEY(id_mazo, id_jugador, id_carta)
);