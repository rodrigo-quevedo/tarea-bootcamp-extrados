using DAO_biblioteca_de_cases.Entidades;
using DAO_biblioteca_de_cases.Singleton_Connections;
using Dapper;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace DAO_biblioteca_de_cases.DAOs
{
    public class LibroDAO
    {
        private MySqlConnection connection;


        public LibroDAO(string connectionString)
        {
            //setear connection string
            UsuarioSingletonConnection.connectionString = connectionString;
            
            //singleton del objeto connection
            connection = UsuarioSingletonConnection.Instance;
        }


        //-------crud--------:

        //READ (leer un libro por su id)
        public Libro BuscarLibroPorId(int id)
        {
            string querySelect = "SELECT * FROM Libros WHERE id=@Id";
            return connection.QueryFirstOrDefault<Libro>(querySelect, new
            {
                Id = id
            });

        }

        //UPDATE (prestar libro: asignar valor a fechaHora_prestamo, fechaHora_vencimiento y username_prestatario )
        public int PrestarLibro(int idLibro, DateTime fechaHora_prestamo, DateTime fechaHora_vencimiento,string username_prestatario)
        {
            string queryUpdate = "" +
                "UPDATE Libros " +
                "SET " +
                "   fechaHora_prestamo = @FechaHora_prestamo," +
                "   fechaHora_vencimiento = @FechaHora_vencimiento," +
                "   username_prestatario = @Username_prestatario " +
                "WHERE id = @Id" +
            "";

            return connection.Execute(queryUpdate, new
            {
                FechaHora_prestamo = fechaHora_prestamo,
                FechaHora_vencimiento = fechaHora_vencimiento,
                Username_prestatario = username_prestatario,
                Id = idLibro
            });

        }


     
    }
}
