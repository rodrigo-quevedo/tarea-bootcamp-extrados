﻿
using DAO.DAOs.Torneos;

namespace Trabajo_Final.Services.TorneoServices.EditarJueces
{
    public class EliminarJuezService : IEliminarJuezService
    {
        ITorneoDAO torneoDAO;

        public EliminarJuezService(ITorneoDAO torneoDao)
        {
            torneoDAO = torneoDao;
        }

        public async Task<bool> EliminarJuez(int id_torneo, int id_juez)
        {
            await torneoDAO.EliminarJuez(id_torneo, id_juez);

            return true;
        }
    }
}
