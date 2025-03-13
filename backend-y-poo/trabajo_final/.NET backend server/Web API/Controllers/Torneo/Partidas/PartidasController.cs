using DAO.Entidades.PartidaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.BuscarPartidas;
using Trabajo_Final.Services.PartidaServices.Buscar_Datos_Partidas;

namespace Trabajo_Final.Controllers.Torneo.Partidas
{
    [ApiController]
    [Route("[controller]")]
    public class PartidasController : ControllerBase
    {
        private readonly IBuscarPartidasService buscarPartidasService;


        public PartidasController(IBuscarPartidasService buscarPartidasService)
        {
            this.buscarPartidasService = buscarPartidasService;
        }



        //Endpoint de detalle partidas (la información varía dependiendo el rol):

        [HttpGet]
        [Route("/partidas")]
        [Authorize]
        public async Task<ActionResult> BuscarPartidas(BuscarPartidasDTO dto)
        {
            dto.id_partidas = dto.id_partidas.Distinct().ToArray();


            string rol_logeado = User.FindFirstValue(ClaimTypes.Role);
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_logeado);

            IEnumerable<Partida> result =
                await buscarPartidasService.BuscarPartidas(rol_logeado, id_logeado, dto.id_partidas);

            return Ok(new { partidas = result.ToArray() });
        }


    }
}
