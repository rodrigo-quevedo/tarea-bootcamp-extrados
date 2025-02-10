using DAO.Entidades.UsuarioEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.UsuarioDao
{
    public interface IUsuarioDAO
    {

        public Task<int> CrearUsuario(Usuario usuario);
        public Task<int> CrearUsuario(Usuario usuario, int id_usuario_creador);
        public Task<Usuario> BuscarUnUsuario(Usuario usuario);

        public Task<int> GuardarRefreshToken(int id, string refreshToken);
        public Task<int> BorradoLogicoRefreshToken(int id, string refreshToken);
        public Task<Refresh_Token> BuscarRefreshToken(string refreshToken, bool activo);

    }
}
