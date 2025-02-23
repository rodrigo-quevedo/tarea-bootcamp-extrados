﻿using DAO.Entidades.Custom.Partida_CantidadRondas;
using DAO.Entidades.PartidaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAOs.Partidas
{
    public interface IPartidaDAO
    {
        public Task<Partida_CantidadRondasDTO> BuscarDatosParaOficializar(Partida partida);
        public Task<IEnumerable<Partida>> BuscarPartidasParaOficializar(int id_juez);

        public Task<bool> VerificarUltimaPartidaDeRonda(
            int id_partida,
            int id_torneo,
            int ronda);

        public Task<bool> OficializarResultado(int id_partida, int id_ganador, int? id_descalificado);

        public Task<bool> OficializarFinal(
            int id_partida, 
            int id_ganador, int? id_descalificado, 
            int id_torneo,
            string faseFinalizado);

    }
}
