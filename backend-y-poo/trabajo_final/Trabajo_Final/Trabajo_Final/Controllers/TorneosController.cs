﻿using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Entidades.PartidaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.BuscarTorneos;
using Trabajo_Final.DTO.Request.EditarPartidas;
using Trabajo_Final.DTO.Request.InputTorneos;
using Trabajo_Final.DTO.Request.OficializarPartidas;
using Trabajo_Final.DTO.Response.TorneoResponseDTO;
using Trabajo_Final.Services.JugadorServices.BuscarTorneosDisponibles;
using Trabajo_Final.Services.PartidaServices.Buscar_Partidas;
using Trabajo_Final.Services.PartidaServices.Editar_Jueces_Partida;
using Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas;
using Trabajo_Final.Services.PartidaServices.Oficializar_Partidas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.CancelarTorneo;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.Services.TorneoServices.EditarJueces;
using Trabajo_Final.Services.TorneoServices.IniciarTorneo;
using Trabajo_Final.Services.TorneoServices.InscribirJugador;

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

        IBuscarTorneosService buscarTorneosService;
        IIniciarTorneoService iniciarTorneoService;

        IBuscarPartidasParaOficializarService buscarPartidasService;
        IOficializarPartidaService oficializarPartidaService;

        IEditarJuezPartidaService editarJuezPartidaService;
        IEditarJugadoresPartidasService editarJugadoresPartidasService;

        ICancelarTorneoService cancelarTorneoService;

        public TorneosController(
            ICrearTorneoService crearTorneo,
            IAgregarJuezService agregarJuez,
            IEliminarJuezService eliminarJuez,

            IBuscarTorneosDisponiblesService buscarTorneosDisponibles,

            IInscribirJugadorService inscribirJugador,

            IBuscarTorneosService buscarTorneos,
            IIniciarTorneoService iniciarTorneo,

            IBuscarPartidasParaOficializarService buscarPartidas,
            IOficializarPartidaService oficializarPartida,

            IEditarJuezPartidaService editarJuezPartida,
            IEditarJugadoresPartidasService editarJugadoresPartidas,

            ICancelarTorneoService cancelarTorneo
        )
        {
            crearTorneoService = crearTorneo;
            agregarJuezService = agregarJuez;
            eliminarJuezService = eliminarJuez;

            buscarTorneosDisponiblesService = buscarTorneosDisponibles;

            inscribirJugadorService = inscribirJugador;

            buscarTorneosService = buscarTorneos;
            iniciarTorneoService = iniciarTorneo;

            buscarPartidasService = buscarPartidas;
            oficializarPartidaService = oficializarPartida;

            editarJuezPartidaService = editarJuezPartida;
            editarJugadoresPartidasService = editarJugadoresPartidas;

            cancelarTorneoService = cancelarTorneo;
        }


        [HttpPost]
        [Route("/torneos")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> CrearTorneo(RegistroTorneoDTO dto)
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out var id_organizador);
            
            string[] series_habilitadas = dto.series_habilitadas.Distinct().ToArray();
            int[] id_jueces_torneo = dto.id_jueces_torneo.Distinct().ToArray();


            await crearTorneoService.CrearTorneo(
                id_organizador,
                dto.fecha_hora_inicio, dto.fecha_hora_fin,
                dto.horario_diario_inicio, dto.horario_diario_fin,
                dto.pais,
                series_habilitadas,
                id_jueces_torneo
            );

            return Ok(new { message = "Torneo creado con éxito" });
        }

        [HttpPost]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> AgregarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            //id organizador
            Int32.TryParse(User.FindFirst(ClaimTypes.Sid).Value, out int id_organizador);

            await agregarJuezService.AgregarJuez(id_organizador, id_torneo, (int)dto.id_juez);

            return Ok(new { message = $"Juez con ID [{dto.id_juez}] agregado con éxito al torneo [{id_torneo}]." });
        }

        [HttpDelete]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EliminarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            //id organizador
            Int32.TryParse(User.FindFirst(ClaimTypes.Sid).Value, out int id_organizador);

            await eliminarJuezService.EliminarJuez(id_organizador, id_torneo, (int)dto.id_juez);

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
            inscribirJugadorService.VerificarRepeticionesMazo(dto.id_cartas_mazo);

            //id jugador
            string str_id_jugador = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(str_id_jugador, out int id_jugador);


            await inscribirJugadorService.Inscribir(id_jugador, (int)dto.id_torneo, dto.id_cartas_mazo);


            return Ok(new { message = $"El jugador id {id_jugador} se inscribió con éxito al torneo [{dto.id_torneo}]." });
        }

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

        [HttpGet]
        [Route("/torneos/organizados")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> BuscarTorneosOrganizados([FromQuery] BuscarTorneosDTO dto)
        {
            //fases
            string[] fases;

            if (dto.fases == null || dto.fases.Length == 0)
                fases = FasesTorneo.fases;
            
            else
             fases = dto.fases.Distinct().ToArray();

            //id organizador
            string str_id_organizador = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(str_id_organizador, out int id_organizador);


            IList<TorneoOrganizadoDTO> result = await buscarTorneosService.BuscarTorneosOrganizados(fases, id_organizador);

            return Ok(new { lista_torneos = result.ToArray() });
        }

        [HttpGet]
        [Route("/torneos/iniciar")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> BuscarTorneosLlenos()
        {
            string str_id_organizador = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(str_id_organizador, out int id_organizador);

            IList<TorneoLlenoDTO> result = await buscarTorneosService.BuscarTorneosLlenos(id_organizador);

            if (result == null) return Ok(new { message = "No hay torneos llenos para aceptar inscripciones." });

            return Ok(result);
        }


        [HttpPost]
        [Route("/torneos/iniciar")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> IniciarTorneo(IniciarTorneoDTO dto)
        {
            Int32.TryParse(User.FindFirst(ClaimTypes.Sid).Value, out int id_organizador);


            await iniciarTorneoService.IniciarTorneo((int)dto.id_torneo, id_organizador);


            return Ok(new { 
                message = "Se inició el torneo correctamente."
            });
        }

        [HttpGet]
        [Route("/torneos/oficializar")]
        [Authorize(Roles = Roles.JUEZ)]
        public async Task<ActionResult> BuscarPartidasParaOficializar()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_juez);

            IEnumerable<Partida> result = 
                await buscarPartidasService.BuscarPartidasParaOficializar(id_juez);

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

            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_juez);

            await oficializarPartidaService.OficializarPartida(
                id_juez,
                (int) dto.id_partida,
                (int) dto.id_ganador,
                dto.id_descalificado,
                dto.motivo_descalificacion);

            return Ok(new { message = $"La partida {dto.id_partida} se oficializó con éxito." });
        }

        [HttpGet]
        [Route("/torneos/oficializados")]
        [Authorize(Roles = Roles.JUEZ)]
        public async Task<ActionResult> BuscarTorneosOficializados()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_juez);

            IList<TorneoOficializadoDTO> result = 
                await buscarTorneosService.BuscarTorneosOficializados(id_juez);

            if (result == null || !result.Any()) return Ok($"No hay torneos oficializados por el juez id [{id_juez}");
            return Ok(result);
        }


        [HttpPut]
        [Route("/partidas/juez")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EditarJuezPartida(EditarJuezPartidaDTO dto)
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_organizador);

            await editarJuezPartidaService.EditarJuezPartida(id_organizador, (int) dto.Id_partida, (int) dto.Id_juez);

            return Ok(new { message = $"Se editó el juez de la partida {dto.Id_partida} con éxito. El nuevo juez es {dto.Id_juez}" });
        }

        [HttpPut]
        [Route("/partidas/jugadores")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EditarJugadoresPartidas(EditarJugadoresPartidasDTO dto)
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_organizador);

            await editarJugadoresPartidasService.EditarJugadoresDePartidas(id_organizador, dto.editar_jugadores_partidas);

            return Ok(new { message = $"Se editaron los jugadores de las partidas con éxito."});
        }


        [HttpPost]
        [Route("/torneos/cancelar")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> CancelarTorneo(CancelarTorneoDTO dto)
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_admin);

            await cancelarTorneoService.CancelarTorneo(id_admin, dto);

            return Ok(new { message = $"El torneo [{dto.Id_torneo}] fue cancelado con exito." });
        }

    }
}
