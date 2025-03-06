using DAO.DAOs.UsuarioDao;
using Isopoh.Cryptography.Argon2;
using Custom_Exceptions.Exceptions.Exceptions;
using Constantes.Constantes;
using Trabajo_Final.DTO.Request.InputUsuarios;
using DAO.DTOs_en_DAOs.RegistroUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public class RegistroUsuarioService: IRegistroUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        public RegistroUsuarioService(IUsuarioDAO usuarioDAO)
        {
            this.usuarioDAO = usuarioDAO;
        }


        //inputs ya vienen validados desde el DTO
        public async Task<bool> RegistrarUsuario(DatosRegistroDTO datos, int? id_usuario_creador)
        {

            DatosRegistroUsuarioDTO nuevoUsuario = new DatosRegistroUsuarioDTO()
            {
                activo = true,

                nombre_apellido = datos.nombre_apellido,
                rol = datos.rol,
                pais = datos.pais,
                email = datos.email,
                password = Argon2.Hash(datos.password),

                foto = (datos.rol == Roles.JUEZ || datos.rol == Roles.JUGADOR) ? datos.foto : null,
                alias = (datos.rol == Roles.JUEZ || datos.rol == Roles.JUGADOR) ? datos.alias : null,
                
                id_usuario_creador = id_usuario_creador
            };


            //Datos del perfil en jugadores y jueces son obligatorios, no pueden ser null (default)
            if (
                (nuevoUsuario.rol == Roles.JUGADOR || nuevoUsuario.rol == Roles.JUEZ)
                &&
                (nuevoUsuario.alias == default || nuevoUsuario.foto == default)
            )
                throw new InvalidInputException($"Campos 'alias' y 'foto' son obligatorios para crear un usuario con rol [{nuevoUsuario.rol}].");


            return await usuarioDAO.CrearUsuarioAsync(nuevoUsuario);
        }


    }
}
