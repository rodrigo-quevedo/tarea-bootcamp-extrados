using DAO.Entidades.Cartas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trabajo_Final.DTO.Request.BuscarCartas;
using Trabajo_Final.DTO.Request.BuscarSeries;
using Trabajo_Final.DTO.Response.Cartas;
using Trabajo_Final.Services.CartasServices.BuscarCartas;
using Trabajo_Final.Services.CartasServices.BuscarSeries;

namespace Trabajo_Final.Controllers.Torneo.CartasYSeries
{
    [ApiController]
    [Route("[controller]")]
    public class CartasController : ControllerBase
    {
        private readonly IBuscarCartasService buscarCartasService;
        private readonly IBuscarSeriesService buscarSeriesService;


        public CartasController(
            IBuscarCartasService buscarCartasService,
            IBuscarSeriesService buscarSeriesService
        )
        {
            this.buscarCartasService = buscarCartasService;
            this.buscarSeriesService = buscarSeriesService;
        }



        //Endpoints para mostrar detalles de cartas y series:

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

        [HttpGet]
        [Route("/series")]
        [Authorize]
        public async Task<ActionResult> BuscarSerie(BuscarSeriesDTO dto)
        {
            dto.nombres_series = dto.nombres_series.Distinct().ToArray();//eliminar repetidas del input

            IEnumerable<Serie> result =
                await buscarSeriesService.BuscarSeries(dto.nombres_series);

            return Ok(new { series = result.ToArray() });
        }


    }
}
