using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.ValidarJueces
{
    public class ValidarJuecesService : IValidarJuecesService
    {
        private IUsuarioDAO usuarioDAO;
        public ValidarJuecesService(IUsuarioDAO usuarioDao) { 
            usuarioDAO = usuarioDao;
        }

        public async Task<bool> ValidarIdsJueces(int[] id_jueces)
        {
            //Tengo que validar que todos los id pertenecen a jueces activos
            //(ya que no tengo tabla jueces, y el id_juez apunta a usuarios(id),
            //el INSERT va a guardar cualquier ID).
            IEnumerable<int> id_jueces_validos =
                await usuarioDAO.BuscarIDsUsuarios(new Usuario() { Rol = Roles.JUEZ, Activo = true });

            foreach (int id_juez in id_jueces)
            {
                if (!id_jueces_validos.Contains(id_juez))
                    throw new InvalidInputException($"No hay ningun juez con id [{id_juez}].");
            }

            return true;
        }

    }
}
