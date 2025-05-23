﻿using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
using DAO.DTOs_en_DAOs.Ganador_Torneo;
using DAO.DTOs_en_DAOs.InsertPartidas;
using DAO.DTOs_en_DAOs.JuezTorneo;
using DAO.Entidades.Cartas;
using DAO.Entidades.TorneoEntidades;
using DAO.Entidades.UsuarioEntidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            int max_cantidad_rondas, 
            string pais,
            string fase,
            string[] series_habilitadas,
            int[] id_jueces,
            string rolJuez
        )
        {
            bool exito = false;

            string ultimaSerieInsert = series_habilitadas[0];
            int ultimoJuezInsert = id_jueces[0];


            using (MySqlTransaction transaction = connection.BeginTransaction()) 
            {
                try
                {
                    //INSERT torneos
                    string insertAndReturnIdQuery =
                    " INSERT INTO torneos" +
                    "   (id_organizador, pais, " +
                    "   fecha_hora_inicio, fecha_hora_fin," +
                    "   horario_diario_inicio, horario_diario_fin, " +
                    "   cantidad_rondas, fase, max_cantidad_rondas) " +
                    " VALUES ( " +
                    "   @Id_organizador, @Pais, " +
                    "   @Fecha_hora_inicio, @Fecha_hora_fin, " +
                    "   @Horario_diario_inicio, @Horario_diario_fin, " +
                    "   @Cantidad_rondas, @Fase, @Max_cantidad_rondas" +
                    " ); ";

                    await connection.ExecuteAsync(insertAndReturnIdQuery, new
                    {
                        Id_organizador = id_organizador,
                        Pais = pais,
                        Fecha_hora_inicio = fecha_hora_inicio,
                        Fecha_hora_fin = fecha_hora_fin,
                        Horario_diario_inicio = horario_inicio,
                        Horario_diario_fin = horario_fin,
                        Cantidad_rondas = 0,
                        Fase = fase,
                        Max_cantidad_rondas = max_cantidad_rondas
                    },
                    transaction);

                    Console.WriteLine("Torneo creado OK");
                    
                    string selectIdQuery = " SELECT LAST_INSERT_ID(); ";

                    int id_torneo = await connection.QueryFirstAsync<int>(selectIdQuery, null, transaction);

                    Console.WriteLine($"Last INSERT id: {id_torneo}");

                    //INSERT series_habilitadas
                    List <Serie_Habilitada> listaSeries = 
                        series_habilitadas.Select( serie => 
                            new Serie_Habilitada() {
                                Nombre_serie = serie,
                                Id_torneo = id_torneo 
                            }
                        )
                        .ToList();

                    var insertSeriesQuery = 
                        @" INSERT INTO series_habilitadas (nombre_serie, id_torneo) " +
                         " VALUES ( " +
                         //"  @Nobre_serie, " +
                         "  (SELECT nombre from series " +
                         "   WHERE nombre = @Nombre_serie), " +
                         "  @Id_torneo" +
                         ");";

                    ultimaSerieInsert = listaSeries[0].Nombre_serie;
                    int serieResult = 0;

                    foreach (Serie_Habilitada serie in listaSeries)
                    {
                        ultimaSerieInsert = serie.Nombre_serie;

                        serieResult = await connection.ExecuteAsync(
                            insertSeriesQuery, 
                            serie,
                            transaction);

                        if (serieResult == 0) throw new Exception($"No se pudo agregar la serie [{ultimaSerieInsert}].");
                    }


                    //INSERT jueces_torneo
                    List<JuezTorneoDTO> listaJueces = 
                        id_jueces
                        .Select( id_juez => 
                            new JuezTorneoDTO(){
                                Id_torneo = id_torneo,
                                Id_juez = id_juez,
                                Activo = true,
                                Rol = rolJuez
                            })
                        .ToList();

                    var insertJuecesQuery =
                        @" INSERT INTO jueces_torneo (id_torneo, id_juez) " +
                         " VALUES (" +
                         "      @Id_torneo, " +
                         "      (SELECT id FROM usuarios " +
                         "       WHERE id = @Id_juez " +
                         "       AND activo = @Activo " +
                         "       AND rol = @Rol)" +
                         " );";

                    ultimoJuezInsert = listaJueces[0].Id_juez;
                    int juezResult = 0;

                    foreach(Juez_Torneo juez in listaJueces)
                    {
                        ultimoJuezInsert = juez.Id_juez;

                        juezResult = await connection.ExecuteAsync(
                            insertJuecesQuery, 
                            juez,
                            transaction);

                        if (juezResult == 0) throw new Exception($"No se pudo agregar al juez [{ultimoJuezInsert}].");
                    }

                   

                    transaction.Commit();
                    exito = true;
                }
                catch (Exception ex) {
                    transaction.Rollback();

                    if (ex.Message.Contains("Column 'id_juez' cannot be null"))
                        throw new InvalidInputException($"Juez [{ultimoJuezInsert}] no existe (o no está activo).");
                    if (ex.Message.Contains("Column 'nombre_serie' cannot be null"))
                        throw new InvalidInputException($"La serie [{ultimaSerieInsert}] no existe.");
                    throw ex;
                }


            }



            return exito;
        }

        //UPDATE jueces del torneo
        public async Task<int> AgregarJuez(
            int id_organizador,
            int id_torneo, 
            int id_juez, 
            string rol, 
            string faseRegistro)
        {
            string insertQueryConValidacion = " INSERT INTO jueces_torneo " +
                                              " VALUES (           " +
                                              "        (SELECT id FROM torneos " +
                                              "         WHERE id = @Id_torneo" +
                                              "         AND fase = @Fase " +
                                              "         AND id_organizador = @Id_organizador " +
                                              "         AND id NOT IN " +
                                              "             (SELECT id_torneo FROM torneos_cancelados) " +
                                              "         )," +
                                              "        (SELECT id FROM usuarios " +
                                              "         WHERE id=@Id_juez " +
                                              "         AND rol = @Rol)   " +
                                              " );                       ";

            
            try
            {
                return await connection.ExecuteAsync(insertQueryConValidacion, new
                {
                    Id_organizador = id_organizador,
                    Id_torneo = id_torneo,
                    Id_juez = id_juez,
                    Rol = rol,
                    Fase = faseRegistro
                });

            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("Column 'id_torneo' cannot be null"))
                    throw new InvalidInputException($"No se pudo agregar el juez al torneo. Razones posibles: 1. El torneo [{id_torneo}] no pertenece al organizador [{id_organizador}]. 2. El torneo no está en fase de registro. 3. El torneo no existe. 4. El torneo está cancelado.");

                if (ex.Message.Contains("Column 'id_juez' cannot be null"))
                    throw new InvalidInputException($"El juez con ID [{id_juez}] no existe.");

                throw ex;
            }

        }

        public async Task<int> EliminarJuez(
            int id_organizador,
            int id_torneo, 
            int id_juez,
            string faseRegistro)
        {
            string deleteQuery = " DELETE FROM jueces_torneo " +
                                 " WHERE id_juez = @Id_juez " +
                                 " AND id_torneo = (SELECT id FROM torneos " +
                                 "                  WHERE id = @Id_torneo " +
                                 "                  AND id_organizador = @Id_organizador " +
                                 "                  AND fase = @Fase " +
                                 "                  AND id NOT IN " +
                                 "                      (SELECT id_torneo FROM torneos_cancelados) " +
                                 "                  ); ";


            int result = 0;
            try
            {
                result = await connection.ExecuteAsync(deleteQuery, new
                {
                    Id_organizador = id_organizador,
                    Id_torneo = id_torneo,
                    Id_juez = id_juez,
                    Fase = faseRegistro
                });

                if (result == 0) throw new InvalidInputException($"No se eliminó el juez por alguna de estas razones: (1) El juez con id [{id_juez}] no existe en el torneo [{id_torneo}]. (2) El torneo no se encuentra en fase registro. (3) El torneo tiene otro organizador. (4) El torneo no existe. (5) El torneo está cancelado.");
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }


        //READ torneos

        public async Task<Torneo> BuscarTorneoActivo(Torneo busqueda)
        {
            string selectQuery;


            if (busqueda.Id != default)
            {
                selectQuery = " SELECT * FROM torneos " +
                              " WHERE id = @Id " +
                              " AND id NOT IN (SELECT id_torneo FROM torneos_cancelados);";

                return await connection.QueryFirstOrDefaultAsync<Torneo>(selectQuery, new
                {
                    Id = busqueda.Id
                });
            }


            return null;//(Torneo busqueda) sin ningun campo valido para buscar
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneos(IList<int> id_torneos)
        {
            string selectQuery = " SELECT * FROM torneos WHERE id IN @id_torneos";

            return await connection.QueryAsync<Torneo>(selectQuery, new { id_torneos });
        }


        public async Task<IEnumerable<Torneo>> BuscarTorneosActivos(Torneo busqueda)
        {
            string selectQuery = null;


            if (busqueda.Fase != default) 
                selectQuery = " SELECT * FROM torneos " +
                              " WHERE fase = @Fase " +
                              " AND id NOT IN " +
                              "   (SELECT id_torneo FROM torneos_cancelados); ";


            return await connection.QueryAsync<Torneo>(selectQuery, new
            {
                Fase = busqueda.Fase
            });

            return null;//(Torneo busqueda) sin ningun campo valido para buscar
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneos(string[] fases)
        {
            IEnumerable<Torneo> result = Enumerable.Empty<Torneo>();

            string selectQuery = " SELECT * FROM torneos " +
                                 " WHERE fase = @Fase; ";

            foreach (string fase in fases)
            {
                IEnumerable<Torneo> queryResult =
                    await connection.QueryAsync<Torneo>(selectQuery,new { Fase = fase});

                result = result.Concat(queryResult);
            }

            return result;
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneosInscriptos(int id_jugador)
        {
            string selectQuery = " SELECT * FROM torneos " +
                                 " WHERE " +
                                 "      fase = @faseRegistro" +
                                 " AND " +
                                 "      id IN " + 
                                 "          (SELECT id_torneo FROM jugadores_inscriptos " +
                                 "           WHERE id_jugador = @id_jugador)" +
                                 " ; ";

            return await connection.QueryAsync<Torneo>(
                selectQuery,
                new {faseRegistro = FasesTorneo.REGISTRO, id_jugador });
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneosOrganizados(string[] fases, int id_organizador)
        {
            IEnumerable<Torneo> result = Enumerable.Empty<Torneo>();
            //using (MySqlTransaction transaction = connection.BeginTransaction())
            //{
                string selectQuery = " SELECT * FROM torneos " +
                                     " WHERE fase = @Fase " +
                                     " AND id_organizador = @Id_organizador; ";

                //try
                //{
                    foreach (string fase in fases)
                    {
                        IEnumerable<Torneo> queryResult =
                            await connection.QueryAsync<Torneo>(
                                selectQuery,
                                new { Fase = fase, Id_organizador = id_organizador }
                                //,
                                //transaction
                                );

                        result = result.Concat(queryResult);
                    }

                    //transaction.Commit();
                ////}
                ////catch (Exception ex) 
                ////{
                //    transaction.Rollback();
                //    throw ex;
                //}

                return result;
            //}
        }

        public async Task<IEnumerable<Torneo>> BuscarTorneosParaIniciar(string faseInscripcion, int id_organizador)
        {
            string selectQuery = " SELECT * FROM torneos " +
                                 " WHERE " +
                                 "      id_organizador = @Id_organizador " +
                                 " AND" +
                                 "      fase = @Fase " +
                                 " AND " +
                                 "      id NOT IN " +
                                 "          (SELECT id_torneo FROM torneos_cancelados) " +
                                 " AND " +
                                        //hay al menos 2 jugadores (para un minimo de 1 partida)
                                 "      2 >= (SELECT COUNT(*) FROM jugadores_inscriptos" +
                                     "        WHERE torneos.id = jugadores_inscriptos.id_torneo)" +
                                 " ; ";

            return await connection.QueryAsync<Torneo>(
                selectQuery,
                new { Fase = faseInscripcion, Id_organizador =  id_organizador }
                );

        }

        public async Task<IEnumerable<int>> BuscarIdTorneosOficializados(int id_juez, string faseFinalizado)
        {
            string selectQuery = " SELECT id_torneo FROM jueces_torneo " +
                                 " WHERE " +
                                 "      id_juez = @id_juez" +
                                 " AND " +
                                 "      @faseFinalizado = " +
                                 "          (SELECT fase FROM torneos " +
                                 "          WHERE torneos.id = jueces_torneo.id_torneo) " +
                                 " ; ";

            return await connection.QueryAsync<int>(selectQuery, new { id_juez, faseFinalizado });
        }

        public async Task<IEnumerable<Serie_Habilitada>> BuscarSeriesDeTorneos(IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM series_habilitadas " +
                                 " WHERE id_torneo IN @id_torneos; ";


            return await connection.QueryAsync<Serie_Habilitada>(
                selectQuery,
                new { id_torneos = torneos.Select(t=>t.Id) });
        }

        public async Task<IEnumerable<Juez_Torneo>> BuscarJuecesDeTorneos(IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM jueces_torneo " +
                                 " WHERE id_torneo IN @id_torneos; ";

            return await connection.QueryAsync<Juez_Torneo>(
                selectQuery,
                new {id_torneos = torneos.Select(t=>t.Id)});
        }

        public async Task<IEnumerable<Juez_Torneo>> BuscarJuecesDeTorneo(int id_organizador, int id_partida)
        {
            //Verificar que la partida pertenece a un torneo del organizador
            //Y verificar que la partida es "editable" -> no tiene id_ganador

            string selectQueryConVerificacion =
                " SELECT * from jueces_torneo " +
                " WHERE id_torneo = " +
                "       (SELECT id_torneo FROM partidas " +
                "        WHERE " +
                "               partidas.id = @id_partida " +
                "        AND " +
                "               partidas.id_ganador IS NULL " + //partida es editable
                "        AND " +
                "           partidas.id_torneo IN " + 
                "               (SELECT id FROM torneos " +
                "                WHERE id_organizador = @id_organizador " +//verificar organizador
                "                AND id NOT IN (SELECT id_torneo FROM torneos_cancelados) )" +
                "       ); ";


            return await connection.QueryAsync<Juez_Torneo>(
                selectQueryConVerificacion,
                new { id_partida, id_organizador });

        }

        public async Task<IEnumerable<Jugador_Inscripto>> BuscarJugadorInscripto(int id_jugador, IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM jugadores_inscriptos " +
                                 " WHERE " +
                                 "      id_jugador = @id_jugador " +
                                 " AND " +
                                 "      id_torneo IN @id_torneos; ";

            return await connection.QueryAsync<Jugador_Inscripto>(
                selectQuery,
                new { id_jugador, id_torneos = torneos.Select(t => t.Id) });
        }

        public async Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresInscriptos(IEnumerable<Torneo> torneos)
        {
            string selectQuery = " SELECT * FROM jugadores_inscriptos " +
                                 " WHERE id_torneo IN @id_torneos; ";

            return await connection.QueryAsync<Jugador_Inscripto>(
                selectQuery,
                new { id_torneos = torneos.Select(t => t.Id) });
        }

        public async Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresInscriptos(int id_torneo)
        {
            string selectQuery = " SELECT * FROM jugadores_inscriptos " +
                                 " WHERE id_torneo = @Id_torneo " +
                                 " ORDER BY orden ASC; ";

            return await connection.QueryAsync<Jugador_Inscripto>(
                selectQuery,
                new {
                    Id_torneo = id_torneo
                });

        }

        public async Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresAceptados(IEnumerable<Torneo> torneos) 
        {
            string selectQuery = " SELECT * FROM jugadores_inscriptos " +
                                 " WHERE id_torneo = @Id_torneo " +
                                 " AND aceptado = @Aceptado; ";

            IEnumerable<Jugador_Inscripto> result = Enumerable.Empty<Jugador_Inscripto>();

            foreach(Torneo torneo in torneos)
            {
                IEnumerable<Jugador_Inscripto> queryResult =  await connection.QueryAsync<Jugador_Inscripto>(
                    selectQuery,
                    new {
                        Id_torneo = torneo.Id,
                        Aceptado = true
                    });

                result = result.Concat(queryResult);
            }

            return result;
        }

        public async Task<IEnumerable<GanadorTorneo>> BuscarGanadoresTorneos(IEnumerable<Torneo> torneos)
        {
            IEnumerable<GanadorTorneo> result = Enumerable.Empty<GanadorTorneo>();

            string selectQuery = " SELECT id_ganador, id_torneo FROM partidas " +
                                 " WHERE " +
                                 "      id_torneo = @Id " +
                                 " AND " +
                                 "      ronda = " +
                                 "          (SELECT cantidad_rondas FROM torneos " +
                                 "          WHERE torneos.id = @Id)" +
                                 " ; ";

            foreach(Torneo torneo in torneos)
            {
                IEnumerable<GanadorTorneo> queryResult = 
                    await connection.QueryAsync<GanadorTorneo>(selectQuery, new {torneo.Id});

                result = result.Concat(queryResult);
            }

            return result;
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
                        "   POWER(2, (SELECT max_cantidad_rondas FROM torneos " +
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
                        "           fase = @Fase "                                  +
                        "       AND id NOT IN "                                     +
                        "           (SELECT id_torneo FROM torneos_cancelados) "    +  
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
                        throw new InvalidInputException($"No se pudo inscribir al torneo [{id_torneo}] por alguna de estas razones: 1. El torneo no está en fase '{fase_inscripcion}'. 2. El torneo no existe. 3. El torneo ya está lleno. 4. El torneo está cancelado.");
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

        public async Task<bool> ActualizarJugadoresYCantidadRondas(
            int id_torneo, IEnumerable<int> id_jugadores, int cantidad_rondas)
        {
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    //UPDATE jugadores aceptados
                    string jugadoresUpdateQuery = " UPDATE jugadores_inscriptos " +
                                            " SET aceptado = @aceptado " +
                                            " WHERE " +
                                            "  id_torneo = @id_torneo " +
                                            " AND " +
                                            "  id_jugador IN @id_jugadores; ";

                    int rows = await connection.ExecuteAsync(
                        jugadoresUpdateQuery,
                        new { id_torneo, id_jugadores, aceptado = true },
                        transaction);

                    if (rows != id_jugadores.Count()) throw new AceptarJugadoresException();

                    //UPDATE cantidad_rondas
                    string rondasUpdateQuery = " UPDATE torneos " +
                                               " SET cantidad_rondas = @cantidad_rondas " +
                                               " WHERE id = @id_torneo; ";

                    int torneoRows = await connection.ExecuteAsync(
                        rondasUpdateQuery,
                        new { id_torneo, cantidad_rondas },
                        transaction);

                    if (torneoRows != 1) throw new ActualizarCantidadRondasException();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
                    return true;
        }


        public async Task<bool> IniciarTorneo(
            string faseTorneo,
            int id_torneo,
            IList<InsertPartidaDTO> partidas_primera_ronda)
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


        public async Task<bool> CancelarTorneo(
            int id_admin,
            int id_torneo,
            string? motivo,
            DateTime now,
            string faseFinalizado)
        {
            string insertQuery =
                " INSERT INTO torneos_cancelados (id_torneo, id_admin, fechaHora, motivo) " +
                " VALUES (" +
                "   (SELECT id FROM torneos WHERE id = @id_torneo AND fase != @faseFinalizado), " +
                "   @id_admin, " +
                "   @now, " +
                "   @motivo); ";


            try
            {
                int rows = await connection.ExecuteAsync(
                    insertQuery,
                    new {
                        id_admin,
                        id_torneo,
                        now,
                        motivo,
                        faseFinalizado
                    });

                if (rows == 0) throw new Exception($"No se pudo cancelar el torneo en la db.");

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Column 'id_torneo' cannot be null"))
                    throw new InvalidInputException($"No se pudo cancelar el torneo [{id_torneo}] por alguna de estas razones: 1. Ya ha finalizado. 2. No existe. ");

                if (ex.Message.Contains("Duplicate entry") && ex.Message.Contains("for key 'torneos_cancelados.PRIMARY'"))
                    throw new InvalidInputException($"El torneo [{id_torneo}] no se canceló porque ya está cancelado.");

                throw ex;
            }

            return true;
        }



    }
}
