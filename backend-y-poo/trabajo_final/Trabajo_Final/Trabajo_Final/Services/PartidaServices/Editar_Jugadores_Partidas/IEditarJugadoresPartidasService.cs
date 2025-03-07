using DAO.DTOs_en_DAOs.JugadoresPartidas;

namespace Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas
{
    public interface IEditarJugadoresPartidasService
    {
        public Task<bool> EditarJugadoresDePartidas(
            int id_organizador, 
            JugadoresPartida[] jugadores_partidas);


    }
}
