﻿using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;
using System.Globalization;

namespace Trabajo_Final.utils.Generar_Cartas
{
    public class GenerarCartasYSeries
    {
        ICartaDAO cartaDAO;
        public GenerarCartasYSeries(ICartaDAO dao)
        {
            //Si esto falla, resetear tablas con Use database + Run selection

            cartaDAO = dao;
            Serie[] arrSeries = GenerarSeries(10);
            Carta[] arrCartas = GenerarCartas(60);
            Series_De_Carta[] arrSeriesDeCartas = GenerarSeriesDeCartas(arrSeries, arrCartas, 2);

            //test:
            //foreach (Serie serie in arrSeries) { Console.WriteLine(serie.Nombre); }
            //foreach (Carta carta in arrCartas) { Console.WriteLine(carta.Id); }
            //foreach (Series_De_Carta serieDeCarta in arrSeriesDeCartas) { Console.WriteLine($"{serieDeCarta.Id_carta} -> {serieDeCarta.Nombre_serie} "); }
            
            cartaDAO.InicializarEnDB(
                arrSeries, arrCartas, arrSeriesDeCartas,
                true, true, true//quitar esta linea para ejecutar
            );

        }


        private Serie[] GenerarSeries(int cantidadSeries)
        {
            if (cantidadSeries > 26) 
                throw new Exception("Este método solo genera series la A a la Z.");
            
            Serie[] arrSeries = new Serie[cantidadSeries];

            string char_nombreSerie = "";
            DateTime fecha_salida = DateTime.Parse("2023-01-01T12:00:00Z", null, DateTimeStyles.RoundtripKind); ;
            
            for (int i = 0; i < arrSeries.Length; i++)
            {
                int char_serie = 97 + i;
                char_nombreSerie = ((char)char_serie).ToString().ToUpper(); 

                fecha_salida = fecha_salida.AddMonths(2);

                arrSeries[i] = new Serie {
                    Nombre = char_nombreSerie,
                    Fecha_salida = fecha_salida
                };
            }

            return arrSeries;
        }

        private Carta[] GenerarCartas(int cantidadCartas)
        {
            Carta[] arrCartas = new Carta[cantidadCartas];

            Random rnd = new Random();
            int id = 0;
            int atk = 0;
            int def = 0;
            string base_url = "https://proveedor.en.la.nube.com/miusuario/imagenes/";

            for (int i = 1; i <= arrCartas.Length; i++)
            {
                id = i;
                atk = rnd.Next(0, 30) * 100;
                def = rnd.Next(0, 30) * 100;

                while (atk + def != 3000)
                {
                    atk = rnd.Next(0, 30) * 100;
                    def = rnd.Next(0, 30) * 100;
                }

                arrCartas[i-1]  = new Carta {
                    Id = id,
                    Ataque = atk,
                    Defensa = def,
                    Ilustracion = base_url + i
                };
            }

            return arrCartas;
        }
    
        private Series_De_Carta[] GenerarSeriesDeCartas(Serie[] arrSeries, Carta[] arrCartas, int maxSeriesPorCarta)
        {
            if (maxSeriesPorCarta > arrSeries.Length) throw new Exception("No se puede asignar a una carta más series de las que actualmente existen.");


            Series_De_Carta[] arrSeriesDeCartas 
                = new Series_De_Carta[arrCartas.Length * maxSeriesPorCarta];

            Random rnd = new Random();


            int acc_index = 0;
            foreach (var carta in arrCartas)
            {
                int random_cantidad_series = rnd.Next(1, maxSeriesPorCarta + 1);

                int[] indexes_ocupados = new int[random_cantidad_series];

                for (int i = 0; i < random_cantidad_series; i++) { 
                    int random_serie_index = rnd.Next(0, arrSeries.Length);

                    while (indexes_ocupados.Contains(random_serie_index)) {
                        random_serie_index = rnd.Next(0, arrSeries.Length);
                    }

                    indexes_ocupados[i] = random_serie_index;

                    arrSeriesDeCartas[acc_index++] = new Series_De_Carta {
                        Id_carta = carta.Id,
                        Nombre_serie = arrSeries[random_serie_index].Nombre
                    };
                }    
            }

            //Limpiar nulls (al calcular el tamaño casi siempre sobra):
            Series_De_Carta[] result_arr = new Series_De_Carta[acc_index];
            int res_acc_index = 0;
            foreach (var carta in arrSeriesDeCartas)
            {
                if (carta == null) break;
                result_arr[res_acc_index++] = carta;
            }

            return result_arr;
        }
    
    

    
    
    }
}
