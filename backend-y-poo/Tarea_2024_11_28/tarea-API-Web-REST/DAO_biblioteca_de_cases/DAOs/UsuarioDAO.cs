using DAO_biblioteca_de_cases.Entidades;
using DAO_biblioteca_de_cases.Singleton_Connections;
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
        public void CrearUsuario(Usuario usuario)
        {
            //el DAO directamente pone la password hasheada en la DB, el hasheo lo tiene que hacer un service del controller

            string query = "INSERT INTO Usuarios VALUES (@Mail , @Nombre, @Edad, @Username, @HashedPassword);";

            var rowsAffected = connection.Execute(query, new
            {
                Mail = usuario.mail,
                Nombre = usuario.nombre,
                Edad = usuario.edad,
                Username = usuario.username,
                HashedPassword = usuario.password
            });

        }


        //READ (leer un usuario por su mail)
        public Usuario BuscarUsuarioPorMail(string mail)
        {
            string querySelect = "SELECT * FROM Usuarios WHERE Mail=@Mail";
            return connection.QueryFirstOrDefault<Usuario>(querySelect, new
            {
                Mail = mail
            });

        }

        //UPDATE
        public void ActualizarUsuario(Usuario usuario)
        {
            //el DAO directamente pone la password hasheada en la DB, el hasheo lo tiene que hacer un service del controller

            string query = "UPDATE Usuarios SET Nombre = @Nombre, Edad = @Edad, username = @Username, password = @HashedPassword WHERE Mail=@Mail;";

            var rowsAffected = connection.Execute(query, new
            {
                Mail = usuario.mail,
                Nombre = usuario.nombre,
                Edad = usuario.edad,
                Username = usuario.username,
                HashedPassword = usuario.password
            });

        }
    }
}
