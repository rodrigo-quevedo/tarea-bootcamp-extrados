using Constantes.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.BuscarTorneos;
using Trabajo_Final.DTO.Request.InputTorneos;
using Trabajo_Final.DTO.Response.TorneoResponseDTO;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.CancelarTorneo;

namespace Web_API.Controllers.Torneo.FuncionesRestringidasPorRol
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IBuscarTorneosService buscarTorneosService;
        private readonly ICancelarTorneoService cancelarTorneoService;


        public AdminController(
            IBuscarTorneosService buscarTorneosService,
            ICancelarTorneoService cancelarTorneoService
        )
        {
            this.buscarTorneosService = buscarTorneosService;
            this.cancelarTorneoService = cancelarTorneoService;
        }



        //Endpoint para buscar torneos:

        [HttpGet]
        [Route("/torneos")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> BuscarTorneos([FromQuery] BuscarTorneosDTO dto)
        {
            //fases
            string[] fases;

            if (dto.fases == null || dto.fases.Length == 0)
                fases = FasesTorneo.fases;

            else
                fases = dto.fases.Distinct().ToArray();

            IList<TorneoVistaAdminDTO> result = await buscarTorneosService.BuscarTorneos(fases);

            return Ok(new { lista_torneos = result.ToArray() });
        }



        //Endpoint para cancelar torneos:

        [HttpPost]
        [Route("/torneos/cancelar")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> CancelarTorneo(CancelarTorneoDTO dto)
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_admin);

            await cancelarTorneoService.CancelarTorneo(id_admin, dto);

            return Ok(new { message = $"El torneo [{dto.Id_torneo}] fue cancelado con exito." });
        }



    }
}
