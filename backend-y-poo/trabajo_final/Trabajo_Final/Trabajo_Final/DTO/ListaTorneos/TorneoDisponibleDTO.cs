﻿using System.ComponentModel.DataAnnotations;

namespace Trabajo_Final.DTO.ListaTorneos
{
    public class TorneoDisponibleDTO
    {
        public int id { get; set; }
        public DateTime fecha_hora_inicio { get; set; }

        public DateTime fecha_hora_fin { get; set; }

        public string horario_diario_inicio { get; set; }

        public string horario_diario_fin { get; set; }

        public string pais { get; set; }

        public string[] series_habilitadas { get; set; }


    }
}
