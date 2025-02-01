using DAO.Entidades;
using Trabajo_Final.DTO;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public interface IJugadorAutoregistroService
    {
        public Usuario AutoregistroJugador(DatosRegistroDTO datos);
    }
}
