namespace Trabajo_Final.DTO.ListaTorneos.ResponseDTO
{
    public class TorneoBaseDTO
    {
        public int Id_torneo { get; set; }
        public DateTime Fecha_hora_inicio { get; set; }
        public DateTime Fecha_hora_fin { get; set; }
        public string Horario_diario_inicio { get; set; }
        public string Horario_diario_fin { get; set; }
        public int Cantidad_rondas { get; set; }
        public string Pais { get; set; }
    }
}
