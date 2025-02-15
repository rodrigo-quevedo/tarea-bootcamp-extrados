using DAO.DAOs.Torneos;

namespace Trabajo_Final.Services.TorneoServices.InscribirJugador
{
    public class InscribirJugadorService : IInscribirJugadorService
    {
        ITorneoDAO torneoDAO;
        public InscribirJugadorService(ITorneoDAO torneoDao)
        {
            torneoDAO = torneoDao;
        }


        //public async Task<bool> InscribirJugador (
        //    int id_jugador,
        //    int id_torneo,
        //    int[] mazo
        //)
        //{
        //    //

        //}

    }
}
