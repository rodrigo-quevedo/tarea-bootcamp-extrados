using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Isopoh.Cryptography.Argon2;
using System.Security.Claims;
using Trabajo_Final.DTO;
using Trabajo_Final.utils.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;

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
        public async Task<bool> RegistrarUsuario(DatosRegistroDTO datos)
        {
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

            //aca se puede hacer catch y tirar una custom exception segun el resultado
            //(ej. falló el INSERT por mail repetido o falló INSERT por network error)
            int filasDeTablaAfectadas = await usuarioDAO.CrearUsuarioAsync(nuevoUsuario);
            if (filasDeTablaAfectadas == 0) throw new DefaultException($"No se pudo crear el usuario [{nuevoUsuario.Email}].");
            
            Console.WriteLine($"Se registró al usuario [{nuevoUsuario.Email}] con éxito.");

            return true;
        }

        public async Task<bool> RegistrarUsuario(DatosRegistroDTO datos, int id_usuario_creador)
        {
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

            int filasDeTablaAfectadas = await usuarioDAO.CrearUsuarioAsync(nuevoUsuario, id_usuario_creador);
            if (filasDeTablaAfectadas == 0) throw new DefaultException($"No se pudo crear el usuario [{nuevoUsuario.Email}].");
            
            Console.WriteLine($"Se registró al usuario [{nuevoUsuario.Email}] con éxito.");

            return true;
        }


        public bool RegistrarUsuario(DatosRegistroDTO datos, int id_usuario_creador, string rol_usuario_creador)
        {
            return true;
        }

    }
}
