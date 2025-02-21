using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
using DAO.Entidades.Cartas;
using DAO.Entidades.Custom;
using DAO.Entidades.TorneoEntidades;
using DAO.Entidades.UsuarioEntidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            if (connection.State != System.Data.ConnectionState.Open) connection.Open();
        }


        //CREATE torneo, series_habilitadas, jueces_torneo
        public async Task<bool> CrearTorneo(
            int id_organizador, 
            DateTime fecha_hora_inicio, DateTime? fecha_hora_fin, 
            string horario_inicio, string horario_fin,
            int? cantidad_rondas, 
            string pais,
            string fase,
            string[] series_habilitadas,
            int[] id_jueces
        )
        {
            bool exito = false;
            
            using (MySqlTransaction transaction = connection.BeginTransaction()) 
            {
                try
                {
                    //TABLE torneos
                    string insertAndReturnIdQuery =
                    " INSERT INTO torneos" +
                    "   (id_organizador, pais, " +
                    "   fecha_hora_inicio, fecha_hora_fin," +
                    "   horario_diario_inicio, horario_diario_fin, " +
                    "   cantidad_rondas, fase) " +
                    " VALUES ( " +
                    "   @Id_organizador, @Pais, " +
                    "   @Fecha_hora_inicio, @Fecha_hora_fin, " +
                    "   @Horario_diario_inicio, @Horario_diario_fin, " +
                    "   @Cantidad_rondas, @Fase" +
                    " ); ";

                    await connection.ExecuteAsync(insertAndReturnIdQuery, new
                    {
                        Id_organizador = id_organizador,
                        Pais = pais,
                        Fecha_hora_inicio = fecha_hora_inicio,
                        Fecha_hora_fin = fecha_hora_fin,
                        Horario_diario_inicio = horario_inicio,
                        Horario_diario_fin = horario_fin,
                        Cantidad_rondas = cantidad_rondas,
                        Fase = fase
                    },
                    transaction: transaction);

                    Console.WriteLine("Torneo creado OK");
                    
                    string selectIdQuery = " SELECT LAST_INSERT_ID(); ";

                    int id_torneo = await connection.QueryFirstAsync<int>(selectIdQuery, null, transaction);

                    Console.WriteLine($"Last INSERT id: {id_torneo}");

                    //TABLE series_habilitadas
                    List <Serie_Habilitada> listaSeries
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
                        .Select( id_juez => 
                            new Juez_Torneo(){
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

        //UPDATE jueces del torneo
        public async Task<int> AgregarJuez(int id_torneo, int id_juez, string rol, string faseInvalida)
        {
            string insertQueryConValidacion = " INSERT INTO jueces_torneo " +
                                              " VALUES (           " +
                                              "        (SELECT id FROM torneos " +
                                              "         WHERE id = @Id_torneo" +
                                              "         AND fase != @Fase)," +
                                              "        (SELECT id FROM usuarios " +
                                              "         WHERE id=@Id_juez " +
                                              "         AND rol = @Rol)   " +
                                              " );                       ";

            
            try
            {
                return await connection.ExecuteAsync(insertQueryConValidacion, new
                {
                    Id_torneo = id_torneo,
                    Id_juez = id_juez,
                    Rol = rol,
                    Fase = faseInvalida
                });

            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("Column 'id_torneo' cannot be null"))
                    throw new InvalidInputException($"No existe ningún torneo en fase registro/torneo con ID [{id_torneo}]. " +
                                                    $"No se puede cambiar los jueces de torneos ya finalizados.");

                if (ex.Message.Contains("Column 'id_juez' cannot be null"))
                    throw new InvalidInputException($"El juez con ID [{id_juez}] no existe.");

                throw ex;
            }

        }

        public async Task<int> EliminarJuez(int id_torneo, int id_juez)
        {
            string deleteQuery = " DELETE FROM jueces_torneo " +
                                 " WHERE id_juez = @Id_juez AND id_torneo = @Id_torneo";

            
            
            int result = await connection.ExecuteAsync(deleteQuery, new
            {
                Id_torneo = id_torneo,
                Id_juez = id_juez
            });

            if (result == 0) throw new InvalidInputException($"El juez con id [{id_juez}] no existe en el torneo [{id_torneo}]");

            return result;
        }


        //READ torneos
        public async Task<Torneo> BuscarTorneo(Torneo busqueda)
        {
            string selectQuery;


            if (busqueda.Id != default)
            {
                selectQuery = " SELECT * FROM torneos " +
                              " WHERE id = @Id;";

                return await connection.QueryFirstOrDefaultAsync<Torneo>(selectQuery, new
                {
                    Id = busqueda.Id
                });
            }


            return null;//(Torneo busqueda) sin ningun campo valido para buscar
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneos(Torneo busqueda)
        {
            string selectQuery;


            if (busqueda.Fase != default)
            {
                 selectQuery = " SELECT * FROM torneos " +
                               " WHERE fase = @Fase;";

                return await connection.QueryAsync<Torneo>(selectQuery, new
                {
                    Fase = busqueda.Fase
                });
            }


            return null;//(Torneo busqueda) sin ningun campo valido para buscar
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneos(string[] fases)
        {
            IEnumerable<Torneo> result = Enumerable.Empty<Torneo>();
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                string selectQuery = " SELECT * FROM torneos " +
                                     " WHERE fase = @Fase; ";

                try
                {
                    foreach (string fase in fases)
                    {
                        IEnumerable<Torneo> queryResult =
                            await connection.QueryAsync<Torneo>(
                                selectQuery,
                                new { Fase = fase },
                                transaction);

                        result = result.Concat(queryResult);
                    }

                    transaction.Commit();
                }
                catch (Exception ex) 
                {
                    transaction.Rollback();
                    throw ex;
                }

                return result;
            }
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneosLlenos(string faseInscripcion)
        {
            string selectQuery = " SELECT * FROM torneos " +
                                 " WHERE " +
                                 "      POWER(2, cantidad_rondas) " +
                                 "      <=" +
                                 "      (SELECT COUNT(*) FROM jugadores_inscriptos" +
                                 "      WHERE torneos.id = jugadores_inscriptos.id_torneo); ";

            return await connection.QueryAsync<Torneo>(selectQuery);

        }

        public async Task<IEnumerable<Serie_Habilitada>> BuscarSeriesDeTorneos(IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM series_habilitadas " +
                                 " WHERE id_torneo = @Id; "; //-->@Id mapea el Id de Torneo

            IEnumerable<Serie_Habilitada> result = Enumerable.Empty<Serie_Habilitada>();

            using (MySqlTransaction transaction = connection.BeginTransaction()) {

                try
                {
                    foreach (Torneo torneo in torneos) {
                        IEnumerable<Serie_Habilitada> queryResult =
                            await connection.QueryAsync<Serie_Habilitada>(
                                selectQuery, 
                                new { Id = torneo.Id }, 
                                transaction
                            );

                        result = result.Concat(queryResult);

                    }

                    //result = result.Concat(await connection.QueryAsync<Serie_Habilitada>(
                    //    selectQuery,
                    //    torneos.ToList(),
                    //    transaction)
                    //);

                    transaction.Commit();

                }
                catch (Exception ex) { 
                    transaction.Rollback(); 
                    throw ex;
                }

                return result;
            }

        }

        public async Task<IEnumerable<Juez_Torneo>> BuscarJuecesDeTorneos(IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM jueces_torneo " +
                                 " WHERE id_torneo = @Id; ";//-->@Id mapea el Id de Torneo

            IEnumerable<Juez_Torneo> result = Enumerable.Empty<Juez_Torneo>();

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {

                try
                {
                    foreach (Torneo torneo in torneos)
                    {
                        IEnumerable<Juez_Torneo> queryResult =
                            await connection.QueryAsync<Juez_Torneo>(
                                selectQuery,
                                new { Id = torneo.Id },
                                transaction
                            );

                        result = result.Concat(queryResult);

                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

                return result;
            }

        }

        public async Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresInscriptos(IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM jugadores_inscriptos " +
                                 " WHERE id_torneo = @Id_torneo; ";//-->@Id mapea el Id de Torneo

            IEnumerable<Jugador_Inscripto> result = Enumerable.Empty<Jugador_Inscripto>();

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (Torneo torneo in torneos)
                    {
                        IEnumerable<Jugador_Inscripto> queryResult =
                            await connection.QueryAsync<Jugador_Inscripto>(
                                selectQuery,
                                new { Id_torneo = torneo.Id },
                                transaction
                            );

                        result = result.Concat(queryResult);

                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }
            
            return result;
        }

        public async Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresInscriptos(int id_torneo, int max_cantidad_jugadores)
        {
            string selectQuery = " SELECT * FROM jugadores_inscriptos " +
                                 " WHERE id_torneo = @Id_torneo " +
                                 " ORDER BY orden DESC " +
                                 " LIMIT @Max_cantidad_jugadores; ";

            return await connection.QueryAsync<Jugador_Inscripto>(
                selectQuery,
                new {
                    Id_torneo = id_torneo,
                    Max_cantidad_jugadores = max_cantidad_jugadores
                });

        }

        public async Task<bool> InscribirJugador(
            int id_jugador, string rol_jugador,
            int id_torneo, string fase_inscripcion,
            int[] id_cartas_mazo)
        {
            bool exito = false;

            using (MySqlTransaction transaction = connection.BeginTransaction()) 
            {
                int ultima_id_carta_insert = id_cartas_mazo[0];

                try
                {

                    //chequear que el torneo no este lleno
                    string hayLugar_selectQuery =
                        " SELECT ( " +
                        "   POWER(2, (SELECT cantidad_rondas FROM torneos " +
                        "            WHERE id = @Id_torneo) " +
                        "   )" +
                        "   > " +
                        "  (SELECT count_jugadores " +
                        "      FROM ( " +
                        "        SELECT COUNT(*) AS count_jugadores" +
                        "        FROM jugadores_inscriptos" +
                        "        WHERE id_torneo = @Id_torneo" +
                        "      ) AS count_table " +
                        "   )" +
                        " ); ";

                    bool hayLugar = await connection.QueryFirstOrDefaultAsync<bool>(
                        hayLugar_selectQuery,
                        new { Id_torneo = id_torneo },
                        transaction);

                    if (!hayLugar) throw new TorneoLlenoException(); 

                    //insert jugadores_inscriptos
                    string jugadorInsertQuery =
                        " INSERT INTO jugadores_inscriptos "                        +
                        "   (id_jugador, id_torneo, aceptado) "                     +
                        " VALUES ("                                                 +
                        "   (SELECT id FROM usuarios "                              +
                        "       WHERE id=@Id_jugador AND rol = @Rol), "             +
                        "   (SELECT id FROM torneos "                               +
                        "       WHERE "                                             +
                        "           id=@Id_torneo "                                 +
                        "       AND "                                               +
                        "           fase = @Fase"                                   +  
                        //"       AND "                                               +
                        //"           ( POWER(2, (SELECT cantidad_rondas FROM torneos "+
                        //"                       WHERE id=@Id_torneo) ) "             +
                        //"           >   "                                            +
                        //"           (SELECT count_jugadores FROM  "                  +
                        //"               (SELECT COUNT(*) AS count_jugadores   "      +
                        //"               FROM jugadores_inscriptos "                  +
                        //"               WHERE id_torneo = @Id_torneo"                +
                        //"               ) AS count_table )"                          +
                        //"           ) "                                              +
                        "   ), "                                                     +
                        "   @Aceptado); ";

                    await connection.ExecuteAsync(
                        jugadorInsertQuery,
                        new {
                            Id_jugador = id_jugador, Rol = rol_jugador,
                            Id_torneo = id_torneo, Fase = fase_inscripcion,
                            Aceptado = false
                        },
                        transaction);

                    Console.WriteLine("Jugador inscripto OK");

                    //insert cartas_del_mazo

                    string mazoInsertQuery =
                        " INSERT INTO cartas_del_mazo (id_jugador, id_carta, id_torneo) " +
                        " VALUES ( " +
                        "   (SELECT id FROM usuarios " +
                        "       WHERE id=@Id_jugador AND rol=@Rol), " +
                        "   (SELECT id_carta FROM cartas_coleccionadas " +
                        "       WHERE " +
                        "           id_carta=@Id_carta " +
                        "           AND " +
                        "           id_jugador=@Id_jugador" +
                        //"           AND " +
                        //"           id_carta IN (SELECT id_carta)" +
                        "       )," +
                        "   (SELECT id FROM torneos " +
                        "       WHERE id=@Id_torneo AND fase=@Fase)" +
                        "   ); ";

                    
                    foreach (int id_carta in id_cartas_mazo) {       
                        
                        ultima_id_carta_insert = id_carta;

                        await connection.ExecuteAsync(
                            mazoInsertQuery,
                            new {
                                Id_jugador = id_jugador,
                                Rol = rol_jugador,
                                Id_carta = id_carta,
                                Id_torneo = id_torneo,
                                Fase = fase_inscripcion
                            },
                            transaction
                        );
                    }

                    transaction.Commit();
                    Console.WriteLine("MAZO del jugador OK");
                    exito = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    //jugador no existe
                    if (ex.Message.Contains("Column 'id_jugador' cannot be null"))
                        throw new InvalidInputException($"No existe ningun jugador con id [{id_jugador}].");
                    //torneo no existe
                    if (ex.Message.Contains("Column 'id_torneo' cannot be null"))
                        throw new InvalidInputException($"No existe ningun torneo en fase '{fase_inscripcion}' con id [{id_torneo}]. O puede que el torneo ya esté lleno.");
                    ////carta no existe en coleccion (coleccion ya comprobó que existe)
                   if (ex.Message.Contains("Column 'id_carta' cannot be null"))
                        throw new InvalidInputException($"No existe ninguna carta con id [{ultima_id_carta_insert}] en la coleccion del jugador id [{id_jugador}].");
                    //jugador ya inscripto
                    if (ex.Message.Contains("Duplicate entry") && ex.Message.Contains("for key 'jugadores_inscriptos.PRIMARY'"))
                        throw new InvalidInputException($"El jugador [{id_jugador}] ya está inscripto en el torneo [{id_torneo}]");
                    
                    
                    throw ex;
                }
            }

            return exito;
        }

        public async Task<bool> IniciarTorneo(
            string faseTorneo,
            int id_torneo,
            IList<int> id_jugadores_aceptados,
            IList<DatosPartidaDTO> partidas_primera_ronda)
        {

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    //UPDATE torneo
                    string updateQuery = " UPDATE torneos " +
                                         " SET fase = @Fase " +
                                         " WHERE id = @Id; ";

                    int torneo_result = await connection.ExecuteAsync(
                        updateQuery, 
                        new {
                            Fase = faseTorneo,
                            Id = id_torneo
                        }, 
                        transaction);

                    if (torneo_result == 0) throw new Exception("No se pudo actualizar fase del torneo.");

                    //UPDATE jugadores_inscriptos
                    string jugadores_updateQuery =
                        " UPDATE jugadores_inscriptos " +
                        " SET aceptado = @Aceptado " +
                        " WHERE id_jugador = @Id_jugador; ";

                    foreach(int id_jugador in id_jugadores_aceptados)
                    {
                        int jugador_result = await connection.ExecuteAsync(
                            jugadores_updateQuery,
                            new { Aceptado = true, Id_jugador = id_jugador },
                            transaction);

                        if (jugador_result == 0) throw new Exception($"No se pudo actualizar estado 'aceptado' del jugador {id_jugador}");
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


                    await connection.ExecuteAsync(
                        insertQuery,
                        partidas_primera_ronda,
                        transaction);

                    transaction.Commit();
                }
                catch (Exception ex) 
                {
                    transaction.Rollback();
                    throw ex;
                }

                return true;
            }
        }


    }
}
