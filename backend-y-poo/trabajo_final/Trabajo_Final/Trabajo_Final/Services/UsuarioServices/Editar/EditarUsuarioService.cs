using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.Custom.EditarUsuario;
using Isopoh.Cryptography.Argon2;
using Trabajo_Final.DTO.EditarUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Editar
{
    public class EditarUsuarioService: IEditarUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        public EditarUsuarioService(IUsuarioDAO usuarioDao)
        {
            usuarioDAO = usuarioDao;
        }

        public async Task<bool> EditarUsuario(int id_usuario, RequestBodyEditarUsuarioDTO dto)
        {
            DatosEditablesUsuarioDTO datosUsuario = 
                await usuarioDAO.BuscarDatosEditablesUsuario(id_usuario);

            if (datosUsuario == null) throw new InvalidInputException($"El usuario [{id_usuario}] no existe o no está activo.");

            DatosEditablesUsuarioDTO objetoUpdate = PrepararObjetoUpdate(dto, datosUsuario);

            return await usuarioDAO.EditarUsuario(objetoUpdate, new string[2] {Roles.JUGADOR, Roles.JUEZ} );
        }


        private DatosEditablesUsuarioDTO PrepararObjetoUpdate(
            RequestBodyEditarUsuarioDTO dto, DatosEditablesUsuarioDTO datosUsuario)
        {
            //usuario
            if (dto.Nombre_apellido != default) datosUsuario.Nombre_apellido = dto.Nombre_apellido;
            if (dto.Rol != default) datosUsuario.Rol = dto.Rol;
            if (dto.Pais != default) datosUsuario.Pais = dto.Pais;
            if (dto.Email != default) datosUsuario.Email = dto.Email;
            if (dto.Password != default) datosUsuario.Password = Argon2.Hash(dto.Password);

            //perfil
            if (dto.Foto != default) datosUsuario.Foto = dto.Foto;
            if (dto.Alias != default) datosUsuario.Alias = dto.Alias;

            return datosUsuario;
        }



    }
}
