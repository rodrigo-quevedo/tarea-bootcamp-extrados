using DAO.Entidades.TorneoEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Torneos
{
    public interface ITorneoDAO
    {
        public Task<bool> CrearTorneo(
            int id_organizador,
            DateTime fecha_hora_inicio, DateTime? fecha_hora_fin,
            string horario_inicio, string horario_fin,
            int? cantidad_rondas,
            string pais,
            string fase,
            string[] series_habilitadas,
            int[] id_jueces
        );

        public Task<int> AgregarJuez(int id_torneo, int id_juez, string rol, string faseInvalida);
        public Task<int> EliminarJuez(int id_torneo, int id_juez);


        public Task<IEnumerable<Torneo>> BuscarTorneos(Torneo busqueda);
        public Task<IEnumerable<Serie_Habilitada>> BuscarSeriesDeTorneos(IEnumerable<Torneo> torneos);
        public Task<IEnumerable<Juez_Torneo>> BuscarJuecesDeTorneos(IEnumerable<Torneo> torneos);

    }
}
