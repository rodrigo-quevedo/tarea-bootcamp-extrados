using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trabajo_Final.Services.UsuarioServices.Registro;
using Custom_Exceptions.Exceptions.Exceptions;
using Trabajo_Final.utils.Verificar_Existencia_Admin;
using Trabajo_Final.Services.UsuarioServices.Perfil;
using Trabajo_Final.Services.UsuarioServices.Eliminar;
using Trabajo_Final.Services.UsuarioServices.Editar;
using Trabajo_Final.Services.UsuarioServices.Buscar;
using Constantes.Constantes;
using Trabajo_Final.DTO.Request.InputUsuarios;

namespace Web_API.Controllers.Usuarios
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        // services
        private readonly IRegistroUsuarioService registroUsuarioService;
        private readonly IActualizarPerfilService actualizarPerfilService;
        private readonly IEliminarUsuarioService eliminarUsuarioService;
        private readonly IEditarUsuarioService editarUsuarioService;
        private readonly IBuscarUsuarioService buscarUsuarioService;

        public UsuariosController(
            VerificarExistenciaAdmin verificarAdmin, //Cuando se crea el controller, se hace una verificación automática. Está en UsuariosController por si un usuario no logeado hace un autoregistro de jugador.

            IRegistroUsuarioService registroUsuarioService,
            IActualizarPerfilService actualizarPerfilService,
            IEliminarUsuarioService eliminarUsuarioService,
            IEditarUsuarioService editarUsuarioService,
            IBuscarUsuarioService buscarUsuarioService
        )
        {
            this.registroUsuarioService = registroUsuarioService;
            this.actualizarPerfilService = actualizarPerfilService;
            this.eliminarUsuarioService = eliminarUsuarioService;
            this.editarUsuarioService = editarUsuarioService;
            this.buscarUsuarioService = buscarUsuarioService;
        }



        //Endpoints para creación de usuarios:

        [HttpPost]
        [Route("/registro")]
        //Si usuario no está logeado, hacer autoregistro de JUGADOR:
        public async Task<ActionResult> AutoRegistrarJugador(DatosRegistroDTO datosUsuarioARegistrar)
        {
            Console.WriteLine("POST /registro");

            string authorizationHeaderValue = Request.Headers["Authorization"].ToString();

            if (authorizationHeaderValue != null && authorizationHeaderValue != "")
                throw new SinPermisoException($"Debe cerrar sesión para registrarse como jugador.");

            if (datosUsuarioARegistrar.rol != Roles.JUGADOR)
                throw new SinPermisoException("Solo se crean usuarios con rol JUGADOR en /registro");


            await registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar, null);

            return Ok(new { message = $"Usuario [{datosUsuarioARegistrar.email}] se autoregistró con éxito." });
        }


        [HttpPost]
        [Route("/crear")]
        [Authorize(Roles = $"{Roles.ADMIN},{Roles.ORGANIZADOR}")]
        //Si está logeado, registrar a otro usuario:
        public async Task<ActionResult> RegistrarUsuario(DatosRegistroDTO datosUsuarioARegistrar)
        {
            //verificar que organizador crea juez, ya que admin puede crear todo
            string rol_usuario_creador = User.FindFirst(ClaimTypes.Role).Value;

            if (rol_usuario_creador == Roles.ORGANIZADOR
                &&
                datosUsuarioARegistrar.rol != Roles.JUEZ
            )
                throw new SinPermisoException($"Intentó crear un '{datosUsuarioARegistrar.rol}' pero no tiene permisos. Está logeado como Organizador y solo puede crear usuarios con rol JUEZ.");


            //registrar            
            string id_string = User.FindFirst(ClaimTypes.Sid).Value;
            int.TryParse(id_string, out int id_usuario_creador);

            await registroUsuarioService.RegistrarUsuario(datosUsuarioARegistrar, id_usuario_creador);

            return Ok(new { message = $"Usuario [{datosUsuarioARegistrar.email}] se registró con éxito." });
        }



        //Endpoints para eliminar y editar jugadores (solo ADMIN):

        [HttpDelete]
        [Route("/usuarios/{id_usuario}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> EliminarUsuario([FromRoute] int id_usuario)
        {
            await eliminarUsuarioService.EliminarUsuario(id_usuario);

            return Ok(new { message = $"Se eliminó al usuario [{id_usuario}] con éxito." });
        }

        [HttpPut]
        [Route("/usuarios/{id_usuario}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> EditarUsuario([FromRoute] int id_usuario, [FromForm] EditarUsuarioDTO dto)
        {
            await editarUsuarioService.EditarUsuario(id_usuario, dto);

            return Ok(new { message = $"Se editó al usuario [{id_usuario}] con éxito." });
        }



        //Endpoints para perfil de jugadores/jueces:

        [HttpPut]
        [Route("/perfil")]
        [Authorize(Roles = $"{Roles.JUGADOR},{Roles.JUEZ}")]
        public async Task<ActionResult> ActualizarPerfil([FromForm] ActualizarPerfilDTO dto)
        {
            //id usuario
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_usuario);

            //manejo de string vacio "", ya que data annotation [RegEx] lo deja pasar como valido:
            if (dto.alias == "") throw new InvalidInputException("Alias incorrecto. Caracteres válidos: letras, números. Entre 4 y 25 caracteres.");

            string response = await actualizarPerfilService.ActualizarPerfil(
                id_usuario, dto.foto, dto.alias);

            return Ok(new { message = response });
        }



        //Endpoint para obtener detalles de jugadores (la información varía según el rol):

        [HttpGet]
        [Route("/usuarios/{id_usuario}")]
        [Authorize]
        public async Task<ActionResult> BuscarDatosUsuario([FromRoute] int id_usuario)
        {
            //datos usurio logueado (necesarios para saber sus permisos)
            string rol_logueado = User.FindFirst(ClaimTypes.Role).Value;
            int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out int id_logeado);


            if (rol_logueado == Roles.ADMIN || rol_logueado == Roles.ORGANIZADOR)
                return Ok(new
                {
                    usuario = await buscarUsuarioService.BuscarDatosCompletosUsuario(
                        id_logeado, rol_logueado, id_usuario)
                });


            else return Ok(new
            {
                usuario = await buscarUsuarioService.BuscarPerfilUsuario(
                    id_logeado, rol_logueado, id_usuario)
            });

        }


    }
}
