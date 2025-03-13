using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Entidades.PartidaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.OficializarPartidas;
using Trabajo_Final.DTO.Response.TorneoResponseDTO;
using Trabajo_Final.Services.PartidaServices.Buscar_Partidas_Para_Oficializar;
using Trabajo_Final.Services.PartidaServices.Oficializar_Partidas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;

namespace Web_API.Controllers.Torneo.FuncionesRestringidasPorRol
{
    [ApiController]
    [Route("[controller]")]
    public class JuezController : ControllerBase
    {
        private readonly IBuscarTorneosService buscarTorneosService;
        private readonly IBuscarPartidasParaOficializarService buscarPartidasParaOficializarService;
        private readonly IOficializarPartidaService oficializarPartidaService;


        public JuezController(
            IBuscarTorneosService buscarTorneosService,
            IBuscarPartidasParaOficializarService buscarPartidasParaOficializarService,
            IOficializarPartidaService oficializarPartidaService
        )
        {
            this.buscarTorneosService = buscarTorneosService;
            this.buscarPartidasParaOficializarService = buscarPartidasParaOficializarService;
            this.oficializarPartidaService = oficializarPartidaService;
        }



        //JUEZ endpoints:

        [HttpGet]
        [Route("/torneos/oficializar")]
        [Authorize(Roles = Roles.JUEZ)]
        public async Task<ActionResult> BuscarPartidasParaOficializar()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_juez);

            IEnumerable<Partida> result =
                await buscarPartidasParaOficializarService.BuscarPartidasParaOficializar(id_juez);

            if (result == null) return Ok(new { message = $"No hay partidas para el juez [{id_juez}]." });

            return Ok(result);
        }

        [HttpPost]
        [Route("/torneos/oficializar")]
        [Authorize(Roles = Roles.JUEZ)]
        public async Task<ActionResult> OficializarPartida(OficializarPartidaDTO dto)
        {
            if (dto.id_descalificado != null && dto.motivo_descalificacion == null)
                throw new InvalidInputException("Debe haber un 'motivo_descalificacion' junto al id_descalificado.");

            if (dto.id_descalificado == null && dto.motivo_descalificacion != null)
                throw new InvalidInputException("Debe haber un 'id_descalificado' junto al motivo_descalificacion");

            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_juez);

            await oficializarPartidaService.OficializarPartida(
                id_juez,
                (int)dto.id_partida,
                (int)dto.id_ganador,
                dto.id_descalificado,
                dto.motivo_descalificacion);

            return Ok(new { message = $"La partida {dto.id_partida} se oficializó con éxito." });
        }

        [HttpGet]
        [Route("/torneos/oficializados")]
        [Authorize(Roles = Roles.JUEZ)]
        public async Task<ActionResult> BuscarTorneosOficializados()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_juez);

            IList<TorneoOficializadoDTO> result =
                await buscarTorneosService.BuscarTorneosOficializados(id_juez);

            if (result == null || !result.Any()) return Ok($"No hay torneos oficializados por el juez id [{id_juez}");
            return Ok(result);
        }



    }
}
