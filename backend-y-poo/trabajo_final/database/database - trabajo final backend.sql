-- Connection string (formato para MySQLConnector -> https://mysqlconnector.net/connection-options/): 
-- "Server=localhost;Port=3306;Username=trabajo_final_backend_user;Password=123456;Database=trabajo_final_backend;"

DROP DATABASE IF EXISTS trabajo_final_backend;
CREATE DATABASE trabajo_final_backend;
USE trabajo_final_backend;

DROP USER IF EXISTS trabajo_final_backend_user;
CREATE USER trabajo_final_backend_user IDENTIFIED BY '123456';