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
        //[Authorize]
        public async Task<ActionResult> BuscarIlustracionCarta([FromRoute] int id_ilustracion)
        {
            byte[] result = 
                await buscarImagenesService.BuscarIlustracion(id_ilustracion);

            //esta Response permite mostrar la img en la pestaña del navegador:
            return File(result, MediaTypeNames.Image.Jpeg);

            //Esta otra Response no muestra la imagen, sino que manda la descarga directamente:
            //return File(result, MediaTypeNames.Image.Jpeg, $"{id_ilustracion}.jpg");

            //De cualquier manera, esto es al acceder con URL o con el tag <a>.
            //El tag <img/> siempre muestra la imagen.
        }



    }
}
