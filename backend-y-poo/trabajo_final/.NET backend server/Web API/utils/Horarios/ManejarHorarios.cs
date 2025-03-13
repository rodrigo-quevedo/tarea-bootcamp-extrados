namespace Trabajo_Final.utils.Horarios
{
    public class ManejarHorarios
    {
        public static DateTime ParseHorario(string horario, DateTime datetime)
        {
            string str_horario_horas = horario.Substring(0, 2);
            string str_horario_minutos = horario.Substring(3, 2);

            Int32.TryParse(str_horario_horas, out int horario_horas);
            Int32.TryParse(str_horario_minutos, out int horario_minutos);


            return datetime.Date //este objeto datetime ya viene en formato UTC
                    .AddHours(horario_horas)
                    .AddMinutes(horario_minutos);
        }

        public static bool ValidarHorario(
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
    }
}
