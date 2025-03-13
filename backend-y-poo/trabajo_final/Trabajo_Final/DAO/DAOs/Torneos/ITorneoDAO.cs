using DAO.DTOs_en_DAOs.Ganador_Torneo;
using DAO.DTOs_en_DAOs.InsertPartidas;
using DAO.Entidades.PartidaEntidades;
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
            int max_cantidad_rondas,
            string pais,
            string fase,
            string[] series_habilitadas,
            int[] id_jueces,
            string rolJuez);

        public Task<int> AgregarJuez(
            int id_organizador,
            int id_torneo, 
            int id_juez, 
            string rol, 
            string faseRegistro);
        public Task<int> EliminarJuez(
            int id_organizador,
            int id_torneo, 
            int id_juez,
            string faseInvalida);


        public Task<Torneo> BuscarTorneoActivo(Torneo busqueda);
        public Task<IEnumerable<Torneo>> BuscarTorneos(IList<int> id_torneos);
        public Task<IEnumerable<Torneo>> BuscarTorneosActivos(Torneo busqueda);
        public Task<IEnumerable<Torneo>> BuscarTorneos(string[] fases);
        public Task<IEnumerable<Torneo>> BuscarTorneosOrganizados(string[] fases, int id_organizador);
        public Task<IEnumerable<Torneo>> BuscarTorneosParaIniciar(string faseInscripcion, int id_organizador);
        public Task<IEnumerable<int>> BuscarIdTorneosOficializados(int id_juez, string faseFinalizado);
        public Task<IEnumerable<Serie_Habilitada>> BuscarSeriesDeTorneos(IEnumerable<Torneo> torneos);
        public Task<IEnumerable<Juez_Torneo>> BuscarJuecesDeTorneos(IEnumerable<Torneo> torneos);
        public Task<IEnumerable<Juez_Torneo>> BuscarJuecesDeTorneo(int id_organizador, int id_partida);
        public Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresInscriptos(IEnumerable<Torneo> torneos);
        public Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresInscriptos(int id_torneo);
        public Task<IEnumerable<Jugador_Inscripto>> BuscarJugadoresAceptados(IEnumerable<Torneo> torneos);
        public Task<IEnumerable<GanadorTorneo>> BuscarGanadoresTorneos(IEnumerable<Torneo> torneos);

        public Task<bool> InscribirJugador(
            int id_jugador, string rol_jugador,
            int id_torneo, string fase_inscripcion,
            int[] id_cartas_mazo);

        public Task<bool> ActualizarJugadoresYCantidadRondas(int id_torneo, IEnumerable<int> id_jugadores, int cantidad_rondas);

        public Task<bool> IniciarTorneo(
            string faseTorneo,
            int id_torneo,
            IList<InsertPartidaDTO> partidas_primera_ronda);


        public Task<bool> CancelarTorneo(
            int id_admin,
            int id_torneo,
            string? motivo,
            DateTime now,
            string faseFinalizado);

    }
}
