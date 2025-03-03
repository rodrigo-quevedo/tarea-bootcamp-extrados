using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.Custom.EditarUsuario
{
    public class DatosEditablesUsuarioDTO
    {
        public int Id {  get; set; }
        public string Nombre_apellido { get; set; }
        public string Password { get; set; }
        public string Pais { get; set; }
        public string Foto { get; set; }
        public string Alias { get; set; }


        //Esto no se si conviene que se pueda editar:
        public string Rol { get; set; }
        public string Email { get; set; }
    }
}
