-- reservaciones vigentes + datos estacionamiento + datos cliente
SELECT * FROM reservaciones_de_cuarto r 
JOIN estacionamiento_clientes e ON r.id_reservacion = e.id_reservacion
JOIN clientes c ON r.dni_cliente = c.dni
WHERE fecha_hora_salida IS NULL
;

-- reservaciones finalizadas + datos estacionamiento + datos cliente
SELECT * FROM reservaciones_de_cuarto r 
JOIN estacionamiento_clientes e ON r.id_reservacion = e.id_reservacion
JOIN clientes c ON r.dni_cliente = c.dni
WHERE fecha_hora_salida IS NOT NULL
;


-- datos para calcular cuándo van a estar disponibles los cuartos luego de limpiarlos:
SELECT *
FROM limpieza_cuartos l
JOIN reservaciones_de_cuarto r
ON l.id_reservacion = r.id_reservacion;

-- cuartos disponibles
SELECT * FROM cuartos WHERE disponible = TRUE;

-- estacionamientos libres
SELECT e.numero_lote AS lotes_libres FROM estacionamiento e
WHERE e.numero_lote NOT IN (
	SELECT numero_lote FROM estacionamiento_clientes 
	WHERE 
		id_reservacion IN(
			(SELECT id_reservacion FROM reservaciones_de_cuarto WHERE fecha_hora_salida IS NULL)
		)
	AND 
		numero_lote IS NOT NULL
);

-- estacionamientos ocupados + datos reservacion
SELECT * FROM estacionamiento e
JOIN ( 
	SELECT ec.numero_lote, ec.dni_cliente, ec.id_reservacion, r.numero_cuarto, r.fecha_hora_entrada, r.fecha_hora_salida
	FROM estacionamiento_clientes ec 
	JOIN reservaciones_de_cuarto r ON r.id_reservacion = ec.id_reservacion
	WHERE r.fecha_hora_salida IS NULL
) AS tabla_join
ON e.numero_lote = tabla_join.numero_lote
ORDER BY e.numero_lote
;