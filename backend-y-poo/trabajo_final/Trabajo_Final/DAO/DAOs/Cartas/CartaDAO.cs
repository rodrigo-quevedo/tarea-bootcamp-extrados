using DAO.Connection;
using DAO.Entidades.Cartas;
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

        public async Task<int> CrearSerieDeCarta(Series_De_Carta seriesDeCarta)
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
        public async Task<bool> InicializarDB(
            Serie[] arrSeries,
            Carta[] arrCartas,
            Series_De_Carta[] arrSeriesDeCartas,

            bool seriesCargadas = false, 
            bool cartasCargadas = false, 
            bool seriesDeCartaCargadas = false
        ) {

            bool flag_seriesCargadas = seriesCargadas;
            bool flag_cartasCargadas = cartasCargadas;
            bool flag_seriesDeCartasCargadas = seriesDeCartaCargadas;

            if (flag_seriesCargadas) 
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
                        flag_seriesCargadas = false;
                    }
                }

            if (flag_cartasCargadas)
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
                        flag_cartasCargadas = false;
                    }


                }

            if (flag_seriesDeCartasCargadas)
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
                        flag_seriesDeCartasCargadas = false;
                    }

                }


            return (flag_seriesCargadas && flag_cartasCargadas && flag_seriesDeCartasCargadas);
        }
        

    }
}
