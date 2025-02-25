using DAO.DAOs.Cartas;
using DAO.Entidades.Custom.Descalificaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.ColeccionCartas;
using Trabajo_Final.DTO.ColeccionCartas.ResponseColeccionar;
using Trabajo_Final.Services.JugadorServices.BuscarDescalificaciones;
using Trabajo_Final.Services.JugadorServices.ColeccionarCartas;
using Trabajo_Final.Services.JugadorServices.ObtenerColeccion;
using Trabajo_Final.Services.JugadorServices.QuitarCartas;
using Trabajo_Final.utils.Constantes;
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
        
        private IBuscarDescalificacionesService buscarDescalificacionesService;

        public JugadoresController (
            VerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            GenerarCartasYSeries generarCartasYSeries,

            IColeccionarCartasService coleccionarCartas,
            IObtenerColeccionService obtenerColeccion,
            IQuitarCartasService quitarCartas,

            IBuscarDescalificacionesService buscarDescalificaciones
        )
        {
            coleccionarCartasService = coleccionarCartas;
            obtenerColeccionService = obtenerColeccion;
            quitarCartasService = quitarCartas;

            buscarDescalificacionesService = buscarDescalificaciones;
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

            ResponseColeccionarDTO response =  await coleccionarCartasService.Coleccionar(usuario_id, id_cartas);
            

            return Ok(response);
        }


        [HttpGet]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> ObtenerCartasColeccionadas()
        {
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_usuario_id, out int usuario_id);

            CartaColeccionadaDTO[] coleccion = 
                await obtenerColeccionService.ObtenerColeccion(usuario_id);

            return Ok( new {cartas_coleccionadas = coleccion});
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
        [Route("/descalificaciones")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> BuscarDescalificaciones()
        {
            Int32.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_jugador);

            IEnumerable<DescalificacionDTO> result = 
                await buscarDescalificacionesService.BuscarDescalificaciones(id_jugador);

            if (result == null || !result.Any()) return Ok($"El jugador [{id_jugador}] no tiene descalificaciones.");

            return Ok(result);
        }


    }
}
