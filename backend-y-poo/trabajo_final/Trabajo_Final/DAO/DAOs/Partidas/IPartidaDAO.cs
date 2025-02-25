using DAO.Entidades.Custom;
using DAO.Entidades.Custom.Descalificaciones;
using DAO.Entidades.Custom.Partida_CantidadRondas;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Partidas
{
    public interface IPartidaDAO
    {
        public Task<Partida_CantidadRondasDTO> BuscarDatosParaOficializar(Partida partida);
        public Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez);

        public Task<bool> VerificarUltimaPartidaDeRonda(
            int id_partida,
            int id_torneo,
            int ronda);

        public Task<bool> OficializarResultado(
            int id_partida, 
            int id_ganador, 
            int? id_descalificado,
            string motivo_descalificacion);

        public Task<bool> OficializarFinal(
            int id_partida, 
            int id_ganador, 
            int? id_descalificado, string motivo_descalificacion,
            int id_torneo,
            string faseFinalizado);

        public Task<bool> OficializarUltimaPartidaDeRonda(
            int id_partida,
            int id_ganador, 
            int? id_descalificado,string motivo_descalificacion,
            IEnumerable<InsertPartidaDTO> partidas);


        public Task<IEnumerable<Partida>> BuscarJugadoresGanadores(
            int id_torneo, 
            int ronda, 
            int cantidad_ganadores);


        public Task<IEnumerable<Partida>> BuscarDescalificaciones(int id_jugador);
        public Task<IEnumerable<Partida>> BuscarPartidasGanadas(int id_jugador);
        public Task<IEnumerable<Partida>> BuscarPartidasPerdidas(int id_jugador);
    }
}
