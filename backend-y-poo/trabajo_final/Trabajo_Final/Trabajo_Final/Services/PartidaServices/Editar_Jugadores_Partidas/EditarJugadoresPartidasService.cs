using Trabajo_Final.DTO.EditarPartidas;

namespace Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas
{
    public class EditarJugadoresPartidasService : IEditarJugadoresPartidasService
    {

        public EditarJugadoresPartidasService() 
        { 
        
        }

        public Task<bool> EditarJugadoresDePartidas(
            int id_organizador, 
            Jugadores_Partida[] jugadores_partidas)
        {
            //juntar todas las partidas y verificarlas
            
            //verificaciones partida:
            // - son partidas de un mismo torneo
            // - el torneo fue organizado por id_organizador
            // - las partidas pertenecen a la ronda 1


            //juntar todos los jugadores y verificarlos



            throw new NotImplementedException();
        }
    }
}
