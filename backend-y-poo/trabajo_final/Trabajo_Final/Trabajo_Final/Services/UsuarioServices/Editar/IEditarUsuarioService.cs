using Trabajo_Final.DTO.Request.InputUsuarios;

namespace Trabajo_Final.Services.UsuarioServices.Editar
{
    public interface IEditarUsuarioService
    {
        public Task<bool> EditarUsuario(int id_usuario, EditarUsuarioDTO dto);
    }
}
