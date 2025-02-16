using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Torneos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.ListaTorneos;
using Trabajo_Final.DTO.Torneo;
using Trabajo_Final.Services.JugadorServices.BuscarTorneosDisponibles;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.Services.TorneoServices.EditarJueces;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TorneosController : ControllerBase
    {
        ICrearTorneoService crearTorneoService;
        IAgregarJuezService agregarJuezService;
        IEliminarJuezService eliminarJuezService;

        IBuscarTorneosDisponiblesService buscarTorneosDisponiblesService;
        public TorneosController(
            ICrearTorneoService crearTorneo,
            IAgregarJuezService agregarJuez,
            IEliminarJuezService eliminarJuez,

            IBuscarTorneosDisponiblesService buscarTorneosDisponibles
        )
        {
            crearTorneoService = crearTorneo;
            agregarJuezService = agregarJuez;
            eliminarJuezService = eliminarJuez;

            buscarTorneosDisponiblesService = buscarTorneosDisponibles;
        }


        [HttpPost]
        [Route("/torneos")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> CrearTorneo(RegistroTorneoDTO dto)
        {
            string string_id_organizador = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_id_organizador, out var id_organizador);


            await crearTorneoService.CrearTorneo(
                id_organizador,
                dto.fecha_hora_inicio, dto.fecha_hora_fin,
                dto.horario_diario_inicio, dto.horario_diario_fin,
                dto.pais,
                dto.series_habilitadas,
                dto.id_jueces_torneo
            );

            return Ok(new { message = "Torneo creado con éxito" });
        }

        [HttpPost]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> AgregarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            await agregarJuezService.AgregarJuez(id_torneo, (int) dto.id_juez);

            return Ok(new { message = $"Juez con ID [{dto.id_juez}] agregado con éxito al torneo [{id_torneo}]." });
        }

        [HttpDelete]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EliminarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            await eliminarJuezService.EliminarJuez(id_torneo, (int) dto.id_juez);

            return Ok(new { message = $"Juez [{dto.id_juez}] eliminado con éxito del torneo [{id_torneo}]." });
        }


        [HttpGet]
        [Route("/torneos/inscribirse")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarTorneosDisponibles()
        {
        
            IList<TorneoDisponibleDTO> torneos = 
                await buscarTorneosDisponiblesService.BuscarTorneosDisponibles();

            return Ok(torneos);
        }


        //[HttpPost]
        //[Route("/torneos/inscribirse")]
        //[Authorize(Roles = Roles.JUGADOR)]
        //public async Task<ActionResult> InscribirseATorneo(InscripcionTorneoDTO reqBody)
        //{
        //validar torneo service

        //validar cartas mazo service

        //inscribir service

        //}



    }
}
