using Configuration;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Connection
{
    public class SingletonConnection
    {
        // Connection
        private MySqlConnection _connection;
        private string _connectionString;

  
        public SingletonConnection(string connectionString)
        {
            _connectionString = connectionString;
        }
            

        // Singleton
        public MySqlConnection Instance { 
            get 
            { 
                if (_connection == null)
                {
                    _connection = new MySqlConnection(_connectionString);
                    Console.WriteLine($"Nueva conexion creada. Connection string:{_connectionString}");
                    return _connection;
                }

                Console.WriteLine($"Ya hay una conexion existente. Connection string:{ _connectionString}");
                return _connection; 
            } 
        }
    }
}
