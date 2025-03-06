using Constantes.Constantes;
using DAO.DAOs.Torneos;
using Trabajo_Final.DTO.Request.InputTorneos;

namespace Trabajo_Final.Services.TorneoServices.CancelarTorneo
{
    public class CancelarTorneoService: ICancelarTorneoService
    {
        private ITorneoDAO torneoDAO;
        public CancelarTorneoService(ITorneoDAO torneoDao)
        {
            torneoDAO = torneoDao;
        }

        public async Task<bool> CancelarTorneo(int id_admin, CancelarTorneoDTO dto)
        {
            DateTime now = DateTime.UtcNow;

            Console.WriteLine(now);

            return await torneoDAO.CancelarTorneo(
                id_admin, (int)dto.Id_torneo, dto.motivo, now, FasesTorneo.FINALIZADO);
        }

    }
}
