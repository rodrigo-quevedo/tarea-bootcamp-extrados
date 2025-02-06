using DAO.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.DI
{
    public interface IUsuarioDAO
    {

        public int CrearUsuario(Usuario usuario);
        public int CrearUsuario(Usuario usuario, int id_usuario_creador);
        public Usuario BuscarUnUsuario(Usuario usuario);

        public int GuardarRefreshToken(int id, string refreshToken);
        public int BorradoLogicoRefreshToken(int id, string refreshToken);
        public Refresh_Token BuscarRefreshToken(string refreshToken, bool activo);

    }
}
