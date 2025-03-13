using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Response.TorneoResponseDTO
{
    //Jugador: ver a qué torneos se puede inscribir
    public class TorneoDisponibleDTO : TorneoBaseDTO
    {
        public int Max_cantidad_rondas { get; set; }
        public string[] Series_habilitadas { get; set; }

    }
}
