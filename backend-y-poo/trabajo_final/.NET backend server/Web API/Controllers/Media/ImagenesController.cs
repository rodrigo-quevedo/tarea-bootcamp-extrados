using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Trabajo_Final.Services.ImagenesServices.BuscarImagenes;

namespace Trabajo_Final.Controllers.Media
{

    [ApiController]
    [Route("/imagenes")]
    public class ImagenesController : ControllerBase
    {
        private readonly IBuscarImagenesService buscarImagenesService;


        public ImagenesController(IBuscarImagenesService buscarImagenesService)
        {
            this.buscarImagenesService = buscarImagenesService;
        }



        //Endpoints para devolver imágenes guardadas en el servidor:

        [HttpGet]
        [Route("ilustraciones/{id_ilustracion}")]
        //[Authorize]
        public async Task<ActionResult> BuscarIlustracionCarta([FromRoute] int id_ilustracion)
        {
            byte[] result =
                await buscarImagenesService.BuscarIlustracion(id_ilustracion);

            return File(result, MediaTypeNames.Image.Jpeg);
        }


        [HttpGet]
        [Route("foto_perfil/{id_foto_perfil}")]
        //[Authorize]
        public async Task<ActionResult> BuscarFotoPerfil([FromRoute] string id_foto_perfil)
        {
            byte[] result =
                await buscarImagenesService.BuscarFotoPerfil(id_foto_perfil);

            return File(result, MediaTypeNames.Image.Jpeg);
        }

    }
}
