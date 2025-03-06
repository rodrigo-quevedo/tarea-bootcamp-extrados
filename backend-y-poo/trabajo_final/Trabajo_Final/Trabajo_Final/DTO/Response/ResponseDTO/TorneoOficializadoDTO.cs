namespace Trabajo_Final.DTO.Response.ResponseDTO
{
    //Juez: ver torneos que oficializó
    public class TorneoOficializadoDTO : TorneoBaseDTO
    {
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jugadores { get; set; }
        public int Id_ganador { get; set; }
        public int[] Id_partidas_oficializadas { get; set; }
    }
}
