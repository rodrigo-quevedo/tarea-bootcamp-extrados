using DAO_biblioteca_de_cases.Entidades;
using DAO_biblioteca_de_cases.Singleton_Connection;
using Dapper;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace DAO_biblioteca_de_cases
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
                //validacion de inputs 
                if (edad <= 14)
                {
                    throw new Exception($"{edad} es una Edad de usuario inválida. " +
                        $"\n1. Debe ser mayor a 14.");
                }

                string nombre_pattern = @"^[a-zA-ZñÑ ]{1,50}$";
                if (Regex.IsMatch(nombre, nombre_pattern) == false)
                {
                    throw new Exception($"\n{nombre} es un nombre de usuario inválido. " +
                        $"\n1. Solo se permiten letras mayusculas, minusculas y espacio. " +
                        $"\n2. Debe tener un minimo de 1 y maximo de 50 caracteres.");
                }

                string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
                if (Regex.IsMatch(mail, mail_pattern) == false)
                {
                    throw new Exception($"\n{mail} es un email inválido. " +
                        $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                        $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                        $"\n3. Solo se permiten cuentas @gmail.com");
                }

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
                string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
                if (Regex.IsMatch(mail, mail_pattern) == false)
                {
                    throw new Exception($"\n{mail} es un email inválido. " +
                        $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                        $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                        $"\n3. Solo se permiten cuentas @gmail.com");
                }

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
                //validacion de inputs
                if (edad <= 14)
                {
                    throw new Exception($"{edad} es una Edad de usuario inválida. " +
                        $"\n1. Debe ser mayor a 14.");
                }

                string nombre_pattern = @"^[a-zA-ZñÑ ]{1,50}$";
                if (Regex.IsMatch(nombre, nombre_pattern) == false)
                {
                    throw new Exception($"\n{nombre} es un nombre de usuario inválido. " +
                        $"\n1. Solo se permiten letras mayusculas, minusculas y espacio. " +
                        $"\n2. Debe tener un minimo de 1 y maximo de 50 caracteres.");
                }

                string mail_pattern = @"^[a-zA-ZñÑ0-9]{1,20}@gmail.com$";
                if (Regex.IsMatch(mail, mail_pattern) == false)
                {
                    throw new Exception($"\n{mail} es un email inválido. " +
                        $"\n1. No se permiten caracteres espaciales antes del '@'. " +
                        $"\n2. Solo se permiten letras y numeros antes del '@'. " +
                        $"\n3. Solo se permiten cuentas @gmail.com");
                }


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
