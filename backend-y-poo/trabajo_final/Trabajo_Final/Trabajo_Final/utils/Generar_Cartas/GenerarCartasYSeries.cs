using Configuration.ServerRoutes;
using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;
using System.Globalization;
using System.Text.Json;

namespace Trabajo_Final.utils.Generar_Cartas
{
    public class GenerarCartasYSeries
    {
        ICartaDAO cartaDAO;
        IServerRoutesConfiguration serverRoutesConfig;
        public GenerarCartasYSeries(ICartaDAO dao, IServerRoutesConfiguration serverRoutes)
        {
            cartaDAO = dao;
            serverRoutesConfig = serverRoutes;
            
            //Si esto falla, resetear tablas con Use database + Run selection

            Serie[] arrSeries = GenerarSeries(10);
            Carta[] arrCartas = GenerarCartas(60);
            Serie_De_Carta[] arrSeriesDeCartas = GenerarSeriesDeCartas(arrSeries, arrCartas, 2);

            //test:
            //foreach (Serie serie in arrSeries) { Console.WriteLine(serie.Nombre); }
            foreach (Carta carta in arrCartas) { Console.WriteLine(JsonSerializer.Serialize(carta)); }
            //foreach (Serie_De_Carta serieDeCarta in arrSeriesDeCartas) { Console.WriteLine($"{serieDeCarta.Id_carta} -> {serieDeCarta.Nombre_serie} "); }
            
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
            string base_url = serverRoutesConfig.GetIlustracionesRoute() + "/";

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
    
        private Serie_De_Carta[] GenerarSeriesDeCartas(Serie[] arrSeries, Carta[] arrCartas, int maxSeriesPorCarta)
        {
            if (maxSeriesPorCarta > arrSeries.Length) throw new Exception("No se puede asignar a una carta más series de las que actualmente existen.");

            List<Serie_De_Carta> arrSeriesDeCartas = new List<Serie_De_Carta>();

            Random rnd = new Random();


            foreach (var carta in arrCartas)
            {
                int random_cantidad_series = rnd.Next(1, maxSeriesPorCarta + 1);

                List<int> indexes_ocupados = new List<int>();

                for (int i = 0; i < random_cantidad_series; i++) { 
                    int random_serie_index = rnd.Next(0, arrSeries.Length);

                    while (indexes_ocupados.Contains(random_serie_index)) {
                        random_serie_index = rnd.Next(0, arrSeries.Length);
                    }

                    indexes_ocupados.Add(random_serie_index);

                    arrSeriesDeCartas.Add( new Serie_De_Carta { 
                        Id_carta = carta.Id,
                        Nombre_serie = arrSeries[random_serie_index].Nombre
                    });
                }    
            }

            return arrSeriesDeCartas.ToArray();
        }
    
    

    
    
    }
}
