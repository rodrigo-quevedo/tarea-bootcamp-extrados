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
using Trabajo_Final.Services.TorneoServices.InscribirJugador;
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
        
        IInscribirJugadorService inscribirJugadorService;
        
        
        public TorneosController(
            ICrearTorneoService crearTorneo,
            IAgregarJuezService agregarJuez,
            IEliminarJuezService eliminarJuez,

            IBuscarTorneosDisponiblesService buscarTorneosDisponibles,

            IInscribirJugadorService inscribirJugador
        )
        {
            crearTorneoService = crearTorneo;
            agregarJuezService = agregarJuez;
            eliminarJuezService = eliminarJuez;

            buscarTorneosDisponiblesService = buscarTorneosDisponibles;

            inscribirJugadorService = inscribirJugador;
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
            await agregarJuezService.AgregarJuez(id_torneo, (int)dto.id_juez);

            return Ok(new { message = $"Juez con ID [{dto.id_juez}] agregado con éxito al torneo [{id_torneo}]." });
        }

        [HttpDelete]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EliminarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            await eliminarJuezService.EliminarJuez(id_torneo, (int)dto.id_juez);

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


        [HttpPost]
        [Route("/torneos/inscribirse")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> InscribirseATorneo(InscripcionTorneoDTO dto)
        {
            //verificar que no hay repetidas en el mazo
            VerificarRepeticionesMazo(dto.id_cartas_mazo);

            //id jugador
            string str_id_jugador = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(str_id_jugador, out int id_jugador);


            await inscribirJugadorService.Inscribir(id_jugador, (int)dto.id_torneo, dto.id_cartas_mazo);


            return Ok( new { message = $"El jugador id {id_jugador} se inscribió con éxito al torneo [{dto.id_torneo}]."});
        }

        private void VerificarRepeticionesMazo(int[] id_cartas_mazo)
        {
            int id_repetida = 0;

            try 
            {
                id_repetida = id_cartas_mazo
                    .GroupBy(id => id)
                    .First(id => id.Count() > 1)
                    .Key;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sequence contains no matching element"))
                    Console.WriteLine("No hay cartas repetidas en el mazo");

                else throw ex;
            }

            if (id_repetida != 0) throw new InvalidInputException($"La carta id [{id_repetida}] esta repetida.");
        }




    }
}
