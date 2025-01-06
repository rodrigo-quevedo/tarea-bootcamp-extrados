using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this._connectionString = connectionString;
        }


        // Singleton
        public MySqlConnection Instance { 
            get { 
                if (_connection == null)
                {
                    this._connection = new MySqlConnection(this._connectionString);
                    return _connection;
                }
                return this._connection; 
            } 
        }
    }
}
