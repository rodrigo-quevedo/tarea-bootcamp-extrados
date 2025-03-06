namespace DAO.DTOs_en_DAOs.DatosUsuario
{

    public class DatosCompletosUsuarioDTO : PerfilUsuarioDTO
    {
        public string Nombre_apellido { get; set; }
        public string Pais { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public int Id_usuario_creador { get; set; }

        //La password está hasheada y no se puede leer, solo se puede verificar contra las
        //passwords que ingresa usuario, no tiene sentido leerla

        //public string Password {  get; set; }

    }



}
