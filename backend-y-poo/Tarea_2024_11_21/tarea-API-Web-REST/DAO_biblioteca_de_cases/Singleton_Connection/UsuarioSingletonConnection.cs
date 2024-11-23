using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO_biblioteca_de_cases.Singleton_Connection
{
    public class UsuarioSingletonConnection
    {
        private static MySqlConnection _instance = null;

        private UsuarioSingletonConnection()
        {
            //connection string:
            //"Server=localhost;Port=3306;Username=tarea_apiWebREST_user;Password=123456;Database=tarea_apiWebREST;"

            try
            {
               _instance = new MySqlConnection("Server=localhost;Port=3306;Username=tarea_apiWebREST_user;Password=123456;Database=tarea_apiWebREST;");
               _instance.Open();
                
                
                Console.WriteLine("Estado de la db connection:", _instance.State);
            }

            catch (Exception ex){ 
                Console.WriteLine(ex.ToString());
            }

        }

        public static MySqlConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    new UsuarioSingletonConnection();
                }

                return _instance;
            }
        }

    }
}
