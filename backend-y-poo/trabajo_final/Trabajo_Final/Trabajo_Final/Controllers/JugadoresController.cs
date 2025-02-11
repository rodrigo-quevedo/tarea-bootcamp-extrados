using DAO.DAOs.Cartas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.ColeccionCartas;
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

        public JugadoresController (
            VerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática.
            GenerarCartasYSeries generarCartasYSeries,

            IColeccionarCartasService coleccionarCartas,
            IObtenerColeccionService obtenerColeccion,
            IQuitarCartasService quitarCartas
        )
        {
            coleccionarCartasService = coleccionarCartas;
            obtenerColeccionService = obtenerColeccion;
            quitarCartasService = quitarCartas;
        }


        [HttpPost]
        [Route("/coleccion")]
        [Authorize(Roles = Roles.JUGADOR)]
        public async Task<ActionResult> ColeccionarCartas(ArrayIdCartasDTO dto)
        {
            //parsear ID usuario
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_usuario_id, out int usuario_id);


            await coleccionarCartasService.Coleccionar(usuario_id, dto.Id_cartas);
            

            return Ok(new {message= "Se agregaron las cartas a la colección."});
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
            string string_usuario_id = User.FindFirst(ClaimTypes.Sid).Value;
            Int32.TryParse(string_usuario_id, out int usuario_id);

            
            await quitarCartasService.QuitarCartas(usuario_id, dto.Id_cartas);

            return Ok(new { message = "Se quitaron las cartas de la colección."});
        }

    }
}
