using Configuration.ServerURL;
using DAO.DAOs.Cartas;
using DAO.Entidades.Cartas;
using Microsoft.AspNetCore.Mvc;
using Trabajo_Final.DTO.Response.Cartas;

namespace Trabajo_Final.Services.CartasServices.BuscarCartas
{
    public class BuscarCartasService : IBuscarCartasService
    {
        private ICartaDAO cartaDAO;
        private IServerURLConfiguration serverURLConfig;
        public BuscarCartasService(ICartaDAO cartaDao, IServerURLConfiguration serverURLConfig) 
        {
            this.cartaDAO = cartaDao;
            this.serverURLConfig = serverURLConfig;
        }

        public async Task<IEnumerable<DatosCartaDTO>> BuscarCartas(int[] id_cartas)
        {
            IEnumerable<Carta> cartas =  await cartaDAO.BuscarCartas(id_cartas);
            
            IEnumerable<Serie_De_Carta> series_de_cartas = await cartaDAO.BuscarSeriesDeCartas(id_cartas);

            if (cartas == null || !cartas.Any() || series_de_cartas == null || !series_de_cartas.Any())
                throw new Exception("No se pudo obtener ninguna informacion de las id_cartas buscadas.");

            IList<DatosCartaDTO> result = new List<DatosCartaDTO>();
            foreach(Carta carta in cartas)
            {
                result.Add(new DatosCartaDTO()
                {
                    Id = carta.Id,
                    Ataque = carta.Ataque,
                    Defensa = carta.Defensa,
                    Ilustracion = serverURLConfig.GetServerURL() + carta.Ilustracion,
                    Series = series_de_cartas
                            .Where(s => s.Id_carta == carta.Id)
                            .Select(s => s.Nombre_serie)
                            .ToArray()
                });
            }

            return result.AsEnumerable();
        }
    }
}
