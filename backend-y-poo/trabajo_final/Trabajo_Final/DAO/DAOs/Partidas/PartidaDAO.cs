using DAO.Connection;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Partidas
{
    public class PartidaDAO : IPartidaDAO
    {
        private MySqlConnection connection { get; set; }

        public PartidaDAO(string connectionString)
        {
            connection = new SingletonConnection(connectionString).Instance;
            if (connection.State != System.Data.ConnectionState.Open) connection.Open();
        }


        //READ partidas juez
        public async Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez)
        {
            string selectQuery = " SELECT * FROM partidas " +
                                 " WHERE id_ganador IS NULL " +
                                 " AND id_juez = @Id_juez; ";

            return await connection.QueryAsync<Partida>(selectQuery, new { Id_juez = id_juez });
        }


    }
}
