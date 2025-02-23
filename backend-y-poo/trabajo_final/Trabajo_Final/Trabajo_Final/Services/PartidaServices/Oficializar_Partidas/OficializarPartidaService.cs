using DAO.DAOs.Partidas;

namespace Trabajo_Final.Services.PartidaServices.Oficializar_Partidas
{
    public class OficializarPartidaService : IOficializarPartidaService
    {
        IPartidaDAO partidaDAO;
        public OficializarPartidaService(IPartidaDAO partidaDao)
        {
            partidaDAO = partidaDao;
        }

        public async Task<bool> OficializarPartida(
            int id_juez, 
            int id_partida, 
            int id_ganador, 
            int id_descalificado)
        {

            //verificar ultima partida de ronda

            //---> es ultima partida de ronda
                //buscar torneo (horario_diario_inicio y horario_diario_fin)

                //armar fechahoras

                //buscar jueces

                //buscar jugadores

                //armar partidas

            //


            throw new NotImplementedException();
        }
    }
}
