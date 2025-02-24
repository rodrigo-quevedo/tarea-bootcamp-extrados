namespace DAO.Entidades.Custom
{
    public class InsertPartidaDTO
    {
        public int Ronda { get; set; }
        public int Id_torneo { get; set; }
        public DateTime Fecha_hora_inicio { get; set; }
        public DateTime Fecha_hora_fin { get; set; }
        public int Id_jugador_1 { get; set; }
        public int Id_jugador_2 { get; set; }
        public int Id_juez { get; set; }
    }
}
