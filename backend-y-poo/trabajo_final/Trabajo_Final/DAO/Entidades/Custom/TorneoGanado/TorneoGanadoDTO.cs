using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades.Custom.TorneoGanado
{
    public class TorneoGanadoDTO
    {
        public int Id { get; set; }
        public DateTime Fecha_hora_inicio { get; set; }
        public DateTime Fecha_hora_fin { get; set; }
        public string Horario_diario_inicio { get; set; }
        public string Horario_diario_fin { get; set; }
        public int Cantidad_rondas { get; set; }
        public string Pais { get; set; }
        public int Id_ganador { get; set; }
        public string[] Series_habilitadas { get; set; }
        public int[] Id_jugadores { get; set; }
        public int[] Id_cartas_mazo { get; set; }
    }
}
