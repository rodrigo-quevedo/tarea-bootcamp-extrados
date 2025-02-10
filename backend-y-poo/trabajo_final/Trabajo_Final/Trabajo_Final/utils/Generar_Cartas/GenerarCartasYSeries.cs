using DAO.DAOs.Cartas;
using DAO.DAOs.DI;
using DAO.Entidades.Cartas;
using System.Globalization;

namespace Trabajo_Final.utils.Generar_Cartas
{
    public class GenerarCartasYSeries
    {
        ICartaDAO cartaDAO;
        public GenerarCartasYSeries(ICartaDAO dao)
        {
            cartaDAO = dao;
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

                arrSeries.Append(new Serie
                {
                    Nombre = char_nombreSerie,
                    Fecha_salida = fecha_salida
                });
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
            string url_img = "https://proveedor.en.la.nube.com/miusuario/imagenes/";

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

                url_img += i;

                arrCartas.Append(new Carta
                {
                    Id = id,
                    Ataque = atk,
                    Defensa = def,
                    Ilustracion = url_img
                });
            }

            return arrCartas;
        }
    
        private Series_De_Carta[] GenerarSeriesDeCartas(Serie[] arrSeries, Carta[] arrCartas, int maxSeriesPorCarta)
        {
            Series_De_Carta[] arrSeriesDeCartas 
                = new Series_De_Carta[arrCartas.Length * maxSeriesPorCarta];

            Random rnd = new Random();

            foreach (var carta in arrCartas)
            {
                int random_cantidad_series = rnd.Next(1, maxSeriesPorCarta + 1);

                for (int i = 0; i < random_cantidad_series; i++) { 
                    int random_serie_index = rnd.Next(0, arrSeries.Length);

                    arrSeriesDeCartas.Append(new Series_De_Carta
                    {
                        Id_carta = carta.Id,
                        Nombre_serie = arrSeries[random_serie_index].Nombre
                    });
                }    
            }

            return arrSeriesDeCartas;
        }
    
    

    
    
    }
}
