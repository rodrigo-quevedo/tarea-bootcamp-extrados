using DAO.DAOs.DI;
using DAO.Entidades;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.utils.Exceptions.Exceptions;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Validar
{
    public class ActualizarJWTService : IActualizarJWTService
    {
        private IUsuarioDAO usuarioDAO;
        private ICrearJwtService crearJwtService;
        public ActualizarJWTService(IUsuarioDAO dao, ICrearJwtService crearJwt)
        {
            usuarioDAO = dao;
            crearJwtService = crearJwt;
        }


        public string ActualizarJWT(string refreshToken)
        {
            Refresh_Token refreshTokenEncontrado = usuarioDAO.BuscarRefreshToken(refreshToken, true);
            if (refreshTokenEncontrado == null) throw new NotFoundException("Su sesión no está activa.");
            Console.WriteLine($"No se encontró refreshToken para el refreshToken: {refreshToken}.");

            //Por definición, si el refreshToken del usuario está en la DB, es válido.
            //Y el usuario que está asociado a ese refreshToken también es válido.


            Usuario busqueda = new Usuario(refreshTokenEncontrado.id_usuario, true);
            Usuario usuarioEncontrado = usuarioDAO.BuscarUnUsuario(busqueda);

            string jwt = crearJwtService.CrearJwt(usuarioEncontrado);

            return jwt;
        }
    }
}
