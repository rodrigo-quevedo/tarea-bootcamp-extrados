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

        public async Task<bool> Inscribir(int id_jugador, int id_torneo, int[] id_cartas_mazo)
        {


            return true;
        }
    }
}
