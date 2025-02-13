
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using System.ComponentModel.DataAnnotations;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.TorneoServices.Crear
{
    public class CrearTorneoService : ICrearTorneoService
    {
        private ITorneoDAO torneoDAO;
        private IUsuarioDAO usuarioDAO;
        private ICartaDAO cartaDAO;
        public CrearTorneoService(ITorneoDAO torneoDao, IUsuarioDAO usuarioDao, ICartaDAO cartaDao) { 
            torneoDAO = torneoDao;
            usuarioDAO = usuarioDao;
            cartaDAO = cartaDao;
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

            if (totalMinutos < 30) throw new DuracionInvalidaException("El torneo debe durar como mínimo 30 minutos.");

            int cantidad_partidas_max = totalMinutos / 30; //tiempo maximo de partida: 30 min

            //juegos: 1, 1+2^1, 3+2^2, 7+2^3
            //      = 1,     3,     7,    15
            //rondas: 1,     2,     3,     4
            bool calcular_siguiente_ronda = true;
            int cantidad_partidas = 1; 
            int cantidad_rondas = 1;
            
            while (calcular_siguiente_ronda) {
                int cantidad_partidas_en_siguiente_ronda =
                    cantidad_partidas + (int)Math.Pow(2, cantidad_rondas);


                if ( cantidad_partidas_en_siguiente_ronda > cantidad_partidas_max){
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

            //Tengo que validar que todos los id pertenecen a jueces activos
            //(ya que no tengo tabla jueces, y el id_juez apunta a usuarios(id),
            //el INSERT va a guardar cualquier ID).
            IEnumerable<int> id_jueces_validos =
                await usuarioDAO.BuscarIDsUsuarios(new Usuario() { Rol = Roles.JUEZ, Activo = true });

            foreach (int id_juez in id_jueces)
            {
                if (!id_jueces_validos.Contains(id_juez))
                    throw new InvalidInputException($"No hay ningun juez con id [{id_juez}].");
            }


            //Validar que las series habilitadas existan
            IEnumerable<string> nombres_series = await cartaDAO.ObtenerNombresSeries();
            foreach (string serie in series_habilitadas)
            {
                if (!nombres_series.Contains(serie))
                    throw new InvalidInputException($"No hay ninguna serie [{serie}].");
            }


            return await torneoDAO.CrearTorneo(
                id_organizador,
                fecha_hora_inicio, fecha_hora_fin,
                cantidad_rondas,
                pais,
                FasesTorneo.REGISTRO,
                series_habilitadas,
                id_jueces
            );

        }
    }
}
