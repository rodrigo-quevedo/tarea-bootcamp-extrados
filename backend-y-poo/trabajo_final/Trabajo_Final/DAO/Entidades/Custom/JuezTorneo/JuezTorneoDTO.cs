using DAO.Entidades.TorneoEntidades;

namespace DAO.Entidades.Custom.JuezTorneo
{
    public class JuezTorneoDTO : Juez_Torneo
    {
        public bool Activo { get; set; }
        public string Rol { get; set; }
    }
}
