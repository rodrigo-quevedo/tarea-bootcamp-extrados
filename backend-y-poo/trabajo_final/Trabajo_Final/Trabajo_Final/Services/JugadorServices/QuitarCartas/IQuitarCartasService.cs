namespace Trabajo_Final.Services.JugadorServices.QuitarCartas
{
    public interface IQuitarCartasService
    {
        public Task<bool> QuitarCartas(int id_jugador, int[] id_cartas);
    }
}
