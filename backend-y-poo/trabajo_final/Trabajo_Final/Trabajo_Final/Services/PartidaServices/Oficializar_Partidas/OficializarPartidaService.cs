using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Partidas;
using DAO.Entidades.Custom.Partida_CantidadRondas;
using DAO.Entidades.PartidaEntidades;
using Trabajo_Final.utils.Constantes;

namespace Trabajo_Final.Services.PartidaServices.Oficializar_Partidas
{
    public class OficializarPartidaService : IOficializarPartidaService
    {
        IPartidaDAO partidaDAO;
        public OficializarPartidaService(IPartidaDAO partidaDao)
        {
            partidaDAO = partidaDao;
        }

        public async Task<bool> OficializarPartida(
            int id_juez, 
            int id_partida, 
            int id_ganador, 
            int? id_descalificado)
        {
            //buscar partida y cantidad de rondas del torneo
            //(necesito la ronda de la partida para ver si es la ultima partida de esa ronda)
            Partida_CantidadRondasDTO datosPartida = 
                await partidaDAO.BuscarDatosParaOficializar(new Partida() { 
                    Id = id_partida, 
                    Id_juez = id_juez });

            if (datosPartida == null) throw new InvalidInputException($"El juez [{id_juez}] no está asignado a la partida con id [{id_partida}].");


            //verificar que no este oficializada aún
            if (datosPartida.Id_ganador != null) throw new InvalidInputException($"La partida [{id_partida}] ya ha sido oficializada.");


            //verificar fechaHora de oficializacion
            DateTime tiempoOficializacionUTC = DateTime.UtcNow;
            if (datosPartida.Fecha_hora_inicio >= tiempoOficializacionUTC)
                throw new InvalidInputException("La partida aún no ha comenzado, no se puede oficializar.");
            

            //(?) se puede verificar ganador/descalificado, pero ya está en el CHECK de la tabla

           
            //verificar si es ultima partida de ronda
            bool ultimaPartidaDeRonda = await partidaDAO.VerificarUltimaPartidaDeRonda(
                id_partida,
                datosPartida.Id_torneo,
                datosPartida.Ronda);

            //--->No es ultima partida de la ronda:
            if (!ultimaPartidaDeRonda) //DAO: UPDATE partida
                return await partidaDAO.OficializarResultado(id_partida, id_ganador, id_descalificado);


            //--->Es ultima partida de ronda:
            //Verificar FINAL     
            bool esFinal = datosPartida.Ronda == datosPartida.Cantidad_rondas;
            
            //---> es final
            if (esFinal) //DAO: UPDATE partida +  UPDATE torneo (fase = 'finalizado')
                return await partidaDAO.OficializarFinal(
                    id_partida, 
                    id_ganador, id_descalificado, 
                    datosPartida.Id_torneo, 
                    FasesTorneo.FINALIZADO);



            //--->NO es final: crear partidas de la siguiente ronda
                
            //buscar torneo (horario_diario_inicio y horario_diario_fin)
            //buscar jueces
            //buscar jugadores


            //armar fechahoras

            //armar partidas

            //DAO: UPDATE partida + INSERT partidas


            throw new NotImplementedException();
        }
    }
}
