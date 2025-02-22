using DAO.Entidades.PartidaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Partidas
{
    public interface IPartidaDAO
    {
        public Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez);
    }
}
