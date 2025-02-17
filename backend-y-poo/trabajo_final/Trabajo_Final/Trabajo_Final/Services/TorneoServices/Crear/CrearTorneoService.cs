
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
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
            //parsear DateTimes y verificar
            DateTime fecha_hora_inicio = DateTime.Parse(str_fecha_hora_inicio, null, System.Globalization.DateTimeStyles.RoundtripKind);
            if (!ValidarHorario(horario_inicio, horario_fin, fecha_hora_inicio))
                throw new InvalidInputException($"fecha_hora_inicio: {str_fecha_hora_inicio} no respeta el horario.");
            
            DateTime fecha_hora_fin = DateTime.Parse(str_fecha_hora_fin, null, System.Globalization.DateTimeStyles.RoundtripKind);
            if (!ValidarHorario(horario_inicio, horario_fin, fecha_hora_fin))
                throw new InvalidInputException($"fecha_hora_fin: {str_fecha_hora_fin} no respeta el horario.");


            if (fecha_hora_inicio.AddMinutes(30) > fecha_hora_fin) throw new DuracionInvalidaException("El torneo debe durar como mínimo 30 minutos.");



            int totalMinutosTorneo = CalcularMinutosTorneo(
                fecha_hora_inicio, fecha_hora_fin,
                horario_inicio, horario_fin);
            
            int cantidad_rondas = CalcularCantidadRondas(totalMinutosTorneo);

            
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


        private int CalcularMinutosTorneo(
            DateTime fecha_hora_inicio, DateTime fecha_hora_fin,
            string horario_inicio, string horario_fin
        )

        {
            int totalMinutos = 0;

            //Mismo dia:
            if (fecha_hora_inicio.Date == fecha_hora_fin.Date) {
                totalMinutos = (int)fecha_hora_fin.Subtract(fecha_hora_inicio).TotalMinutes;
                Console.WriteLine($"Total minutos (mismo dia): {totalMinutos}");
                return totalMinutos;
            }


            //Diferentes días:
                //minuos primer dia +
                //minutos ultimo dia +
                //(minutos del horario * dias intermedios)


            //primer dia: fechaHora de fin del 1er dia - fechaHora de inicio del 1er dia
            DateTime horario_fin_en_1er_dia = 
                ParseHorario(horario_fin, fecha_hora_inicio);

            int minutos_1er_dia =
                (int) horario_fin_en_1er_dia.Subtract(fecha_hora_inicio).TotalMinutes;


            //ultimo dia: fechaHora de fin del ultimo dia - fechaHora de inicio del ultimo dia
            DateTime horario_inicio_en_ultimo_dia = 
                ParseHorario(horario_inicio, fecha_hora_fin);
            
            int minutos_ultimo_dia = 
                (int) fecha_hora_fin.Subtract(horario_inicio_en_ultimo_dia).TotalMinutes;


            //dias entre medio: cantidad de minutos en el horario * dias intermedios
            DateTime hoy = DateTime.Now;
            DateTime horario_inicio_hoy = ParseHorario(horario_inicio, hoy);
            DateTime horario_fin_hoy = ParseHorario(horario_fin, hoy);
            
            int minutos_en_horario = 
                (int) horario_fin_hoy.Subtract(horario_inicio_hoy).TotalMinutes;


            DateTime parsed_fecha_hora_fin = ParseHorario("00:00", fecha_hora_fin);
            DateTime parsed_fecha_hora_inicio = ParseHorario("00:00", fecha_hora_inicio);

            int cantidad_dias_intermedios =
                (int) parsed_fecha_hora_fin.Subtract(parsed_fecha_hora_inicio)
                .TotalDays - 1; //Resto un día para no incluir el último día (ya calculado)


            int minutos_dias_entre_medio = minutos_en_horario * cantidad_dias_intermedios;

            totalMinutos = minutos_1er_dia + minutos_ultimo_dia + minutos_dias_entre_medio;

            Console.WriteLine($"Minutos 1er dia: {minutos_1er_dia}");
            Console.WriteLine($"Minutos ultimo dia: {minutos_ultimo_dia}");
            Console.WriteLine($"Minutos dias entre medio: {minutos_dias_entre_medio}");
            Console.WriteLine($"Total minutos: {totalMinutos}");

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

            Console.WriteLine($"minutos: {totalMinutos}");
            Console.WriteLine($"cant partidas maximas: {cantidad_partidas_max}");
            Console.WriteLine($"partidas: {cantidad_partidas}");
            Console.WriteLine($"rondas: {cantidad_rondas}");
            Console.WriteLine($"cantidad maxima jugadores: {(int)Math.Pow(2,cantidad_rondas)}");

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
            DateTime horario_fechahora_inicio = ParseHorario(horario_inicio, fechaHora);


            //Horario fin
            DateTime horario_fechahora_fin = ParseHorario(horario_fin, fechaHora);


            //Logica
            if (horario_fechahora_inicio < horario_fechahora_fin) //ej. 08:00 a 23:30
            {
                return 
                (
                    fechaHora >= horario_fechahora_inicio //ej. [08:01] > 08:00
                    && 
                    fechaHora <= horario_fechahora_fin    //ej. [23:29] = 23:30
                );
            }

            if (horario_fechahora_inicio > horario_fechahora_fin) //ej. 20:00 a 04:30
            {
                return 
                (
                    fechaHora >= horario_fechahora_inicio  //ej. [23:15] > 20:00
                    ||
                    fechaHora <= horario_fechahora_fin     //ej. [00:04] < 04:30
                );
            }

            return true; //horario_inicio == horario_fin: abierto 24hs

        }

        private DateTime ParseHorario(string horario, DateTime datetime)
        {
            string str_horario_horas = horario.Substring(0, 2);
            string str_horario_minutos = horario.Substring(3, 2);

            Int32.TryParse(str_horario_horas, out int horario_horas);
            Int32.TryParse(str_horario_minutos, out int horario_minutos);


            return datetime.Date
                    .AddHours(horario_horas)
                    .AddMinutes(horario_minutos);
        }






    }
}
