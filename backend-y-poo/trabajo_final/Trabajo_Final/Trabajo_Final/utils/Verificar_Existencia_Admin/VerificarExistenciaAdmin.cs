using DAO.DAOs;
using DAO.Entidades;
using Trabajo_Final.utils.Constantes;
using Isopoh.Cryptography.Argon2;
using Configuration;
using Microsoft.Extensions.Options;
using DAO.DAOs.DI;

namespace Trabajo_Final.utils.Verificar_Existencia_Admin
{
    public class VerificarExistenciaAdmin
    {
        // cargar DAO y connectionString

        //private IUsuarioDAO usuarioDAO;
        private IUsuarioDAO usuarioDAO;
        
        private Usuario nuevoAdmin;
        private Primer_AdminConfiguration _primerAdminConf;

        //public VerificarExistenciaAdmin(IOptions<Primer_AdminConfiguration> options, IUsuarioDAO usuarioDaoConf) 
        public VerificarExistenciaAdmin(IOptions<Primer_AdminConfiguration> options)
        {
            Console.WriteLine("Adentro de VerificarExistenciaAdmin");

            Console.WriteLine($"options: {options}");
            _primerAdminConf = options.Value;

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

            
        }


        public VerificarExistenciaAdmin()
        {
            Console.WriteLine($"Mi usuarioDAO: {usuarioDAO}");

            // verificar si existe admin
            Usuario adminExistente = usuarioDAO.BuscarUnUsuario(new Usuario(0, Roles.ADMIN, null, null, null, null, true, null));
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
            Usuario adminCreado = usuarioDAO.BuscarUnUsuario(new Usuario(0, Roles.ADMIN, null, null, null, null, true));
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

        public VerificarExistenciaAdmin(IUsuarioDAO DI_usuarioDAO)
        {
            usuarioDAO = DI_usuarioDAO;
        }

       




    }
}
