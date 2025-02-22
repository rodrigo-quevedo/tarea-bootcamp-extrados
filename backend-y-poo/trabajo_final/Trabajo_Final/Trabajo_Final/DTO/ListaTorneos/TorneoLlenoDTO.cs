﻿using DAO.Entidades.TorneoEntidades;
using System.Runtime.CompilerServices;

namespace Trabajo_Final.DTO.ListaTorneos
{
    public class TorneoLlenoDTO: Torneo
    {
        public string[] series_habilitadas { get; set; }

        public int[] id_jueces_torneo { get; set; }

        public int[] id_jugadores_inscriptos { get; set; }
    }
}
