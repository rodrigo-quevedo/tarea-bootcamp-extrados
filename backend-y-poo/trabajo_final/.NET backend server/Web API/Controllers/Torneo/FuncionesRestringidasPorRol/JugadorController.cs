﻿using Constantes.Constantes;
using DAO.Entidades.PartidaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.ColeccionarCartas;
using Trabajo_Final.DTO.Request.InputTorneos;
using Trabajo_Final.DTO.Response.ResponseColeccionar;
using Trabajo_Final.DTO.Response.TorneoResponseDTO;
using Trabajo_Final.Services.JugadorServices.BuscarPartidas;
using Trabajo_Final.Services.JugadorServices.ColeccionarCartas;
using Trabajo_Final.Services.JugadorServices.ObtenerColeccion;
using Trabajo_Final.Services.JugadorServices.QuitarCartas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.InscribirJugador;
using Trabajo_Final.utils.Generar_Cartas;


namespace Web_API.Controllers.Torneo.FuncionesRestringidasPorRol
{
    [ApiController]
    [Route("[controller]")]
    public class JugadorController : ControllerBase
    {
        private readonly IColeccionarCartasService coleccionarCartasService;
        private readonly IObtenerColeccionService obtenerColeccionService;
        private readonly IQuitarCartasService quitarCartasService;
        private readonly IInscribirJugadorService inscribirJugadorService;
        private readonly IBuscarPartidasJugadorService buscarPartidasService;
        private readonly IBuscarTorneosService buscarTorneosService;


        public JugadorController(
            GenerarCartasYSeries generarCartasYSeries,

            IColeccionarCartasService coleccionarCartasService,
            IObtenerColeccionService obtenerColeccionService,
            IQuitarCartasService quitarCartasService,
            IInscribirJugadorService inscribirJugadorService,
            IBuscarPartidasJugadorService buscarPartidasService,
            IBuscarTorneosService buscarTorneosService
        )
        {
            this.coleccionarCartasService = coleccionarCartasService;
            this.obtenerColeccionService = obtenerColeccionService;
            this.quitarCartasService = quitarCartasService;
            this.inscribirJugadorService = inscribirJugadorService;
            this.buscarPartidasService = buscarPartidasService;
            this.buscarTorneosService = buscarTorneosService;
        }



        //Endpoints para cartas coleccionadas:

        [HttpGet]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> ObtenerCartasColeccionadas()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int usuario_id);

            int[] coleccion =
                await obtenerColeccionService.ObtenerColeccion(usuario_id);

            return Ok(new { id_cartas_coleccionadas = coleccion });
        }


        [HttpPost]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> ColeccionarCartas(ArrayIdCartasDTO dto)
        {
            //ID usuario
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            int.TryParse(string_usuario_id, out int usuario_id);


            //Eliminar repeticiones dentro del array de IDs
            int[] id_cartas = dto.Id_cartas.Distinct().ToArray();

            ResponseColeccionarDTO response = await coleccionarCartasService.Coleccionar(usuario_id, id_cartas);


            return Ok(response);
        }


        [HttpDelete]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BorrarCartasColeccionadas(ArrayIdCartasDTO dto)
        {
            //ID jugador
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            int.TryParse(string_usuario_id, out int usuario_id);

            //Eliminar repeticiones dentro del array de IDs
            int[] id_cartas = dto.Id_cartas.Distinct().ToArray();

            await quitarCartasService.QuitarCartas(usuario_id, id_cartas);

            return Ok(new
            {
                message = "Se quitaron las cartas de la colección.",
                cartas_eliminadas = id_cartas
            });
        }


        //demo front: plantillas de mazos

        [HttpGet]
        [Route("/mazos")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarMazosJugador(ArrayIdCartasDTO dto)
        {
            return Ok(new
            {

            });
        }



        //Endpoints para inscripcion a torneos:

        [HttpGet]
        [Route("/torneos/inscribirse")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarTorneosDisponibles()
        {

            IList<TorneoDisponibleDTO> torneos =
                await buscarTorneosService.BuscarTorneosDisponibles();

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
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);


            await inscribirJugadorService.Inscribir(id_jugador, (int)dto.id_torneo, dto.id_cartas_mazo);


            return Ok(new { message = $"El jugador id {id_jugador} se inscribió con éxito al torneo [{dto.id_torneo}]." });
        }


        [HttpGet]
        [Route("/torneos/inscribirse/estado")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarEstadoInscripciones()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IEnumerable<TorneoInscriptoDTO> result =
                await buscarTorneosService.BuscarTorneosInscriptos(id_jugador);

            return Ok(new { estado_inscripciones = result });
        }



        //Endpoint para ver próximas partidas a jugar:
        [HttpGet]
        [Route("/partidas/jugar")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarProximasPartidas()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IEnumerable<Partida> result = 
                await buscarPartidasService.BuscarPartidasPorJugar(id_jugador);

            return Ok(new { partidas_por_jugar = result });
        }



        //Endpoints para estadísticas:

        [HttpGet]
        [Route("/partidas/descalificaciones")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarDescalificaciones()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IEnumerable<Partida> result =
                await buscarPartidasService.BuscarDescalificaciones(id_jugador);

            if (result == null || !result.Any()) return Ok(new { message = $"El jugador [{id_jugador}] no tiene descalificaciones." });

            return Ok(result);
        }


        [HttpGet]
        [Route("/partidas/victorias")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarPartidasGanadas()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IEnumerable<Partida> result =
                await buscarPartidasService.BuscarPartidasGanadas(id_jugador);

            if (result == null || !result.Any()) return Ok(new { message = $"El jugador [{id_jugador}] no tiene partidas ganadas." });

            return Ok(result);
        }


        [HttpGet]
        [Route("/partidas/derrotas")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarPartidasPerdidas()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IEnumerable<Partida> result =
                await buscarPartidasService.BuscarPartidasPerdidas(id_jugador);

            if (result == null || !result.Any()) return Ok(new { message = $"El jugador [{id_jugador}] no tiene partidas perdidas." });

            return Ok(result);
        }


        [HttpGet]
        [Route("/torneos/victorias")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarTorneosGanados()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IList<TorneoGanadoDTO> result =
                await buscarTorneosService.BuscarTorneosGanados(id_jugador);

            if (result == null || !result.Any()) return Ok($"No hay torneos ganados del jugador [{id_jugador}]");

            return Ok(result);
        }




    }
}
