
using Constantes.Constantes;
using DAO.DAOs.Torneos;

namespace Trabajo_Final.Services.TorneoServices.EditarJueces
{
    public class AgregarJuezService : IAgregarJuezService
    {
        private ITorneoDAO torneoDAO;
        public AgregarJuezService(ITorneoDAO torneoDao)
        {
            torneoDAO = torneoDao;
        }


        public async Task<bool> AgregarJuez(int id_organizador, int id_torneo, int id_juez)
        {
            await torneoDAO.AgregarJuez(
                id_organizador, 
                id_torneo, 
                id_juez, 
                Roles.JUEZ, 
                FasesTorneo.REGISTRO);

            return true;
        }
    }
}
