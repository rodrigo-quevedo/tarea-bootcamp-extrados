using DAO.DAOs.DI;
using DAO.Entidades;
using Isopoh.Cryptography.Argon2;
using Trabajo_Final.DTO;
using Trabajo_Final.utils.Constantes;
using Trabajo_Final.utils.Exceptions.Exceptions;

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
        public Usuario RegistrarUsuario(DatosRegistroDTO datos)
        {
            //verificar si el mail ya está usado
            Usuario busqueda = new Usuario(datos.email, true);
            
            Usuario usuarioConMailExistente = usuarioDAO.BuscarUnUsuario(busqueda);
            
            if (usuarioConMailExistente != null) { throw new AlreadyExistsException($"El usuario con mail [{datos.email}] ya existe."); }


            //registrar usuario
            string hashedPassword = Argon2.Hash( datos.password );
            Usuario nuevoUsuario = new Usuario(
                0, 
                datos.rol, 
                datos.pais, 
                datos.nombre_apellido, 
                datos.email, 
                hashedPassword, 
                true, 
                null,
                null
            );

            int rows = usuarioDAO.CrearUsuario(nuevoUsuario);
            if (rows == 0) throw new Exception($"No se pudo crear el usuario [{nuevoUsuario.Email}].");
            Console.WriteLine($"Se registró al usuario [{nuevoUsuario.Email}] con éxito.");


            //buscar jugador creado y retornarlo
            Usuario nuevoJugadorCreado = usuarioDAO.BuscarUnUsuario(busqueda);
            if (nuevoJugadorCreado == null) { throw new NotFoundException($"No se encontró el usuario con mail {busqueda.Email}, pero fue registrado con éxito."); }

            return nuevoJugadorCreado;
        }

        public Usuario RegistrarUsuario(DatosRegistroDTO datos, int id_usuario_creador)
        {
            //verificar si el mail ya está usado
            Usuario busqueda = new Usuario(datos.email, true);

            Usuario usuarioConMailExistente = usuarioDAO.BuscarUnUsuario(busqueda);

            if (usuarioConMailExistente != null) { throw new AlreadyExistsException($"El usuario con mail [{datos.email}] ya existe."); }


            //registrar usuario
            string hashedPassword = Argon2.Hash(datos.password);
            Usuario nuevoUsuario = new Usuario(
                0,
                datos.rol,
                datos.pais,
                datos.nombre_apellido,
                datos.email,
                hashedPassword,
                true,
                null,
                null
            );

            int rows = usuarioDAO.CrearUsuario(nuevoUsuario, id_usuario_creador);
            if (rows == 0) throw new Exception($"No se pudo crear el usuario [{nuevoUsuario.Email}].");
            Console.WriteLine($"Se registró al usuario [{nuevoUsuario.Email}] con éxito.");


            //buscar jugador creado y retornarlo
            Usuario nuevoJugadorCreado = usuarioDAO.BuscarUnUsuario(busqueda);
            if (nuevoJugadorCreado == null) { throw new NotFoundException($"No se encontró el usuario con mail {busqueda.Email}, pero fue registrado con éxito."); }

            return nuevoJugadorCreado;
        }
    }
}
