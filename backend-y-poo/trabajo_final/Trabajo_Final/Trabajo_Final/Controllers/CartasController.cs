using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trabajo_Final.DTO.Request.BuscarCartas;
using Trabajo_Final.DTO.Response.Cartas;
using Trabajo_Final.Services.CartasServices.BuscarCartas;

namespace Trabajo_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartasController : ControllerBase
    {
        private IBuscarCartasService buscarCartasService;

        public CartasController(
            IBuscarCartasService buscarCartas
        
        ){ 
            buscarCartasService = buscarCartas;
        }


        [HttpGet]
        [Route("/cartas")]
        [Authorize]
        public async Task<ActionResult> BuscarCarta(BuscarCartasDTO dto)
        {
            dto.id_cartas = dto.id_cartas.Distinct().ToArray();//eliminar repetidas del input

            IEnumerable<DatosCartaDTO> result =
                await buscarCartasService.BuscarCartas(dto.id_cartas);

            return Ok(new { cartas = result.ToArray() });
        }

        //[HttpGet]
        //[Route("/series")]
        //[Authorize]
        //public async Task<ActionResult> BuscarSerie()
        //{

        //}


    }
}
