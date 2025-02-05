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

        public int AsignarRefreshTokenById(int id, string refreshToken);

    }
}
