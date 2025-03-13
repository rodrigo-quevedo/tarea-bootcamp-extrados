namespace DAO.DTOs_en_DAOs.RegistroUsuario
{
    public class DatosRegistroUsuarioDTO
    {
        public string email { get; set; }
        public string password { get; set; }
        public string rol { get; set; }
        public string pais { get; set; }
        public string nombre_apellido { get; set; }
        public bool activo { get; set; }

        public string foto { get; set; }
        public string alias { get; set; }

        public int? id_usuario_creador { get; set; }
    }
}
