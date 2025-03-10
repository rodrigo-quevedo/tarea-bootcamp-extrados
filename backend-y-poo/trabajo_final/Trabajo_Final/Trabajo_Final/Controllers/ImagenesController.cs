using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Trabajo_Final.Services.ImagenesServices.BuscarImagenes;

namespace Trabajo_Final.Controllers
{

    [ApiController]
    [Route("/imagenes")]
    public class ImagenesController : ControllerBase
    {
        private IBuscarImagenesService buscarImagenesService;
        public ImagenesController(
            IBuscarImagenesService buscarImagenesService
            
            )
        {
            this.buscarImagenesService = buscarImagenesService;
        }



        [HttpGet]
        [Route("ilustraciones/{id_ilustracion}")]
        [Authorize]
        public async Task<ActionResult> BuscarIlustracionCarta([FromRoute] int id_ilustracion)
        {
            Console.WriteLine($"GET en /ilustraciones: {DateTime.Now}");

            byte[] result = 
                await buscarImagenesService.BuscarIlustracion(id_ilustracion);

            return File(result, MediaTypeNames.Image.Jpeg, $"{id_ilustracion}");
        }

    }
}
