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
                //connection string: esta string, o el user y password, se deberia guardar y traer de una variable de entorno
                //(ahora no importa porque es una DB de practica).

                //-> documentacion de la connection string para MySQLConnector: https://mysqlconnector.net/connection-options/
                string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

                using (var connection = new MySqlConnection(connection_string))
                {
                    var rowsAffected = connection.Execute(query, new {
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
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        //READ (lista usuarios)
        public IEnumerable<Usuario> read_lista_usuarios()
        {
            string query = "SELECT * FROM Usuarios;";
            
            string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

            try
            {
                using (var connection = new MySqlConnection(connection_string))
                {
                    //Console.WriteLine(connection.State);

                    var lista = connection.Query<Usuario>(query).ToList();

                    return lista;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                //connection string: esta string, o el user y password, se deberia guardar y traer de una variable de entorno
                //(ahora no importa porque es una DB de practica).

                //-> documentacion de la connection string para MySQLConnector: https://mysqlconnector.net/connection-options/
                string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

                using (var connection = new MySqlConnection(connection_string))
                {
                    return connection.QueryFirstOrDefault<Usuario>(query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        //UPDATE
        public Usuario update_usuario(int id, string nombre, int edad)
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


                string query = "UPDATE Usuarios SET Edad = @Edad, Nombre = @Nombre WHERE Id=@Id;";
                string querySelect = "SELECT * FROM Usuarios WHERE Id=@Id;";

                
                string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

                using (var connection = new MySqlConnection(connection_string))
                {
                    Console.WriteLine($"Usuario id={id} antes del update:");
                    connection.QueryFirstOrDefault<Usuario>(querySelect, new
                    {
                        Id = id
                    }).mostrarDatos();

                    var rowsAffected = connection.Execute(query, new
                    {
                        Id = id,
                        Nombre = nombre,
                        Edad = edad
                    });

                    return connection.QueryFirstOrDefault<Usuario>(querySelect, new
                    {
                        Id = id
                    });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        //DELETE (borrado logico)
        public Usuario delete_borrado_logico_usuario(int id)
        {
            try
            {
                //validacion de inputs
                if (id < 0)
                {
                    throw new Exception($"{id} es una Id de usuario inválida.");
                }

                //formato de Fecha_baja en la DB:

                string Fecha_baja = DateTime.Today.ToString("yyyy-MM-dd");

                string query = $"UPDATE Usuarios SET Fecha_baja = @Fecha_baja WHERE Id=@Id;";
                string querySelect = "SELECT * FROM Usuarios WHERE Id=@Id;";

                
                string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";

                using (var connection = new MySqlConnection(connection_string))
                {
                    Console.WriteLine($"Usuario id={id} antes del borrado logico:");
                    connection.QueryFirstOrDefault<Usuario>(querySelect, new
                    {
                        Id = id
                    }).mostrarDatos();

                    var rowsAffected = connection.Execute(query, new
                    {
                        Id = id,
                        Fecha_baja = Fecha_baja
                    });

                    return connection.QueryFirstOrDefault<Usuario>(querySelect, new
                    {
                        Id = id
                    });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


    }


    

  
}
