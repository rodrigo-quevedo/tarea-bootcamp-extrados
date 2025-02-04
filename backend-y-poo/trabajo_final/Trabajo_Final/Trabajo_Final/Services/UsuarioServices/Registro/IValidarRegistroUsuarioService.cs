using Configuration.DI;
using DAO.DAOs.DI;
using DAO.Entidades;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public interface IValidarRegistroUsuarioService
    {
        public Usuario ValidarRegistroUsuario(DatosRegistroDTO datosUsuarioARegistrar, string jwt);
    }
}
