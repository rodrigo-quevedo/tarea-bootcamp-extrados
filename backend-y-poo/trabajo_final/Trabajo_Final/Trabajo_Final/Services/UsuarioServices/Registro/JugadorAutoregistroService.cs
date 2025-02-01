using DAO.DAOs.DI;
using DAO.Entidades;
using Isopoh.Cryptography.Argon2;
using Trabajo_Final.DTO;
using Trabajo_Final.utils.Constantes;
using Trabajo_Final.utils.Exceptions.Exceptions;

namespace Trabajo_Final.Services.UsuarioServices.Registro
{
    public class JugadorAutoregistroService: IJugadorAutoregistroService
    {
        private IUsuarioDAO usuarioDAO;
        public JugadorAutoregistroService(IUsuarioDAO usuarioDAO)
        {
            this.usuarioDAO = usuarioDAO;
        }


        //inputs ya vienen validados desde el DTO
        public Usuario AutoregistroJugador(DatosRegistroDTO datos)
        {
            //verificar si el mail ya está usado
            Usuario busqueda = new Usuario(0, null, null, null, datos.email, null, true, null);
            
            Usuario usuarioConMailExistente = usuarioDAO.BuscarUnUsuario(busqueda);
            
            if (usuarioConMailExistente != null) { throw new AlreadyExistsException($"El usuario con mail [{datos.email}] ya existe."); }


            //registrar jugador
            string hashedPassword = Argon2.Hash( datos.password );
            Usuario nuevoJugador = new Usuario(
                0, 
                datos.rol, 
                datos.pais, 
                datos.nombre_apellido, 
                datos.email, 
                hashedPassword, 
                true, 
                null
            );

            int rows = usuarioDAO.CrearUsuario(nuevoJugador);
            if (rows == 0) throw new Exception("No se pudo crear el jugador.");
            Console.WriteLine("Se registró al jugador con éxito.");


            //buscar jugador creado y retornarlo
            Usuario nuevoJugadorCreado = usuarioDAO.BuscarUnUsuario(busqueda);
            if (nuevoJugadorCreado == null) { throw new NotFoundException($"No se encontró el jugador con mail {busqueda.Email} recién registrado."); }

            return nuevoJugadorCreado;
        }
    }
}
