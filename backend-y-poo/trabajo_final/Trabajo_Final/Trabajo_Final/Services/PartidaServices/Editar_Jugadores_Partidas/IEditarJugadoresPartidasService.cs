using Trabajo_Final.DTO.EditarPartidas;

namespace Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas
{
    public interface IEditarJugadoresPartidasService
    {
        public Task<bool> EditarJugadoresDePartidas(
            int id_organizador, 
            Jugadores_Partida[] jugadores_partidas);


    }
}
