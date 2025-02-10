using DAO.Entidades.Cartas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Cartas
{
    public interface ICartaDAO
    {
        public bool InicializarEnDB(
            Serie[] arrSeries,
            Carta[] arrCartas,
            Series_De_Carta[] arrSeriesDeCartas,

            bool seriesCargadas = false,
            bool cartasCargadas = false,
            bool seriesDeCartaCargadas = false
        );
    }
}
