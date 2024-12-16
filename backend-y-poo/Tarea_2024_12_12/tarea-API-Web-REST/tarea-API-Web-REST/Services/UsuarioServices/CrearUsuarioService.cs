using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using Isopoh.Cryptography.Argon2;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class CrearUsuarioService
    {
        UsuarioDAO usuarioDAO { get; set; }
        public CrearUsuarioService(string connectionString)
        {
            usuarioDAO = new UsuarioDAO(connectionString);
        }

        public Usuario CrearUsuario(Usuario usuario)
        {
            //chequear que el usuario no existe
            Usuario usuarioEncontrado = usuarioDAO.BuscarUsuarioPorMail(usuario.mail);

            if (usuarioEncontrado != null)
            {
                Console.WriteLine("El usuario ya existe:");
                usuarioEncontrado.mostrarDatos();

                throw new AlreadyExistsException($"No se puede crear el usuario con mail '{usuario.mail}' porque ya existe.");
            }

            //hash password

            var passwordHash = Argon2.Hash(usuario.password);
            Console.WriteLine($"password anterior: {usuario.password}");
            Console.WriteLine($"password nueva: {passwordHash}");

            Usuario nuevoUsuario = new Usuario(
                usuario.mail,
                usuario.nombre,
                usuario.edad,
                usuario.username,
                passwordHash,
                usuario.role,
                null
            );

            //crear usuario
            usuarioDAO.CrearUsuario(nuevoUsuario);


            //corroborar que se creo el usuario
            Usuario usuarioCreado = usuarioDAO.BuscarUsuarioPorMail(usuario.mail);
            if (usuarioCreado == null) throw new NotFoundException($"Error al crear: el usuario con mail '{usuario.mail}' no se encuentra en la Base de Datos.");

            usuarioCreado.mostrarDatos();

            return usuarioCreado;
        }
    }
}
