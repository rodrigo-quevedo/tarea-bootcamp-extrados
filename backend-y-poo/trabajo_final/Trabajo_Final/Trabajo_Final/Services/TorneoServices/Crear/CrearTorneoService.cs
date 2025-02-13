
using DAO.DAOs.Torneos;
using System.ComponentModel.DataAnnotations;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.Crear
{
    public class CrearTorneoService : ICrearTorneoService
    {
        private ITorneoDAO torneoDAO;
        public CrearTorneoService(ITorneoDAO dao) { 
            torneoDAO = dao;
        }

        public async Task<bool> CrearTorneo(
            int id_organizador, 
            DateTime fecha_hora_inicio, DateTime fecha_hora_fin, 
            string pais,
            string[] series_habilitadas,
            int[] id_jueces
        )
        {
            //calcular la cantidad de rondas
            int totalMinutos = 
                (int) fecha_hora_fin.Subtract(fecha_hora_inicio).TotalMinutes;

            int cant_max_partidas = totalMinutos / 30; //tiempo maximo de partida: 30 min

            
            int contador_partidas = 1;
            int cant_rondas_maxima = 1;
            while (contador_partidas <= cant_max_partidas) {
                contador_partidas = contador_partidas + (int)Math.Pow(2, cant_rondas_maxima);
                                       //juegos: 1,3,7,15
                                       //juegos: 1,1+2^1,3+2^2,7+2^3
                
                cant_rondas_maxima++;  //rondas: 1,2,3,4
            }

            //deshacer ultimo ciclo (lo hice sobrar para que corte el while)
            cant_rondas_maxima--;
            contador_partidas = contador_partidas - (int)Math.Pow(2,cant_rondas_maxima);
            //Jugadores en primera ronda: 2^(cant_rondas_maxima)

            //Console.WriteLine($"minutos: {totalMinutos}");
            //Console.WriteLine($"cant partidas maximas: {cant_max_partidas}");
            //Console.WriteLine($"contador de partidas: {contador_partidas}");
            //Console.WriteLine($"cantidad de rondas: {cant_rondas_maxima}");
            //Console.WriteLine($"cantidad maxima jugadores: {(int)Math.Pow(2,cant_rondas_maxima)}");


            return await torneoDAO.CrearTorneo(
                id_organizador,
                fecha_hora_inicio, fecha_hora_fin,
                cant_rondas_maxima,
                pais,
                FasesTorneo.REGISTRO,
                series_habilitadas,
                id_jueces
            );

        }
    }
}
