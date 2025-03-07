using DAO.DTOs_en_DAOs.InsertPartidas;
using DAO.DTOs_en_DAOs.JugadoresPartidas;
using DAO.DTOs_en_DAOs.Partida_CantidadRondas;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;

namespace DAO.DAOs.Partidas
{
    public interface IPartidaDAO
    {
        public Task<IEnumerable<Partida>> BuscarPartidasDelTorneo(IEnumerable<Torneo> torneos);

        public Task<Partida_CantidadRondasDTO> BuscarDatosParaOficializar(Partida partida);
        public Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez);
        public Task<IEnumerable<Partida>> BuscarPartidasOficializadasDelTorneo(int id_juez, IEnumerable<Torneo> torneos);

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

        public Task<IEnumerable<Partida>> BuscarFinalesGanadas(int id_jugador);

        public Task<bool> EditarJuezPartida(int id_partida, int id_nuevo_juez);

        public Task<int> BuscarIdTorneoVerificandoPartidas(
            IEnumerable<int> id_partidas, 
            int id_organizador,
            int ronda);

        public Task<IEnumerable<Partida>> BuscarPartidasPrimeraRonda(int id_torneo);

        public Task<bool> EditarJugadoresPartidas(IEnumerable<JugadoresPartida> jugadores_partidas);

    }
}
