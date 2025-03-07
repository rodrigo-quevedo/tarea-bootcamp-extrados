using DAO.Entidades.Cartas;
using DAO.Entidades.ColeccionCartas;
using DAO.Entidades.TorneoEntidades;
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
            Serie_De_Carta[] arrSeriesDeCartas,

            bool seriesCargadas = false,
            bool cartasCargadas = false,
            bool seriesDeCartaCargadas = false
        );

        public Task<IEnumerable<string>> ObtenerNombresSeries();

        public Task<bool> ColeccionarCartas(int id_jugador, int[] id_cartas);

        public Task<IEnumerable<Carta>> BuscarCartas(int[] id_cartas);
        public Task<IEnumerable<Carta>> BuscarCartasColeccionadas(int usuario_id);

        public Task<IEnumerable<Serie_De_Carta>> BuscarSeriesDeCartas(int[] id_cartas);

        public Task<bool> QuitarCartasColeccionadas(int id_jugador, int[] id_cartas);

        public Task<IEnumerable<Carta_Del_Mazo>> BuscarMazosInscriptos(
            int id_jugador, IList<int> id_torneos);
        public Task<IEnumerable<Carta_Del_Mazo>> BuscarMazosInscriptos(
            IList<int> id_jugadores, IList<int> id_torneos);
    }
}
