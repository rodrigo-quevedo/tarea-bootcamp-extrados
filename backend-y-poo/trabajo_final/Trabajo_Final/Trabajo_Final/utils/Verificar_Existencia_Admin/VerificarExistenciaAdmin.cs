using DAO.DAOs;
using DAO.Entidades;
using Trabajo_Final.utils.Constantes;
using Isopoh.Cryptography.Argon2;

namespace Trabajo_Final.utils.Verificar_Existencia_Admin
{
    public class VerificarExistenciaAdmin
    {
        private UsuarioDAO usuarioDAO;
        public VerificarExistenciaAdmin(string connectionString, Usuario nuevoAdmin)
        {
            // cargar DAO y connectionString
            usuarioDAO = new(connectionString);

            // verificar si existe admin
            Usuario adminExistente = usuarioDAO.BuscarUnUsuario(new Usuario(0, Roles.ADMIN, null, null, null, null, true));
            if (adminExistente != null) {
                Console.WriteLine("Existe al menos un admin en la base de datos:");
                Console.WriteLine(""+
                    $"\n Id: {adminExistente.Id}"+
                    $"\n Rol: {adminExistente.Rol}"+
                    $"\n Pais: {adminExistente.Pais}"+
                    $"\n Nombre_apellido: {adminExistente.Nombre_apellido}"+
                    $"\n Email: {adminExistente.Email}"+
                    $"\n Activo: {adminExistente.Activo}"
                );
                return; 
            }

            // si no existe

                 // crearlo

                    //hashear password
                    var passwordHash = Argon2.Hash(nuevoAdmin.Password);
                    nuevoAdmin.Password = passwordHash;
            int rowInserts = usuarioDAO.CrearUsuario(nuevoAdmin);
            if (rowInserts == 1) { Console.WriteLine("Se creo un admin."); }

                 //mostrarlo
            Usuario adminCreado = usuarioDAO.BuscarUnUsuario(new Usuario(0, Roles.ADMIN, null, null, null, null, true));
            if (adminCreado.Id != null)
            {
                Console.WriteLine(
                    $"\n Id: {adminCreado.Id}" +
                    $"\n Rol: {adminCreado.Rol}"+
                    $"\n Pais: {adminCreado.Pais}"+
                    $"\n Nombre_apellido: {adminCreado.Nombre_apellido}"+
                    $"\n Email: {adminCreado.Email}"+
                    $"\n Activo: {adminCreado.Activo}"
                );
                return;
            }

        }
           
    }
}
