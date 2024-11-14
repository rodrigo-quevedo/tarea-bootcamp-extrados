using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using tareaDAO_libreria_de_clases.Entidades;

namespace tareaDAO_libreria_de_clases.DAO
{
    public class UsuarioDAO
    {
        //CREATE
        public Usuario create_usuario(int id, string nombre, int edad)
        {
            try
            {
                //validacion de inputs
                if (id < 0)
                {
                    throw new Exception($"{id} es una Id de usuario inválida.");
                }

                if (edad < 0)
                {
                    throw new Exception($"{edad} es una Edad de usuario inválida.");
                }

                string nombre_pattern = @"^[a-zA-ZñÑ ]{0,50}$";
                if (Regex.IsMatch(nombre, nombre_pattern) == false)
                {
                    throw new Exception($"{nombre} es un nombre de usuario inválido.");
                }

                string query = "INSERT INTO Usuarios VALUES (@Id , @Nombre, @Edad, null);";
                //esta string, o el user y password, se deberia guardar y traer de una variable de entorno
                //(ahora no importa porque es una DB de practica).

                //-> documentacion de la connection string para MySQLConnector: https://mysqlconnector.net/connection-options/
                string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

                using (var connection = new MySqlConnection(connection_string))
                {
                    var rowsAffected =  connection.Execute(query, new { 
                        Id = id, 
                        Nombre = nombre,
                        Edad = edad 
                    });

                    string querySelect = "SELECT * FROM Usuarios WHERE Id=@Id;";
                    return connection.QueryFirstOrDefault<Usuario>(querySelect, new {
                        Id = id
                    });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
    

        //READ (lista usuarios)
        public IEnumerable<Usuario> read_lista_usuarios()
        {
            string query = "SELECT * FROM Usuarios;";
            //esta string, o el user y password, se deberia guardar y traer de una variable de entorno
            //(ahora no importa porque es una DB de practica).

            //-> documentacion de la connection string para MySQLConnector: https://mysqlconnector.net/connection-options/
            string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";
           
            try
            {
                using (var connection = new MySqlConnection(connection_string))
                {
                    //Console.WriteLine(connection.State);
                    
                    var lista =  connection.Query<Usuario>(query).ToList();

                    return lista;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        //READ (info de un usuario a partir de su Id)
        public Usuario read_usuario_by_id(int id)
        {
            try
            {
                //validacion de input
                if (id < 0)
                {
                    throw new Exception($"{id} es una Id de usuario inválida.");
                }


                string query = "SELECT * FROM Usuarios WHERE Id=@Id;";
                //esta string, o el user y password, se deberia guardar y traer de una variable de entorno
                //(ahora no importa porque es una DB de practica).

                //-> documentacion de la connection string para MySQLConnector: https://mysqlconnector.net/connection-options/
                string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

                using (var connection = new MySqlConnection(connection_string))
                {
                    return connection.QueryFirstOrDefault<Usuario>(query,  new { Id = id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }


        //UPDATE


        //DELETE (borrado logico)

    }
}
