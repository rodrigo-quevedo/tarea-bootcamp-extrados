using DAO_biblioteca_de_cases.Entidades;
using DAO_biblioteca_de_cases.Singleton_Connection;
using MySqlConnector;

namespace DAO_biblioteca_de_cases
{
    public class UsuarioDAO
    {

        //singleton del objeto connection
        private MySqlConnection connection = UsuarioSingletonConnection.Instance;


        //CREATE
        public Usuario CrearUsuario()
        {

            return null;
        }


        //READ (leer un usuario por su mail)
        public Usuario BuscarUsuarioPorMail()
        {

            return null;
        }

        //UPDATE
        public Usuario ActualizarUsuario()
        {

            return null;
        }

    }
}
