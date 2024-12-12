using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO_biblioteca_de_cases.Singleton_Connections
{
    public class UsuarioSingletonConnection
    {
        public static string connectionString { get; set; }
        private static MySqlConnection _instance = null;
        public static MySqlConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    new UsuarioSingletonConnection(connectionString);
                }

                return _instance;
            }
        }

        private UsuarioSingletonConnection(string connectionStringArg)
        {
            //connection string:
            //obtenerla del appsettings.json en el proyecto web
            //(es decir, configurarla en el program.cs y pasarla al DAO como parámetro)
            //(el DAO, a su vez, se lo pasa al SingletonConnection como parámetro)

            try
            {
                _instance = new MySqlConnection(connectionStringArg);
                _instance.Open();


                Console.WriteLine($"Estado de la db connection: {_instance.State}");

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


    }
}
