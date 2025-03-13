using Constantes.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.DTO.Request.BuscarTorneos;
using Trabajo_Final.DTO.Request.EditarPartidas;
using Trabajo_Final.DTO.Request.InputTorneos;
using Trabajo_Final.DTO.Response.TorneoResponseDTO;
using Trabajo_Final.Services.PartidaServices.Editar_Jueces_Partida;
using Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.Services.TorneoServices.EditarJueces;
using Trabajo_Final.Services.TorneoServices.IniciarTorneo;

namespace Web_API.Controllers.Torneo.FuncionesRestringidasPorRol
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizadorController : ControllerBase
    {
        private readonly ICrearTorneoService crearTorneoService;
        private readonly IBuscarTorneosService buscarTorneosService;
        private readonly IIniciarTorneoService iniciarTorneoService;
        private readonly IAgregarJuezService agregarJuezService;
        private readonly IEliminarJuezService eliminarJuezService;
        private readonly IEditarJuezPartidaService editarJuezPartidaService;
        private readonly IEditarJugadoresPartidasService editarJugadoresPartidasService;


        public OrganizadorController(
            ICrearTorneoService crearTorneoService,
            IBuscarTorneosService buscarTorneosService,
            IIniciarTorneoService iniciarTorneoService,
            IAgregarJuezService agregarJuezService,
            IEliminarJuezService eliminarJuezService,
            IEditarJuezPartidaService editarJuezPartidaService,
            IEditarJugadoresPartidasService editarJugadoresPartidasService
        )
        {
            this.crearTorneoService = crearTorneoService;
            this.buscarTorneosService = buscarTorneosService;
            this.iniciarTorneoService = iniciarTorneoService;
            this.agregarJuezService = agregarJuezService;
            this.eliminarJuezService = eliminarJuezService;
            this.editarJuezPartidaService = editarJuezPartidaService;
            this.editarJugadoresPartidasService = editarJugadoresPartidasService;
        }



        //Endpoint para crear torneos:

        [HttpPost]
        [Route("/torneos")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> CrearTorneo(RegistroTorneoDTO dto)
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out var id_organizador);

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



        //Endpoint para obtener información de torneos organizados:

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
            int.TryParse(str_id_organizador, out int id_organizador);


            IList<TorneoOrganizadoDTO> result = await buscarTorneosService.BuscarTorneosOrganizados(fases, id_organizador);

            return Ok(new { lista_torneos = result.ToArray() });
        }



        //Endpoints para iniciar torneos:

        [HttpGet]
        [Route("/torneos/iniciar")]
        //Una alternativa a esta búsqueda (para buscar torneos a iniciar que aún NO están llenos),
        //es buscar los torneos en fase REGISTRO dentro de un GET a /torneos/organizados.
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> BuscarTorneosLlenos()
        {
            string str_id_organizador = User.FindFirst(ClaimTypes.Sid).Value;
            int.TryParse(str_id_organizador, out int id_organizador);

            IList<TorneoLlenoDTO> result = await buscarTorneosService.BuscarTorneosParaIniciar(id_organizador);

            if (result == null) return Ok(new { message = "No hay torneos llenos para aceptar inscripciones." });

            return Ok(result);
        }


        [HttpPost]
        [Route("/torneos/iniciar")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> IniciarTorneo(IniciarTorneoDTO dto)
        {
            int.TryParse(User.FindFirst(ClaimTypes.Sid).Value, out int id_organizador);


            await iniciarTorneoService.IniciarTorneo((int)dto.id_torneo, id_organizador);


            return Ok(new
            {
                message = "Se inició el torneo correctamente."
            });
        }



        //Endpoints para editar torneos:

        [HttpPost]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> AgregarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            //id organizador
            int.TryParse(User.FindFirst(ClaimTypes.Sid).Value, out int id_organizador);

            await agregarJuezService.AgregarJuez(id_organizador, id_torneo, (int)dto.id_juez);

            return Ok(new { message = $"Juez con ID [{dto.id_juez}] agregado con éxito al torneo [{id_torneo}]." });
        }

        [HttpDelete]
        [Route("/torneos/{id_torneo}/jueces")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EliminarJuezTorneo([FromRoute] int id_torneo, EditarJuezTorneoDTO dto)
        {
            //id organizador
            int.TryParse(User.FindFirst(ClaimTypes.Sid).Value, out int id_organizador);

            await eliminarJuezService.EliminarJuez(id_organizador, id_torneo, (int)dto.id_juez);

            return Ok(new { message = $"Juez [{dto.id_juez}] eliminado con éxito del torneo [{id_torneo}]." });
        }



        //Endpoints para editar partidas:

        [HttpPut]
        [Route("/partidas/juez")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EditarJuezPartida(EditarJuezPartidaDTO dto)
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_organizador);

            await editarJuezPartidaService.EditarJuezPartida(id_organizador, (int)dto.Id_partida, (int)dto.Id_juez);

            return Ok(new { message = $"Se editó el juez de la partida {dto.Id_partida} con éxito. El nuevo juez es {dto.Id_juez}" });
        }

        [HttpPut]
        [Route("/partidas/jugadores")]
        [Authorize(Roles = Roles.ORGANIZADOR)]
        public async Task<ActionResult> EditarJugadoresPartidas(EditarJugadoresPartidasDTO dto)
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_organizador);

            await editarJugadoresPartidasService.EditarJugadoresDePartidas(id_organizador, dto.editar_jugadores_partidas);

            return Ok(new { message = $"Se editaron los jugadores de las partidas con éxito." });
        }

    }
}
