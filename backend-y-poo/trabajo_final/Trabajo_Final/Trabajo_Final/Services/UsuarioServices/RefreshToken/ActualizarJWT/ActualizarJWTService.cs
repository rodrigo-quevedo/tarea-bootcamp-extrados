using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Custom_Exceptions.Exceptions.Exceptions;

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


        public async Task<string> ActualizarJWT(string refreshToken)
        {
            //Por definición, si el refreshToken del usuario está en la DB, es válido.
            //Y el usuario que está asociado a ese refreshToken también es válido.
    
            Refresh_Token refreshTokenEncontrado = await usuarioDAO.BuscarRefreshTokenAsync(refreshToken, true);
            if (refreshTokenEncontrado == null)
            {
                Console.WriteLine($"No se encontró el refreshToken: {refreshToken}");
                throw new InvalidRefreshTokenException("Su sesión no está activa. Inicie sesión nuevamente.");
            }

            Usuario busqueda = new Usuario(refreshTokenEncontrado.id_usuario, true);
            Usuario usuarioEncontrado = await usuarioDAO.BuscarUnUsuarioAsync(busqueda);
            if (usuarioEncontrado == null) throw new Exception($"No se pudo obtener el usuario id [{refreshTokenEncontrado.id_usuario}] en la base de datos.");

            string jwt = crearJwtService.CrearJwt(usuarioEncontrado);

            return jwt;
        }


    }
}
