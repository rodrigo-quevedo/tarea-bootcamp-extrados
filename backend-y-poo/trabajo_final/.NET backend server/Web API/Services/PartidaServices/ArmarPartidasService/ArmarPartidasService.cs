﻿using DAO.DTOs_en_DAOs.InsertPartidas;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.TorneoEntidades;
using System.Text.Json;
using Trabajo_Final.DTO.ProcesamientoDatos.FechaHoraPartidas;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.utils.Horarios;

namespace Trabajo_Final.Services.PartidaServices.ArmarPartidasService
{
    public class ArmarPartidasService : IArmarPartidasService
    {

        public IEnumerable<FechaHoraPartida> ArmarFechaHoraPartidas(
            DateTime fecha_hora_inicio,
            string horario_diario_inicio,
            string horario_diario_fin,
            int cantidad_partidas)
        {
            IList<FechaHoraPartida> fechaHora_partidas = new List<FechaHoraPartida>();

            int partida_pointer = 1;

            DateTime momento_pointer = fecha_hora_inicio;


            while (partida_pointer <= cantidad_partidas)
            {
                DateTime inicio_partida = momento_pointer;
                DateTime fin_partida = momento_pointer.AddMinutes(30);

                //Averiguar hora de cierre del dia actual
                DateTime momento_cierre =
                    ManejarHorarios.ParseHorario(
                        horario_diario_fin,
                        momento_pointer);

                //Averiguar si la partida excede el horario de cierre
                if (momento_pointer.AddMinutes(30) > momento_cierre)
                {   //Si lo excede:

                    momento_pointer.AddDays(1); //voy al dia siguiente

                    momento_pointer = //voy al inicio de ese dia
                        ManejarHorarios.ParseHorario(
                            horario_diario_inicio,
                            momento_pointer);

                    inicio_partida = momento_pointer;//seteo el inicio de partida

                    fin_partida = momento_pointer.AddMinutes(30);//seteo el final de partida
                    
                }

                //Mover el pointer del inicio de partida al final:
                momento_pointer = momento_pointer.AddMinutes(30);


                fechaHora_partidas.Add(new FechaHoraPartida()
                {
                    fecha_hora_inicio = inicio_partida,
                    fecha_hora_fin = fin_partida
                });

                Console.WriteLine($"{inicio_partida} -> {fin_partida}");
                partida_pointer++;
            }

            return fechaHora_partidas;
        }


        public IEnumerable<InsertPartidaDTO> ArmarPartidas_JugadoresAleatorios(
           int id_torneo,
           IList<FechaHoraPartida> fechaHoraPartidas,
           IList<Jugador_Inscripto> jugadoresPartida,
           IList<Juez_Torneo> jueces,
           int ronda)
        {
            IList<InsertPartidaDTO> partidas = new List<InsertPartidaDTO>();

            Random rnd = new Random();

            foreach (FechaHoraPartida fechahora in fechaHoraPartidas)
            {
                //sortear jugadores
                int index_jugador_1 = rnd.Next(jugadoresPartida.Count);
                Jugador_Inscripto j1 = jugadoresPartida[index_jugador_1];
                jugadoresPartida.RemoveAt(index_jugador_1);


                int index_jugador_2 = rnd.Next(jugadoresPartida.Count);
                Jugador_Inscripto j2 = jugadoresPartida[index_jugador_2];
                jugadoresPartida.RemoveAt(index_jugador_2);


                //sortear juez
                int index_juez = rnd.Next(jueces.Count);
                Juez_Torneo juez = jueces[index_juez];

                //armar partida
                partidas.Add(new InsertPartidaDTO()
                {
                    Ronda = ronda,
                    Id_torneo = id_torneo,
                    Id_jugador_1 = j1.Id_jugador,
                    Id_jugador_2 = j2.Id_jugador,
                    Fecha_hora_inicio = fechahora.fecha_hora_inicio,
                    Fecha_hora_fin = fechahora.fecha_hora_fin,
                    Id_juez = juez.Id_juez
                });
            }

            return partidas;
        }

        public IEnumerable<InsertPartidaDTO> ArmarPartidas_JugadoresEnOrdenCronologico(
            int id_torneo, 
            IList<FechaHoraPartida> fechaHoraPartidas, 
            IList<Partida> ganadores_ronda_anterior, 
            IList<Juez_Torneo> jueces, 
            int ronda)
        {
            IList<InsertPartidaDTO> partidas = new List<InsertPartidaDTO>();

            Random rnd = new Random();

            foreach (FechaHoraPartida fechahora in fechaHoraPartidas)
            {
                //asignar jugadores en orden cronologico
                int id_jugador_1 = (int) ganadores_ronda_anterior[0].Id_ganador; 
                ganadores_ronda_anterior.RemoveAt(0);

                int id_jugador_2 = (int)ganadores_ronda_anterior[0].Id_ganador;
                ganadores_ronda_anterior.RemoveAt(0);

                //sortear juez
                Juez_Torneo juez = jueces[rnd.Next(jueces.Count)];

                //armar partida
                partidas.Add(new InsertPartidaDTO()
                {
                    Ronda = ronda,
                    Id_torneo = id_torneo,
                    Id_jugador_1 = id_jugador_1,
                    Id_jugador_2 = id_jugador_2,
                    Fecha_hora_inicio = fechahora.fecha_hora_inicio,
                    Fecha_hora_fin = fechahora.fecha_hora_fin,
                    Id_juez = juez.Id_juez
                });

            }


            return partidas;
        }
    
    
    }
}
