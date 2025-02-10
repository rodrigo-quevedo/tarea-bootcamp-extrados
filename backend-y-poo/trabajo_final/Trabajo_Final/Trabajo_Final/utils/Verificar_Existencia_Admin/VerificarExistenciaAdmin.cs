using DAO.DAOs;
using Trabajo_Final.utils.Constantes;
using Isopoh.Cryptography.Argon2;
using Configuration;
using Microsoft.Extensions.Options;
using Configuration.DI;
using DAO.DAOs.Usuario;
using DAO.Entidades.Usuario;

namespace Trabajo_Final.utils.Verificar_Existencia_Admin
{
    public class VerificarExistenciaAdmin : IVerificarExistenciaAdmin
    {
        // cargar DAO y connectionString
        private IUsuarioDAO usuarioDAO;
        private IPrimer_AdminConfiguration _primerAdminConf;

        private Usuario nuevoAdmin;
        public VerificarExistenciaAdmin(IPrimer_AdminConfiguration injectedAdminData, IUsuarioDAO DI_usuarioDAO)
        {
            //Console.WriteLine("Adentro de VerificarExistenciaAdmin");

            //Console.WriteLine($"injectedAdminData: {injectedAdminData}");
            _primerAdminConf = injectedAdminData;

            nuevoAdmin = new Usuario(
                0,
                Roles.ADMIN,
                _primerAdminConf.Pais,
                _primerAdminConf.Nombre_apellido,
                _primerAdminConf.Email,
                _primerAdminConf.Password,
                true,
                null
            );

            //Console.WriteLine("Inyeccion de DAO ejecutandose...");
            usuarioDAO = DI_usuarioDAO;


            Usuario busqueda = new Usuario(0, Roles.ADMIN, null, null, null, null, true, null);
            //Console.WriteLine(busqueda);

            // verificar si existe admin
            Usuario adminExistente = usuarioDAO.BuscarUnUsuario(busqueda);
            if (adminExistente != null)
            {
                Console.WriteLine("Existe al menos un admin en la base de datos:");
                Console.WriteLine("" +
                    $"\n Id: {adminExistente.Id}" +
                    $"\n Rol: {adminExistente.Rol}" +
                    $"\n Pais: {adminExistente.Pais}" +
                    $"\n Nombre_apellido: {adminExistente.Nombre_apellido}" +
                    $"\n Email: {adminExistente.Email}" +
                    $"\n Activo: {adminExistente.Activo}"
                );
                return;
            }

            // si no existe, crearlo:


            //hashear password
            var passwordHash = Argon2.Hash(nuevoAdmin.Password);
            nuevoAdmin.Password = passwordHash;
            int rowInserts = usuarioDAO.CrearUsuario(nuevoAdmin);
            if (rowInserts == 1) { Console.WriteLine("Se creo un admin."); }

            //mostrarlo
            Usuario adminCreado = usuarioDAO.BuscarUnUsuario(new Usuario(0, Roles.ADMIN, null, null, null, null, true, null));
            if (adminCreado.Id != null)
            {
                Console.WriteLine(
                    $"\n Id: {adminCreado.Id}" +
                    $"\n Rol: {adminCreado.Rol}" +
                    $"\n Pais: {adminCreado.Pais}" +
                    $"\n Nombre_apellido: {adminCreado.Nombre_apellido}" +
                    $"\n Email: {adminCreado.Email}" +
                    $"\n Activo: {adminCreado.Activo}"
                );
                return;
            }
        }


        
    }
}
