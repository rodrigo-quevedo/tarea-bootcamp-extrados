using DAO.Connection;
using DAO.Entidades.Custom;
using DAO.Entidades.Custom.Partida_CantidadRondas;
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

        public async Task<Partida_CantidadRondasDTO> BuscarDatosParaOficializar(Partida partida)
        {
            string selectQuery;

            if (partida.Id != default && partida.Id_juez != default) 
            {
                selectQuery = " SELECT partidas.*, cantidad_rondas " +
                              " FROM partidas " +
                              " JOIN torneos " +
                              " ON partidas.id_torneo = torneos.id " +
                              " WHERE partidas.id = @Id_partida " +
                              " AND partidas.id_juez = @Id_juez ";

                return await connection.QueryFirstOrDefaultAsync<Partida_CantidadRondasDTO>(selectQuery, new {
                    Id_partida = partida.Id,
                    Id_juez = partida.Id_juez
                });

            }

            return null;
        }

        public async Task<bool> VerificarUltimaPartidaDeRonda(
            int id_partida,
            int id_torneo,
            int ronda)
        {
            string selectQuery = " SELECT COUNT(*) FROM partidas " +
                                 " WHERE id_torneo = @Id_torneo " +
                                 " AND ronda = @Ronda " +
                                 " AND id_ganador IS NULL " +
                                 " AND id != @Id_partida; ";

            int? partidasRestantes = 
                await connection.QueryFirstOrDefaultAsync<int?>(selectQuery, new { 
                    Id_torneo = id_torneo,
                    Ronda = ronda,
                    Id_partida = id_partida
                });

            Console.WriteLine($"Partida [{id_partida}], ronda [{ronda}] | Partidas restantes: [{partidasRestantes}]");

            if (partidasRestantes == null) throw new Exception("No se pudo obtener la cantidad de partidas restantes.");

            if (partidasRestantes == 0) return true;
            else return false;
        }

        public async Task<bool> OficializarResultado(int id_partida, int id_ganador, int? id_descalificado)
        {
            string updateQuery;
            int result = 0;

            try
            {
                if (id_descalificado == null)
                {
                    updateQuery = " UPDATE partidas " +
                                  " SET id_ganador = @Id_ganador " +
                                  " WHERE id = @Id_partida";

                    result = await connection.ExecuteAsync(updateQuery, new {
                        Id_partida = id_partida,
                        Id_ganador = id_ganador
                    });

                    if (result == 0) throw new Exception($"No se pudo guardar el resultado de la partida [{id_partida}]");
                
                    return true;
                }


                updateQuery = " UPDATE partidas " +
                              " SET " +
                              "     id_ganador = @Id_ganador," +
                              "     id_descalificado = @Id_descalificado " +
                              " WHERE id = @Id_partida";

                result = await connection.ExecuteAsync(updateQuery, new
                {
                    Id_partida = id_partida,
                    Id_ganador = id_ganador,
                    Id_descalificado = id_descalificado
                });

                if (result == 0) throw new Exception($"No se pudo guardar el resultado de la partida [{id_partida}]");


            }
            catch (Exception ex) 
            {
                //id_ganador o id_descalificado no son jugador_1 o jugador_2 (salta el
                //CHECK de la tabla)


                throw ex;
            }
            return true;
        }


        public async Task<bool> OficializarFinal(
            int id_partida,
            int id_ganador, 
            int? id_descalificado,
            int id_torneo,
            string faseFinalizado)
        {
            string updatePartidaQuery;
            int updatePartidaResult = 0;            

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    //UPDATE partida
                    if (id_descalificado == null)
                    {
                        updatePartidaQuery = " UPDATE partidas " +
                                             " SET id_ganador = @Id_ganador " +
                                             " WHERE id = @Id_partida";

                        updatePartidaResult = await connection.ExecuteAsync(
                            updatePartidaQuery, 
                            new {
                                Id_partida = id_partida,
                                Id_ganador = id_ganador },
                            transaction);

                        if (updatePartidaResult == 0) throw new Exception($"No se pudo guardar el resultado de la partida [{id_partida}]");

                    }
                    else
                    {
                        updatePartidaQuery = " UPDATE partidas " +
                                             " SET " +
                                             "     id_ganador = @Id_ganador," +
                                             "     id_descalificado = @Id_descalificado " +
                                             " WHERE id = @Id_partida";

                        updatePartidaResult = await connection.ExecuteAsync(
                            updatePartidaQuery, 
                            new {
                                Id_partida = id_partida,
                                Id_ganador = id_ganador,
                                Id_descalificado = id_descalificado},
                            transaction);

                        if (updatePartidaResult == 0) throw new Exception($"No se pudo guardar el resultado de la partida [{id_partida}]");
                    }

                    //UPDATE torneo
                    string updateTorneoQuery =
                        " UPDATE torneos SET fase = @Fase " +
                        " WHERE id = @Id_torneo; ";

                    int updateTorneoResult = await connection.ExecuteAsync(
                        updateTorneoQuery,
                        new {
                            Id_torneo = id_torneo,
                            Fase = faseFinalizado
                        },
                        transaction);

                    if (updateTorneoResult == 0) throw new Exception($"No se pudo actualizar la fase del torneo [{id_torneo}]");


                    transaction.Commit();
                    return true;
                }
                catch (Exception ex) 
                { 
                    transaction.Rollback();

                    //id_ganador o id_descalificado no son jugador_1 o jugador_2 (salta el
                    //CHECK de la tabla)


                    throw ex;
                }
            }
        }

        public async Task<bool> OficializarUltimaPartidaDeRonda(
            int id_partida, 
            int id_ganador, int? id_descalificado, 
            IEnumerable<InsertPartidaDTO> partidas)
        {
            string updatePartidaQuery;
            int updatePartidaResult = 0;


            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    //UPDATE partida
                    if (id_descalificado == null)
                    {
                        updatePartidaQuery = " UPDATE partidas " +
                                             " SET id_ganador = @Id_ganador " +
                                             " WHERE id = @Id_partida";

                        updatePartidaResult = await connection.ExecuteAsync(
                            updatePartidaQuery,
                            new
                            {
                                Id_partida = id_partida,
                                Id_ganador = id_ganador
                            },
                            transaction);

                        if (updatePartidaResult == 0) throw new Exception($"No se pudo guardar el resultado de la partida [{id_partida}]");

                    }
                    else
                    {
                        updatePartidaQuery = " UPDATE partidas " +
                                             " SET " +
                                             "     id_ganador = @Id_ganador," +
                                             "     id_descalificado = @Id_descalificado " +
                                             " WHERE id = @Id_partida";

                        updatePartidaResult = await connection.ExecuteAsync(
                            updatePartidaQuery,
                            new
                            {
                                Id_partida = id_partida,
                                Id_ganador = id_ganador,
                                Id_descalificado = id_descalificado
                            },
                            transaction);

                        if (updatePartidaResult == 0) throw new Exception($"No se pudo guardar el resultado de la partida [{id_partida}]");
                    }

                    //INSERT partidas
                    string insertQuery =
                         " INSERT INTO partidas ( " +
                         "   id_torneo, ronda, fecha_hora_inicio, " +
                         "   fecha_hora_fin, id_jugador_1, id_jugador_2, " +
                         "   id_juez) " +
                         " VALUES (" +
                         "   @Id_torneo, @Ronda, @Fecha_hora_inicio, " +
                         "   @Fecha_hora_fin, @Id_jugador_1, @Id_jugador_2," +
                         "   @Id_juez); ";


                    int result = await connection.ExecuteAsync(
                        insertQuery,
                        partidas,
                        transaction);

                    if (partidas.Count() != result) throw new Exception("No se pudieron guardar todas las partidas de la siguiente ronda. Se cancela esta oficializacion.");

                    Console.WriteLine($"Cantidad de partidas creadas: {result}");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    //id_ganador o id_descalificado no son jugador_1 o jugador_2 (salta el
                    //CHECK de la tabla)


                    throw ex;
                }


            }
                return true;
        }


        public async Task<IEnumerable<Partida>> BuscarJugadoresGanadores(
            int id_torneo, 
            int ronda,
            int cantidad_ganadores)
        {
            string selectQuery = " SELECT * FROM partidas " +
                                 " WHERE id_torneo = @Id_torneo " +
                                 " AND ronda = @Ronda " +
                                 " AND id_ganador IS NOT NULL" +
                                 " ORDER BY fecha_hora_fin; ";

            IEnumerable<Partida> result = await connection.QueryAsync<Partida>(
                selectQuery,
                new {
                    Id_torneo = id_torneo,
                    Ronda = ronda
                });

            if (result.Count() != cantidad_ganadores) throw new Exception($"Error al buscar ganadores de la ronda {ronda}, torneo [{id_torneo}]. Cantidad actual: {result.Count()}");

            return result;
        }



    }
}
