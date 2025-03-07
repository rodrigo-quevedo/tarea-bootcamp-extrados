using Constantes.Constantes;
using DAO.Entidades.PartidaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.ColeccionarCartas;
using Trabajo_Final.DTO.Response.Cartas;
using Trabajo_Final.DTO.Response.ResponseColeccionar;
using Trabajo_Final.DTO.Response.TorneoResponseDTO;
using Trabajo_Final.Services.JugadorServices.BuscarPartidas;
using Trabajo_Final.Services.JugadorServices.ColeccionarCartas;
using Trabajo_Final.Services.JugadorServices.ObtenerColeccion;
using Trabajo_Final.Services.JugadorServices.QuitarCartas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;

using Trabajo_Final.utils.Generar_Cartas;
using Trabajo_Final.utils.Verificar_Existencia_Admin;


namespace Trabajo_Final.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JugadoresController : ControllerBase
    {
        private IColeccionarCartasService coleccionarCartasService;
        private IObtenerColeccionService obtenerColeccionService;
        private IQuitarCartasService quitarCartasService;

        private IBuscarPartidasService buscarPartidasService;
        private IBuscarTorneosService buscarTorneosService;

        public JugadoresController(
            VerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            GenerarCartasYSeries generarCartasYSeries,

            IColeccionarCartasService coleccionarCartas,
            IObtenerColeccionService obtenerColeccion,
            IQuitarCartasService quitarCartas,

            IBuscarPartidasService buscarPartidas,
            IBuscarTorneosService buscarTorneos
        )
        {
            coleccionarCartasService = coleccionarCartas;
            obtenerColeccionService = obtenerColeccion;
            quitarCartasService = quitarCartas;

            buscarPartidasService = buscarPartidas;
            buscarTorneosService = buscarTorneos;
        }


        [HttpPost]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> ColeccionarCartas(ArrayIdCartasDTO dto)
        {
            //ID usuario
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_usuario_id, out int usuario_id);


            //Eliminar repeticiones dentro del array de IDs
            int[] id_cartas = dto.Id_cartas.Distinct().ToArray();

            ResponseColeccionarDTO response = await coleccionarCartasService.Coleccionar(usuario_id, id_cartas);


            return Ok(response);
        }


        [HttpGet]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> ObtenerCartasColeccionadas()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int usuario_id);

            int[] coleccion =
                await obtenerColeccionService.ObtenerColeccion(usuario_id);

            return Ok(new { id_cartas_coleccionadas = coleccion });
        }

        [HttpDelete]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BorrarCartasColeccionadas(ArrayIdCartasDTO dto)
        {
            //ID jugador
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_usuario_id, out int usuario_id);

            //Eliminar repeticiones dentro del array de IDs
            int[] id_cartas = dto.Id_cartas.Distinct().ToArray();

            await quitarCartasService.QuitarCartas(usuario_id, id_cartas);

            return Ok(new {
                message = "Se quitaron las cartas de la colección.",
                cartas_eliminadas = id_cartas
            });
        }

        [HttpGet]
        [Route("/partidas/descalificaciones")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarDescalificaciones()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

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
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

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
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

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
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IList<TorneoGanadoDTO> result =
                await buscarTorneosService.BuscarTorneosGanados(id_jugador);

            if (result == null || !result.Any()) return Ok($"No hay torneos ganados del jugador [{id_jugador}]");

            return Ok(result);
        }


    }
}
