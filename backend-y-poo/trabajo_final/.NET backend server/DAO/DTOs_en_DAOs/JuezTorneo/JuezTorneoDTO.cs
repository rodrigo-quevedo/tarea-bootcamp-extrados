using DAO.Entidades.TorneoEntidades;

namespace DAO.DTOs_en_DAOs.JuezTorneo
{
    public class JuezTorneoDTO : Juez_Torneo
    {
        public bool Activo { get; set; }
        public string Rol { get; set; }
    }
}
