using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.Response.ResponseDTO
{
    //Jugador: ver a qué torneos se puede inscribir
    public class TorneoDisponibleDTO : TorneoBaseDTO
    {
        public string[] Series_habilitadas { get; set; }

    }
}
