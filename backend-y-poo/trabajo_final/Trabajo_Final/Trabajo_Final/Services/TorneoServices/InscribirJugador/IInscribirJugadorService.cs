namespace Trabajo_Final.Services.TorneoServices.InscribirJugador
{
    public interface IInscribirJugadorService
    {
        public Task<bool> Inscribir(int id_jugador, int id_torneo, int[] id_cartas_mazo);

    }
}
