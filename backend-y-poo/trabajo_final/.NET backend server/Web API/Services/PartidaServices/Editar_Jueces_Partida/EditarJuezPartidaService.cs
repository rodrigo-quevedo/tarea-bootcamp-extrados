
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.Entidades.TorneoEntidades;
using System.Text.Json;

namespace Trabajo_Final.Services.PartidaServices.Editar_Jueces_Partida
{
    public class EditarJuezPartidaService : IEditarJuezPartidaService
    {
        private IPartidaDAO partidaDAO;
        private ITorneoDAO torneoDAO;
        public EditarJuezPartidaService(IPartidaDAO partidaDao, ITorneoDAO torneoDao)
        {
            partidaDAO = partidaDao;
            torneoDAO = torneoDao;
        }

        public async Task<bool> EditarJuezPartida(int id_organizador, int id_partida, int id_juez)
        {
            //buscar jueces (tiene verificacion adentro del dao)
            IEnumerable<Juez_Torneo> jueces_torneo = 
                await torneoDAO.BuscarJuecesDeTorneo(id_organizador, id_partida);

            if (!jueces_torneo.Any() || jueces_torneo == null) throw new InvalidInputException($"No tiene permiso para editar el juez de la partida. Razones posibles: 1. La partida [{id_partida}] no pertenece a un torneo del organizador [{id_organizador}]. 2. La partida [{id_partida}] ya ha sido oficializada y no se puede editar. 3. La partida no existe. 4. El torneo está cancelado.");

            Console.WriteLine(JsonSerializer.Serialize(jueces_torneo));

            //verificar que juez esta en jueces_torneo
            IList<int> id_jueces = jueces_torneo.Select(juez => juez.Id_juez).ToList();

            if (!id_jueces.Contains(id_juez)) throw new InvalidInputException($"El juez [{id_juez}] no pertenece al torneo [{jueces_torneo.First().Id_torneo}].");

            //dao UPDATE
            return await partidaDAO.EditarJuezPartida(id_partida, id_juez);

        }
    }
}
