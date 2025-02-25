using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.PartidaEntidades
{
    public class Partida
    {
        public int Id { get; set; }
        public int Ronda { get; set; }
        public int Id_torneo {  get; set; }
        public DateTime Fecha_hora_inicio { get; set; }
        public DateTime Fecha_hora_fin {  get; set; }
        public int Id_jugador_1 {  get; set; }
        public int Id_jugador_2 { get; set; }
        public int? Id_ganador { get; set; }
        public int? Id_descalificado {  get; set; }
        public string? Motivo_descalificacion { get; set; }
        public int Id_juez {  get; set; }


    }
}
