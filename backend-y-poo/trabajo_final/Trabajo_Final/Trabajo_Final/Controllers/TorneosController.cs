using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Torneos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Torneo;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TorneosController : ControllerBase
    {
        ICrearTorneoService crearTorneoService;

        public TorneosController(
            ICrearTorneoService crearTorneo   
        )
        {
            crearTorneoService = crearTorneo;
        }


        [HttpPost]
        [Route("/torneos")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> CrearTorneo(RegistroTorneoDTO dto)
        {
            string string_id_organizador = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_id_organizador, out var id_organizador);

            DateTime fecha_hora_inicio = DateTime.Parse(dto.fecha_hora_inicio, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime fecha_hora_fin = DateTime.Parse(dto.fecha_hora_fin, null, System.Globalization.DateTimeStyles.RoundtripKind);

            
            await crearTorneoService.CrearTorneo(
                id_organizador, 
                fecha_hora_inicio, fecha_hora_fin, 
                dto.pais,
                dto.series_habilitadas,
                dto.id_jueces_torneo
            );

            return Ok(new { message = "Torneo creado con éxito"});
        }

    }
}
