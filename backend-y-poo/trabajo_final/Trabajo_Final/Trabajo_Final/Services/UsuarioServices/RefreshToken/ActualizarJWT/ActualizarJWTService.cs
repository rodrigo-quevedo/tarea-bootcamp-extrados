using DAO.DAOs.Usuario;
using DAO.Entidades.Usuario;
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
            //Por definición, si el refreshToken del usuario está en la DB, es válido.
            //Y el usuario que está asociado a ese refreshToken también es válido.
    
            Refresh_Token refreshTokenEncontrado = usuarioDAO.BuscarRefreshToken(refreshToken, true);
            if (refreshTokenEncontrado == null)
            {
                Console.WriteLine($"No se encontró el refreshToken: {refreshToken}");
                throw new NotFoundException("Su sesión no está activa. Inicie sesión nuevamente.");
            }

            Usuario busqueda = new Usuario(refreshTokenEncontrado.id_usuario, true);
            Usuario usuarioEncontrado = usuarioDAO.BuscarUnUsuario(busqueda);
            if (usuarioEncontrado == null) throw new Exception($"No se pudo obtener el usuario id [{refreshTokenEncontrado.id_usuario}] en la base de datos.");

            string jwt = crearJwtService.CrearJwt(usuarioEncontrado);

            return jwt;
        }


    }
}
