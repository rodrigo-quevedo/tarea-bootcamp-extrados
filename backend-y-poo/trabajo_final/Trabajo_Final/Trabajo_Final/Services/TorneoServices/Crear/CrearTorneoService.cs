
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.ComponentModel.DataAnnotations;
using Trabajo_Final.Services.TorneoServices.ValidarJueces;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.Crear
{
    public class CrearTorneoService : ICrearTorneoService
    {
        private ITorneoDAO torneoDAO;
        private IValidarJuecesService validarJuecesService;
        private ICartaDAO cartaDAO;

        public CrearTorneoService(ITorneoDAO torneoDao, IValidarJuecesService validarJueces, ICartaDAO cartaDao) { 
            torneoDAO = torneoDao;
            validarJuecesService = validarJueces;
            cartaDAO = cartaDao;
        }

        public async Task<bool> CrearTorneo(
            int id_organizador, 
            string str_fecha_hora_inicio, string str_fecha_hora_fin,
            string horario_inicio, string horario_fin,
            string pais,
            string[] series_habilitadas,
            int[] id_jueces
        )
        {
            //Parsear strings a DateTime, por defecto se asume
            //que no hay fechaHora_fin

            DateTime fecha_hora_inicio = DateTime.Parse(str_fecha_hora_inicio, null, System.Globalization.DateTimeStyles.RoundtripKind);
            ValidarHorario(horario_inicio, horario_fin, fecha_hora_inicio);
            
            DateTime? fecha_hora_fin = null;
            
            int? cantidad_rondas = null;


            //Si existe fecha_hora_fin, se calculan las rondas maximas:
            if (str_fecha_hora_fin != null)
            {
                fecha_hora_fin = DateTime.Parse(str_fecha_hora_fin, null, System.Globalization.DateTimeStyles.RoundtripKind);
                ValidarHorario(horario_inicio, horario_fin, (DateTime)fecha_hora_fin);
                int totalMinutosTorneo = VerificarDuracionTorneo(fecha_hora_inicio, (DateTime) fecha_hora_fin);
                cantidad_rondas = CalcularCantidadRondas(totalMinutosTorneo);
            }

            
            await validarJuecesService.ValidarIdsJueces(id_jueces);

            await ValidarSeriesHabilitadas(series_habilitadas);

            
            return await torneoDAO.CrearTorneo(
                id_organizador,
                fecha_hora_inicio, fecha_hora_fin,
                horario_inicio, horario_fin,
                cantidad_rondas,
                pais,
                FasesTorneo.REGISTRO,
                series_habilitadas,
                id_jueces
            );
  
        }


        private int VerificarDuracionTorneo(DateTime fecha_hora_inicio, DateTime fecha_hora_fin
        )
        {
            //calcular la cantidad de rondas
            int totalMinutos =
                (int)fecha_hora_fin.Subtract(fecha_hora_inicio).TotalMinutes;

            if (totalMinutos < 30) throw new DuracionInvalidaException("El torneo debe durar como mínimo 30 minutos.");

            return totalMinutos;
        }

        private int CalcularCantidadRondas(int totalMinutos) 
        {
            int cantidad_partidas_max = totalMinutos / 30; //tiempo maximo de partida: 30 min

            //juegos: 1, 1+2^1, 3+2^2, 7+2^3
            //      = 1,     3,     7,    15
            //rondas: 1,     2,     3,     4
            bool calcular_siguiente_ronda = true;
            int cantidad_partidas = 1;
            int cantidad_rondas = 1;

            while (calcular_siguiente_ronda)
            {
                int cantidad_partidas_en_siguiente_ronda =
                    cantidad_partidas + (int)Math.Pow(2, cantidad_rondas);


                if (cantidad_partidas_en_siguiente_ronda > cantidad_partidas_max)
                {
                    //calcular_siguiente_ronda = false;
                    break;
                }


                cantidad_partidas = cantidad_partidas_en_siguiente_ronda;
                cantidad_rondas++;
            }

            //Console.WriteLine($"minutos: {totalMinutos}");
            //Console.WriteLine($"cant partidas maximas: {cantidad_partidas_max}");
            //Console.WriteLine($"partidas: {cantidad_partidas}");
            //Console.WriteLine($"rondas: {cantidad_rondas}");
            //Console.WriteLine($"cantidad maxima jugadores: {(int)Math.Pow(2,cantidad_rondas)}");

            return cantidad_rondas;
        }



        private async Task<bool> ValidarSeriesHabilitadas(string[] series_habilitadas)
        {
            //Validar que las series habilitadas existan
            IEnumerable<string> nombres_series = await cartaDAO.ObtenerNombresSeries();
            foreach (string serie in series_habilitadas)
            {
                if (!nombres_series.Contains(serie))
                    throw new InvalidInputException($"No hay ninguna serie [{serie}].");
            }

            return true;
        }

        private bool ValidarHorario(
            string horario_inicio, string horario_fin,
            DateTime fechaHora)
        {
            //Horario inicio
            string str_horario_inicio_horas = horario_inicio.Substring(0,2);
            string str_horario_inicio_minutos = horario_inicio.Substring(3, 2);

            Int32.TryParse(str_horario_inicio_horas, out int horario_inicio_horas);
            Int32.TryParse(str_horario_inicio_minutos, out int horario_inicio_minutos);


            DateTime fecha_hora_inicio =
                fechaHora.Date
                    .AddHours(horario_inicio_horas)
                    .AddMinutes(horario_inicio_minutos);


            //Horario fin
            string str_horario_fin_horas = horario_inicio.Substring(0, 2);
            string str_horario_fin_minutos = horario_inicio.Substring(3, 2);

            Int32.TryParse(str_horario_fin_horas, out int horario_fin_horas);
            Int32.TryParse(str_horario_fin_minutos, out int horario_fin_minutos);


            DateTime fecha_hora_fin =
                fechaHora.Date
                    .AddHours(horario_fin_horas)
                    .AddMinutes(horario_fin_minutos);


            //Logica
            if (fecha_hora_inicio < fecha_hora_fin) //ej. 08:00 a 23:30
            {
                return 
                (
                    fechaHora >= fecha_hora_inicio 
                    && 
                    fechaHora <= fecha_hora_fin
                );
            }

            if (fecha_hora_inicio > fecha_hora_fin) //ej. 20:00 a 04:30
            {
                return 
                (
                    fechaHora >= fecha_hora_inicio
                    ||
                    fechaHora <= fecha_hora_fin
                );
            }

            return true; //fecha_hora_inicio == fecha_hora_fin: abierto 24hs

        }








    }
}
