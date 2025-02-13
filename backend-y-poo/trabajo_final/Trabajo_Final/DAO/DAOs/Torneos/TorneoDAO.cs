using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
using DAO.Entidades.Cartas;
using DAO.Entidades.TorneoEntidades;
using DAO.Entidades.UsuarioEntidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Torneos
{
    public class TorneoDAO : ITorneoDAO
    {
        MySqlConnection connection;
        public TorneoDAO(string connectionString) { 
            this.connection = new SingletonConnection(connectionString).Instance;
        }


        //CREATE torneo, series_habilitadas, jueces_torneo
        public async Task<bool> CrearTorneo(
            int id_organizador, 
            DateTime fecha_hora_inicio, DateTime fecha_hora_fin, 
            int cantidad_rondas, 
            string pais,
            string fase,
            string[] series_habilitadas,
            int[] id_jueces
        )
        {
            bool exito = false;
            connection.Open();

            using (MySqlTransaction transaction = connection.BeginTransaction()) 
            {
                try
                {
                    //TABLE torneos
                    string insertAndReturnIdQuery =
                    " INSERT INTO torneos" +
                    "   (id_organizador, pais, " +
                    "   fecha_hora_inicio, fecha_hora_fin, " +
                    "   cantidad_rondas, fase) " +
                    " VALUES ( " +
                    "   @Id_organizador, @Pais, " +
                    "   @Fecha_hora_inicio, @Fecha_hora_fin, " +
                    "   @Cantidad_rondas, @Fase" +
                    " ); " +
                    "                             "+ 
                    " SELECT LAST_INSERT_ID();";

                    int id_torneo = await connection.ExecuteAsync(insertAndReturnIdQuery, new
                    {
                        Id_organizador = id_organizador,
                        Pais = pais,
                        Fecha_hora_inicio = fecha_hora_inicio,
                        Fecha_hora_fin = fecha_hora_fin,
                        Cantidad_rondas = cantidad_rondas,
                        Fase = fase
                    },
                    transaction: transaction);

                    //TABLE series_habilitadas
                    List<Serie_Habilitada> listaSeries
                        = series_habilitadas
                        .Select(
                            serie => new Serie_Habilitada()
                            {
                                Nombre_serie = serie,
                                Id_torneo = id_torneo
                            })
                        .ToList();

                    var insertSeriesQuery = 
                        @" INSERT INTO series_habilitadas (nombre_serie, id_torneo) " +
                         " VALUES (@Nombre_serie, @Id_torneo);";

                    await connection.ExecuteAsync(insertSeriesQuery, listaSeries,
                    transaction: transaction);


                    //TABLE jueces_torneo
                    List<Juez_Torneo> listaJueces
                        = id_jueces
                        .Select(
                            id_juez => new Juez_Torneo()
                            {
                                Id_torneo = id_torneo,
                                Id_juez = id_juez
                            })
                        .ToList();

                    var insertJuecesQuery =
                        @" INSERT INTO jueces_torneo (id_torneo, id_juez) " +
                         " VALUES (@Id_torneo, @Id_juez);";

                    await connection.ExecuteAsync(insertJuecesQuery, listaJueces,
                    transaction: transaction);

                    transaction.Commit();
                    exito = true;
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    throw ex;
                }


            }



            return exito;
        }

       
    }
}
