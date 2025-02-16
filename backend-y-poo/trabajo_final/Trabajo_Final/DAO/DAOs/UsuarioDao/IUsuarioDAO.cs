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

        public int CrearUsuario(Usuario usuario);
        public Task<int> CrearUsuarioAsync(Usuario usuario);
        public Task<int> CrearUsuarioAsync(Usuario usuario, int id_usuario_creador);

        public Task<IEnumerable<int>> BuscarIDsUsuarios(Usuario busqueda);
        public Usuario BuscarUnUsuario(Usuario usuario);
        public Task<Usuario> BuscarUnUsuarioAsync(Usuario usuario);

        public Task<int> GuardarRefreshTokenAsync(int id, string refreshToken);
        public Task<int> BorradoLogicoRefreshTokenAsync(int id, string refreshToken);
        public Task<Refresh_Token> BuscarRefreshTokenAsync(string refreshToken, bool activo);


        public Task<string> ActualizarPerfil(int id_usuario, string url_foto, string alias);
        public Task<string> ActualizarFotoPerfil(int id_usuario, string url_foto);
        public Task<string> ActualizarAliasPerfil(int id_usuario, string alias);

    }
}
