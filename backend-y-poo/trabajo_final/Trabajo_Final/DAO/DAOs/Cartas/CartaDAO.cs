using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
using DAO.Entidades.Cartas;
using DAO.Entidades.ColeccionCartas;
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
            connection.Open();
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
       


        //CREATE 
        public async Task<bool> ColeccionarCartas(int id_jugador, int[] id_cartas)
        {
            bool exito = false;
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                string insertQuery = " INSERT into cartas_coleccionadas " +
                                     " VALUES(@Id_carta, @Id_jugador); ";

                try
                {
                    foreach (int id_carta in id_cartas)
                    {
                        await connection.ExecuteAsync(
                            insertQuery, 
                            new {
                                Id_carta = id_carta,
                                Id_jugador = id_jugador
                            },
                            transaction: transaction
                        );
                    }

                    transaction.Commit();
                    exito = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error transaction [coleccionar cartas]: " + ex.Message);
                    throw new DefaultException("No se pudo agregar las cartas a la colección.", ex);
                }

            }
            return exito;

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

        public async Task<IEnumerable<Serie_De_Carta>> BuscarSeriesDeCartas(int[] id_cartas)
        {
            string selectQuery = " SELECT * FROM series_de_cartas " +
                                 " WHERE id_carta IN @Id_carta; ";

            return await connection.QueryAsync<Serie_De_Carta>(selectQuery, new
            {
                Id_carta = id_cartas
            });

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
                    foreach (int id_carta in id_cartas)
                    {
                        await connection.ExecuteAsync(
                            deleteQuery, 
                            new {
                                Id_jugador = id_jugador,
                                Id_carta = id_carta
                            },
                            transaction: transaction
                        );
                    }

                    transaction.Commit();
                    exito = true;
                }
                catch (Exception ex) {
                    transaction.Rollback();
                    Console.WriteLine("Error transaction [coleccionar cartas]: " + ex.Message);
                    throw new DefaultException("No se pudo quitar las cartas de la colección.", ex);
                }
            }
            return exito;
        }
    }
}
