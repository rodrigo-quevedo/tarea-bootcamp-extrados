-- USUARIOS Y PRIVILEGIOS
	-- 4 roles -> 4 usuarios del DBMS (sistema gestor de DB).

USE trabajo_final_backend;


-- ADMIN
	-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
	-- "Server=localhost;Port=3306;Username=admin_trabajo_final_backend;Password=123456;Database=trabajo_final_backend;"
DROP USER IF EXISTS admin_trabajo_final_backend;
CREATE USER admin_trabajo_final_backend IDENTIFIED BY '123456';

-- -> el rol de administrador debe poder crear, editar y eliminar otros administradores, organizadores, jueces y jugadores.
	-- ->borrado logico: UPDATE usuarios SET activo=false WHERE id=(id usuario)
			-- -> sin permiso a DELETE entonces
	-- ->creacion de usuario: INSERT en usuarios y en la tabla del rol correspondiente
	-- ->SELECT de rol: JOIN usuarios y WHERE activo=true

GRANT SELECT, INSERT, UPDATE 
	ON trabajo_final_backend.usuarios
	TO admin_trabajo_final_backend
;
GRANT SELECT, INSERT, UPDATE 
	ON trabajo_final_backend.organizadores
	TO admin_trabajo_final_backend
;

GRANT SELECT, INSERT, UPDATE 
	ON trabajo_final_backend.jueces
	TO admin_trabajo_final_backend
;

GRANT SELECT, INSERT, UPDATE 
	ON trabajo_final_backend.jugadores
	TO admin_trabajo_final_backend
;
	
	
-- ->el administrador debe poder ver y cancelar torneos.
		-- borrado: Si uso DELETE, primero debo borrar todas las filas cuyas FK apunten a la PK (id) de torneo.
			-- En el caso de que se use BORRADO LOGICO, no hace falta.
			-- (ej. UPDATE torneo SET activo=false WHERE id=idDelTorneo)
		-- el SELECT de tablas auxiliares debe comprobar WHERE activo=true
		
GRANT SELECT, UPDATE 
	ON trabajo_final_backend.torneos
	TO admin_trabajo_final_backend;

SHOW GRANTS FOR admin_trabajo_final_backend;


-- ORGANIZADOR

-- DROP USER IF EXISTS organizador_trabajo_final_backend;
-- CREATE USER organizador_trabajo_final_backend;
-- 
-- -- -> poder crear jueces
-- GRANT SELECT, INSERT 
-- 	ON trabajo_final_backend.usuarios
-- 	TO organizador_trabajo_final_backend;
-- 	
-- GRANT SELECT, INSERT 
-- 	ON trabajo_final_backend.jueces
-- 	TO organizador_trabajo_final_backend;
-- 
-- -- -> debe poder crear, editar y cancelar torneos
-- GRANT SELECT, INSERT, UPDATE
-- 	ON trabajo_final_backend.torneos
-- 	TO organizador_trabajo_final_backend;
-- 
-- GRANT SELECT, INSERT, UPDATE
-- 	ON trabajo_final_backend.torneos
-- 	TO organizador_trabajo_final_backend;