namespace DAO.Entidades.TorneoEntidades
{
    public class Torneo
    {
        public int Id {  get; set; }
        public DateTime Fecha_hora_inicio { get; set; }
        public DateTime Fecha_hora_fin { get; set; }
        public string Horario_diario_inicio { get; set; }
        public string Horario_diario_fin { get; set; }
        public int Cantidad_rondas {  get; set; }
        public int Max_cantidad_rondas { get; set; }
        public string Pais { get; set; }
        public string Fase { get; set; }
        public int Id_organizador { get; set; }
    }
}
