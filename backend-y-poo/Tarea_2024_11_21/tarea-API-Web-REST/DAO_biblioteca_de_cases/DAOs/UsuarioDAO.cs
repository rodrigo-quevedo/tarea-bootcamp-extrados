using DAO_biblioteca_de_cases.Entidades;
using DAO_biblioteca_de_cases.Singleton_Connection;
using Dapper;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace DAO_biblioteca_de_cases.DAOs
{
    public class UsuarioDAO
    {

        //singleton del objeto connection
        private MySqlConnection connection = UsuarioSingletonConnection.Instance;


        //CREATE
        public Usuario CrearUsuario(string mail, string nombre, int edad)
        {
            try
            {
                string query = "INSERT INTO Usuarios VALUES (@Mail , @Nombre, @Edad);";

                var rowsAffected = connection.Execute(query, new
                {
                    Mail = mail,
                    Nombre = nombre,
                    Edad = edad
                });

                string querySelect = "SELECT * FROM Usuarios WHERE Mail=@Mail";
                return connection.QueryFirstOrDefault<Usuario>(querySelect, new
                {
                    Mail = mail
                });
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        //READ (leer un usuario por su mail)
        public Usuario BuscarUsuarioPorMail(string mail)
        {
            try
            {
                string querySelect = "SELECT * FROM Usuarios WHERE Mail=@Mail";
                return connection.QueryFirstOrDefault<Usuario>(querySelect, new
                {
                    Mail = mail
                });
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //UPDATE
        public Usuario ActualizarUsuario(string mail, string nombre, int edad)
        {

            try
            {
                string query = "UPDATE Usuarios SET Nombre = @Nombre, Edad = @Edad WHERE Mail=@Mail;";
                string querySelect = "SELECT * FROM Usuarios WHERE Mail=@Mail;";

                Console.WriteLine($"Usuario con mail={mail} antes del update:");
                Usuario usuarioExistente = connection.QueryFirstOrDefault<Usuario>(querySelect, new
                {
                    Mail = mail
                });
                if (usuarioExistente == null)
                {
                    throw new Exception($"El usuario con mail '{mail}' no existe.");
                }
                usuarioExistente.mostrarDatos();

                var rowsAffected = connection.Execute(query, new
                {
                    Mail = mail,
                    Nombre = nombre,
                    Edad = edad
                });

                return connection.QueryFirstOrDefault<Usuario>(querySelect, new
                {
                    Mail = mail
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
