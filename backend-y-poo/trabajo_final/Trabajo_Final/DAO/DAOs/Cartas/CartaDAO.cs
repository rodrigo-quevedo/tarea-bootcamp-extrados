using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
using DAO.Entidades.Cartas;
using DAO.Entidades.ColeccionCartas;
using DAO.Entidades.TorneoEntidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Cartas
{
    public class CartaDAO : ICartaDAO
    {
        private MySqlConnection connection { get; set; }

        public CartaDAO(string connectionString) 
        {
            connection = new SingletonConnection(connectionString).Instance;
            if (connection.State != System.Data.ConnectionState.Open) connection.Open();
        }


        // ------------------ CRUD ------------------ //
        public async Task<int> CrearSerie(Serie serie)
        {
            string insertQuery = " INSERT into series " +
                                 " VALUES (@Nombre, @Fecha_salida);";

            return await connection.ExecuteAsync(insertQuery, new
            {
                Nombre = serie.Nombre,
                Fecha_salida = serie.Fecha_salida
            });
        }

        public async Task<int> CrearCarta(Carta carta)
        {
            string insertQuery = " INSERT into cartas(ataque, defensa, ilustracion) " +
                                 " VALUES (@Atk, @Def, @Ilustracion);";

            return await connection.ExecuteAsync(insertQuery, new
            {
                Atk = carta.Ataque,
                Def= carta.Defensa,
                Ilustracion = carta.Ilustracion
            });
        }

        public async Task<int> CrearSerieDeCarta(Serie_De_Carta seriesDeCarta)
        {
            string insertQuery = " INSERT into series_de_cartas " +
                                 " VALUES (@Nombre_serie, @Id_carta);";

            return await connection.ExecuteAsync(insertQuery, new
            {
                Nombre_serie = seriesDeCarta.Nombre_serie,
                Id_carta = seriesDeCarta.Id_carta
            });
        }

        //cargar series y cartas
        public bool InicializarEnDB(
            Serie[] arrSeries,
            Carta[] arrCartas,
            Serie_De_Carta[] arrSeriesDeCartas,

            bool seriesCargadas = false, 
            bool cartasCargadas = false, 
            bool seriesDeCartaCargadas = false
        ) {
            
            bool flag_seriesCargadas = seriesCargadas;
            bool flag_cartasCargadas = cartasCargadas;
            bool flag_seriesDeCartasCargadas = seriesDeCartaCargadas;

            if (!flag_seriesCargadas) 
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    string insertQuery = " INSERT into series " +
                                         " VALUES (@Nombre, @Fecha_salida);";


                    try
                    {
                        foreach (var serie in arrSeries)
                        {
                            connection.Execute(insertQuery,
                            //new {
                            //    Nombre = serie.Nombre,
                            //    Fecha_salida = serie.Fecha_salida
                            //}, 
                            serie,
                            transaction: transaction);
                        }

                        transaction.Commit();

                        flag_seriesCargadas = true;
                    }
                    catch (Exception ex) 
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error transaction [series]: " + ex.Message);
                    }
                }

            if (!flag_cartasCargadas)
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    string insertQuery = " INSERT into cartas(id, ataque, defensa, ilustracion) " +
                                         " VALUES (@Id, @Ataque, @Defensa, @Ilustracion);";

                    try
                    {
                        foreach (var carta in arrCartas)
                        {
                            connection.Execute(insertQuery,
                            //new {
                            //    Nombre = serie.Nombre,
                            //    Fecha_salida = serie.Fecha_salida
                            //}, 
                            carta,
                            transaction: transaction);
                        }

                        transaction.Commit();

                        flag_cartasCargadas = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error transaction [cartas]: " + ex.Message);
                    }


                }

            if (!flag_seriesDeCartasCargadas)
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    string insertQuery = " INSERT into series_de_cartas " +
                                         " VALUES (@Nombre_serie, @Id_carta);";

                    try
                    {
                        foreach (var seriesDeCarta in arrSeriesDeCartas)
                        {
                            connection.Execute(insertQuery,
                            //new {
                            //    Nombre = serie.Nombre,
                            //    Fecha_salida = serie.Fecha_salida
                            //}, 
                            seriesDeCarta,
                            transaction: transaction);
                        }

                        transaction.Commit();

                        flag_seriesDeCartasCargadas = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error transaction [series_de_cartas]: " + ex.Message);
                    }

                }


            return (flag_seriesCargadas && flag_cartasCargadas && flag_seriesDeCartasCargadas);
        }
       
        //READ nombres de todas las series
        public async Task<IEnumerable<string>> ObtenerNombresSeries()
        {
            string selectQuery = " SELECT nombre FROM series; ";

            return await connection.QueryAsync<string>(selectQuery, null);

        }



        //CREATE 
        public async Task<bool> ColeccionarCartas(int id_jugador, int[] id_cartas)
        {
            bool exito = false;
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                string insertQuery = " INSERT into cartas_coleccionadas " +
                                     " VALUES(@Id_carta, @Id_jugador); ";

                string selectIdQuery = " SELECT LAST_INSERT_ID(); ";

                int id_ultima_carta_INSERT = 0;
                try
                {
                    foreach (int id_carta in id_cartas)
                    {
                        id_ultima_carta_INSERT = id_carta;

                        await connection.ExecuteAsync(
                            insertQuery, 
                            new {
                                Id_carta = id_carta,
                                Id_jugador = id_jugador
                            },
                            transaction
                        );                        
                    }

                    transaction.Commit();
                    exito = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error transaction [coleccionar cartas]: " + ex.Message);

                    if (ex.Message.Contains("Cannot add or update a child row: a foreign key constraint fails (`trabajo_final_backend`.`cartas_coleccionadas`, CONSTRAINT `cartas_coleccionadas_ibfk_1` FOREIGN KEY (`id_carta`) REFERENCES `cartas` (`id`))"))
                        throw new InvalidInputException($"La carta id [{id_ultima_carta_INSERT}] no existe.");

                    throw ex;
                }

            }
            return exito;

        }

        //READ carta
        public async Task<IEnumerable<Carta>> BuscarCartas(int[] id_cartas)
        {
            string selectQuery = " SELECT * FROM cartas " +
                                 " WHERE id IN @id_cartas; ";

            return await connection.QueryAsync<Carta>(selectQuery, new { id_cartas } );
        }

        //READ cartas_coleccionadas
        public async Task<IEnumerable<Carta>> BuscarCartasColeccionadas(int usuario_id)
        {
            string selectQuery = " SELECT * FROM cartas " +
                                 " WHERE id IN ( " +
                                 "    SELECT id_carta FROM cartas_coleccionadas " +
                                 "    WHERE id_jugador = @Id_jugador " +
                                 " ); ";

            return await connection.QueryAsync<Carta>(selectQuery, new {
                    Id_jugador = usuario_id
                });
        }

        public async Task<IEnumerable<Serie>> BuscarSeries(string[] nombres_series)
        {
            string selectQuery = " SELECT * FROM series WHERE nombre IN @nombres_series; ";

            return await connection.QueryAsync<Serie>(selectQuery, new { nombres_series } );
        }

        public async Task<IEnumerable<Serie_De_Carta>> BuscarSeriesDeCartas(int[] id_cartas)
        {
            //string selectQuery = " SELECT * FROM series_de_cartas " +
            //                     " WHERE id_carta IN @id_cartas; ";
            //return await connection.QueryAsync<Serie_De_Carta>(selectQuery, new {id_cartas});


            IEnumerable<Serie_De_Carta> result = Enumerable.Empty<Serie_De_Carta>();
            

            int ultimo_id_carta_select = id_cartas[0];
                     
            string selectQuery = " SELECT * from series_de_cartas " +
                                    " WHERE id_carta = " +
                                    "      (SELECT id FROM cartas " +//verifica que existe
                                    "       WHERE id=@id_carta); ";

            foreach (int id_carta in id_cartas) 
            {
                ultimo_id_carta_select = id_carta;

                IEnumerable<Serie_De_Carta> queryResult =
                    await connection.QueryAsync<Serie_De_Carta>(selectQuery, new {id_carta});

                if (!queryResult.Any()) throw new InvalidInputException($"No existe ninguna carta id [{ultimo_id_carta_select}]"); ;

                result = result.Concat(queryResult);
            }

            return result;
        }

        //DELETE cartas coleccionadas
        public async Task<bool> QuitarCartasColeccionadas(int id_jugador, int[] id_cartas)
        {
            
            string deleteQuery = " DELETE FROM cartas_coleccionadas " +
                                 " WHERE id_jugador = @Id_jugador " +
                                 "       AND" +
                                 "       id_carta = @Id_carta;  ";

            bool exito = false;

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int id_ultima_carta_DELETE = 0;
                    foreach (int id_carta in id_cartas)
                    {
                        id_ultima_carta_DELETE = id_carta;

                        int result = await connection.ExecuteAsync(
                            deleteQuery, 
                            new {
                                Id_jugador = id_jugador,
                                Id_carta = id_carta
                            },
                            transaction
                        );

                    //DELETE en MySql no tira ninguna Exception cuando algun valor es invalido.
                    //En su lugar compruebo las filas afectadas.
                    //Si me mandan un monton de IDs invalidas, me las ahorro con el 1er throw:
                        if (result == 0) throw new InvalidInputException($"La carta [{id_ultima_carta_DELETE}] no existe en la coleccion.");

                    }

                    transaction.Commit();
                    exito = true;
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    Console.WriteLine("Error transaction [coleccionar cartas]: " + ex.Message);

                    throw ex;
                }
            }
            return exito;
        }

        public async Task<IEnumerable<Carta_Del_Mazo>> BuscarMazosInscriptos(
            int id_jugador, IList<int> id_torneos)
        {
            string selectQuery = " SELECT * FROM cartas_del_mazo " +
                                 " WHERE " +
                                 "      id_jugador = @id_jugador " +
                                 " AND " +
                                 "      id_torneo IN @id_torneos; ";

            return await connection.QueryAsync<Carta_Del_Mazo>(
                selectQuery,
                new {
                    id_jugador,
                    id_torneos
                });
        }

         public async Task<IEnumerable<Carta_Del_Mazo>> BuscarMazosInscriptos(
            IList<int> id_jugadores, IList<int> id_torneos)
        {
            string selectQuery = " SELECT * FROM cartas_del_mazo " +
                                 " WHERE " +
                                 "      id_jugador IN @id_jugadores " +
                                 " AND " +
                                 "      id_torneo IN @id_torneos; ";

            return await connection.QueryAsync<Carta_Del_Mazo>(
                selectQuery,
                new {
                    id_jugadores,
                    id_torneos
                });
        }

    }
}
