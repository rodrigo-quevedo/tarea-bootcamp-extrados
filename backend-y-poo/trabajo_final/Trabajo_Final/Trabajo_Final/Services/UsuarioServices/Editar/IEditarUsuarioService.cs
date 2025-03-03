using Trabajo_Final.DTO.EditarUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Editar
{
    public interface IEditarUsuarioService
    {
        public Task<bool> EditarUsuario(int id_usuario, RequestBodyEditarUsuarioDTO dto);
    }
}
