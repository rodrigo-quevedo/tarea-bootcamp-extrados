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
        private MySqlConnection connection;


        public UsuarioDAO(string connectionString)
        {
            //setear connection string
            UsuarioSingletonConnection.connectionString = connectionString;
            
            //singleton del objeto connection
            connection = UsuarioSingletonConnection.Instance;
        }


        //-------crud--------:
        //CREATE
        public void CrearUsuario(Usuario usuario)
        {
            //el DAO directamente pone la password hasheada en la DB, el hasheo lo tiene que hacer un service del controller

            string query = "INSERT INTO Usuarios " +
                "VALUES (@Mail , @Nombre, @Edad, " +
                "@Username, @HashedPassword, @Role, @RefreshToken);";

            var rowsAffected = connection.Execute(query, new
            {
                Mail = usuario.mail,
                Nombre = usuario.nombre,
                Edad = usuario.edad,
                Username = usuario.username,
                HashedPassword = usuario.password,
                Role = usuario.role,
                RefreshToken = usuario.refresh_token
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

        //leer usuario por username (tambien es unique)
        public Usuario BuscarUsuarioPorUsername(string username)
        {
            string querySelect = "SELECT * FROM Usuarios WHERE username=@Username";
            return connection.QueryFirstOrDefault<Usuario>(querySelect, new
            {
                Username = username 
            });

        }


        //UPDATE (refreshtoken)
        public void AsignarRefreshTokenByUsername(string username, string refreshToken)
        {
            string updateQuery = "UPDATE Usuarios SET refresh_token = @Refresh_token WHERE username = @Username";

            connection.Execute(updateQuery, new
            {
                Refresh_token = refreshToken,
                Username = username
            });

            return;
        }

        //UPDATE
        public void ActualizarUsuario(Usuario usuario)
        {
            //el DAO directamente pone la password hasheada en la DB, el hasheo lo tiene que hacer un service del controller

            string query = "UPDATE Usuarios SET Nombre = @Nombre, Edad = @Edad, username = @Username, password = @HashedPassword, role = @Role WHERE Mail=@Mail;";

            var rowsAffected = connection.Execute(query, new
            {
                Mail = usuario.mail,
                Nombre = usuario.nombre,
                Edad = usuario.edad,
                Username = usuario.username,
                HashedPassword = usuario.password,
                Role = usuario.role
            });

        }
    }
}
